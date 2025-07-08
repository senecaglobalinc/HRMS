using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using HRMS.Admin.Infrastructure.Models.Response;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    public class MenuService : IMenuService
    {
        #region Global Variables
        private readonly ILogger<MenuService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_mapper;
        private IHttpClientFactory m_clientFactory;
        #endregion

        #region constructor
        public MenuService(ILogger<MenuService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;
        }
        #endregion

        #region GetMenuDetails
        public async Task<ServiceListResponse<MenuData>> GetMenuDetails(string roleName)
        {
            //usp_GetMenuDetailsByRoles
            var response = new ServiceListResponse<MenuData>();

            var childTable = await (from mm in m_AdminContext.MenuMaster
                                    join mr in m_AdminContext.MenuRoles on mm.MenuId equals mr.MenuId
                                    join r in m_AdminContext.Roles on mr.RoleId equals r.RoleId
                                    where mm.IsActive == true && mr.IsActive == true && r.IsActive == true && r.RoleName == roleName
                                    select mm).ToListAsync();
            var parentIds = childTable.Select(x => x.ParentId).Distinct().ToList();
            childTable.AddRange(await (from mp in m_AdminContext.MenuMaster
                                       where mp.IsActive == true && mp.ParentId == 0 && parentIds.Contains(mp.MenuId)
                                       select mp).ToListAsync());
            childTable.AddRange(await (from am in m_AdminContext.AllMenus
                                       join mm in m_AdminContext.MenuMaster on am.MenuId equals mm.MenuId
                                       //where mm.IsActive == true
                                       select mm).ToListAsync());
            var menuIds = childTable.Select(x => x.MenuId).Distinct();
            var menuData = await (from mm in m_AdminContext.MenuMaster
                                  where mm.IsActive == true && menuIds.Contains(mm.MenuId)
                                  select new MenuData
                                  {
                                      MenuId = mm.MenuId,
                                      Title = mm.Title,
                                      IsActive = mm.IsActive,
                                      Path = mm.Path,
                                      DisplayOrder = mm.DisplayOrder,
                                      ParentId = mm.ParentId,
                                      Parameter = mm.Parameter,
                                      NodeId = mm.NodeId,
                                      Style = mm.Style
                                  }).ToListAsync();

            response.Items = menuData.Where(menu => menu.ParentId == 0).OrderBy(x => x.DisplayOrder).ToList();

            foreach (var menuItem in response.Items)
            {
                buildTreeviewMenu(menuItem, menuData);
            }
            return response;
        }

        private void buildTreeviewMenu(MenuData menuItem, IEnumerable<MenuData> menudata)
        {
            IEnumerable<MenuData> _menuItems;

            _menuItems = menudata.Where(menu => menu.ParentId == menuItem.MenuId);

            if (_menuItems != null && _menuItems.Count() > 0)
            {
                foreach (var item in _menuItems)
                {
                    menuItem.Categories.Add(item);
                    buildTreeviewMenu(item, menudata);
                }
            }
        }
        #endregion

        #region GetSourceMenuRoles
        public async Task<ServiceListResponse<Menus>> GetSourceMenuRoles(int roleId)
        {
            //usp_GetSourceMenusByRoleId
            var response = new ServiceListResponse<Menus>();
            if (roleId == 0)
            {
                response.Message = "RoleId is Required!.";
                return response;
            }
            response.Items = await (from mm in m_AdminContext.MenuMaster
                                    where mm.IsActive == true && mm.ParentId != 0 && !(from mr in m_AdminContext.MenuRoles
                                                                                       where mr.RoleId == roleId && mr.IsActive == true
                                                                                       select mr.MenuId).Contains(mm.MenuId)
                                    select new Menus
                                    {
                                        MenuId = mm.MenuId,
                                        MenuName = mm.Title
                                    }).ToListAsync();
            return response;
        }
        #endregion

        #region GetTargetMenuRoles
        public async Task<ServiceListResponse<Menus>> GetTargetMenuRoles(int roleId)
        {
            //usp_GetTargetMenusByRoleId
            var response = new ServiceListResponse<Menus>();
            if (roleId == 0)
            {
                response.Message = "RoleId is Required!.";
                return response;
            }
            response.Items = await (from mm in m_AdminContext.MenuMaster
                                    join mr in m_AdminContext.MenuRoles on mm.MenuId equals mr.MenuId
                                    where mr.RoleId == roleId && mm.IsActive == true && mr.IsActive == true
                                    select new Menus
                                    {
                                        MenuId = mr.MenuId,
                                        MenuName = mm.Title
                                    }).ToListAsync();
            return response;
        }
        #endregion

        #region UpdateTargetMenuRoles
        /// <summary>
        /// UpdateTargetMenuRoles
        /// </summary>
        /// <param name="menuRoles"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> AddTargetMenuRoles(MenuRoleDetails menuRoles)
        {
            var response = new BaseServiceResponse();
            using (var trans = m_AdminContext.Database.BeginTransaction())
            {
                try
                {
                    //usp_DeleteMenuRoles and usp_AddTargetMenuRoles
                    //Delete Existing MenuRoles
                    var menuRolesList = await m_AdminContext.MenuRoles.Where(x => x.RoleId == menuRoles.RoleId).ToListAsync();
                    m_AdminContext.MenuRoles.RemoveRange(menuRolesList);
                    response.IsSuccessful = await m_AdminContext.SaveChangesAsync() > 0 ? true : false;
                    List<MenuRole> lstMenuRole = new List<MenuRole>();
                    menuRoles.MenuList.ForEach(x => lstMenuRole.Add(new MenuRole { MenuId = x.MenuId, RoleId = menuRoles.RoleId, IsActive = true }));
                    await m_AdminContext.MenuRoles.AddRangeAsync(lstMenuRole);
                    await m_AdminContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception ex)
                {
                    m_Logger.LogInformation("Error while adding MenuRole " + ex.StackTrace);
                    response.IsSuccessful = false;
                    await trans.RollbackAsync();
                }
            }
            return response;
        }
        #endregion
    }
}
