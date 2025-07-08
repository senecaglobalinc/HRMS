using AutoMapper;
using HRMS.Admin.Database;
using HRMS.Admin.Entities;
using HRMS.Admin.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Admin.Service
{
    /// <summary>
    /// Service class to get the category master
    /// </summary>
    public class CategoryMasterService : ICategoryMasterService
    {
        #region Global Variables 

        private readonly ILogger<CategoryMasterService> m_Logger;
        private readonly AdminContext m_AdminContext;
        private readonly IMapper m_Mapper;

        #endregion

        #region Constructor
        public CategoryMasterService(ILogger<CategoryMasterService> logger, AdminContext adminContext)
        {
            m_Logger = logger;
            m_AdminContext = adminContext;

            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CategoryMaster, CategoryMaster>();
            });

            m_Mapper = config.CreateMapper();
        }
        #endregion

        #region Create
        /// <summary>
        /// This method create category master. 
        /// </summary>
        /// <param name="categoryMasterIn">Category master information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Create(CategoryMaster categoryMasterIn)
        {
            try
            {
                int isCreated;
                m_Logger.LogInformation("CategoryMasterService: Calling \"Create\" method.");
                m_Logger.LogInformation("CategoryMasterService: Verifying category master already exists?");

                CategoryMaster categoryMasterAlreadyExits =
                    await GetByCategoryName(categoryMasterIn.CategoryName);

                m_Logger.LogInformation("CategoryMasterService: Verifying category master exists?");
                if (categoryMasterAlreadyExits != null)
                    return CreateResponse(null, false, "Category name already exists.");
                else
                    m_Logger.LogInformation("CategoryMasterService: Category name does not already exists.");

                CategoryMaster parentCategory = null;
                if (categoryMasterIn.ParentId > 0)
                {
                    parentCategory = await GetParentCategoryByCategoryMasterId(categoryMasterIn.ParentId);

                    if (parentCategory == null)
                        return CreateResponse(null, false, "Parent category not found.");
                    else
                        m_Logger.LogInformation("CategoryMasterService: Parent category found.");
                }
                else
                    m_Logger.LogInformation("CategoryMasterService: Parent category not sent.");

                m_Logger.LogInformation("CategoryMasterService: Update feilds.");

                CategoryMaster categoryMaster = new CategoryMaster();

                if (!categoryMasterIn.IsActive.HasValue)
                    categoryMasterIn.IsActive = true;

                m_Logger.LogInformation("CategoryMasterService: assigning to automapper.");

                m_Mapper.Map<CategoryMaster, CategoryMaster>(categoryMasterIn, categoryMaster);

                m_Logger.LogInformation("CategoryMasterService: Add category master to list.");

                m_AdminContext.Categories.Add(categoryMaster);

                m_Logger.LogInformation("CategoryMasterService: Calling SaveChangesAsync method on DBContext.");

                isCreated = await m_AdminContext.SaveChangesAsync();

                if (isCreated > 0)
                {
                    m_Logger.LogInformation("CategoryMasterService: category master created successfully.");

                    return CreateResponse(categoryMaster, true, string.Empty);

                }
                else
                    return CreateResponse(null, false, "No category master created.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Delete
        /// <summary>
        /// This method deactivates category master.
        /// </summary>
        /// <param name="CategoryMasterId"></param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Delete(int CategoryMasterId)
        {
            int isUpdated;

            m_Logger.LogInformation("CategoryMasterService: Calling \"Delete\" method");

            //Fetch category master for update
            var CategoryMaster = m_AdminContext.Categories.Find(CategoryMasterId);

            //category master exists?
            if (CategoryMaster == null)
                return CreateResponse(null, false, "Category master not found for delete.");

            CategoryMaster.IsActive = false;

            m_Logger.LogInformation("CategoryMasterService: Calling SaveChanges method on DB Context.");
            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("CategoryMasterService: Updating category master record.");
                return CreateResponse(null, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }
        #endregion

        #region GetAll
        /// <summary>
        /// This method fetches all category master based on isActive flag.
        /// </summary>
        /// <param name="isActive">Is active or not</param>
        /// <returns>List<CategoryMaster></returns>
        public async Task<List<CategoryMaster>> GetAll(bool? isActive)
        {
            m_Logger.LogInformation("CategoryMasterService: Calling \"GetAll\" method.");

            if (!isActive.HasValue)
                return await (from cm in m_AdminContext.Categories
                              join pcm in m_AdminContext.Categories on cm.ParentId equals pcm.CategoryMasterId into parent
                              from pcm in parent.DefaultIfEmpty()
                              orderby cm.CategoryName
                              select new CategoryMaster
                              {
                                  CategoryMasterId = cm.CategoryMasterId,
                                  CategoryName = cm.CategoryName,
                                  ParentId = cm.ParentId,
                                  ParentCategoryName = pcm != null ? pcm.CategoryName : string.Empty
                              }).OrderBy(x => x.CategoryName).ToListAsync();
            else
                return await (from cm in m_AdminContext.Categories
                              join pcm in m_AdminContext.Categories on cm.ParentId equals pcm.CategoryMasterId into parent
                              from pcm in parent.DefaultIfEmpty()
                              where cm.IsActive == isActive
                              orderby cm.CategoryName
                              select new CategoryMaster
                              {
                                  CategoryMasterId = cm.CategoryMasterId,
                                  CategoryName = cm.CategoryName,
                                  ParentId = cm.ParentId,
                                  ParentCategoryName = pcm != null ? pcm.CategoryName : string.Empty
                              }).OrderBy(x => x.CategoryName).ToListAsync();
        }
        #endregion

        #region GetByCategoryMasterId
        /// <summary>
        /// this method fetches category master based on category Id.
        /// </summary>
        /// <param name="CategoryMasterId">category Id</param>
        /// <returns>CategoryMaster</returns>
        public async Task<CategoryMaster> GetByCategoryMasterId(int? CategoryMasterId) =>
            await m_AdminContext.Categories.Where(cm => cm.CategoryMasterId == CategoryMasterId)
                        .FirstOrDefaultAsync();

        #endregion

        #region GetByCategoryName
        /// <summary>
        /// this method fetches category master based on category Name.
        /// </summary>
        /// <param name="categoryName">Category Name</param>
        /// <returns>CategoryMaster</returns>
        public async Task<CategoryMaster> GetByCategoryName(string categoryName) =>
            await m_AdminContext.Categories.Where(cm => cm.CategoryName == categoryName)
                        .FirstOrDefaultAsync();
        #endregion

        #region GetParentCategoies
        /// <summary>
        /// This method fetches all parent categories.
        /// </summary>
        /// <returns>List<CategoryMaster></returns>
        public async Task<List<CategoryMaster>> GetParentCategoies()
        {
            m_Logger.LogInformation("CategoryMasterService: Calling \"GetParentCategoies\" method.");

            return await m_AdminContext.Categories.Where(cm => cm.ParentId == 0).OrderBy(cm => cm.CategoryName).ToListAsync();
        }
        #endregion

        #region Update
        /// <summary>
        /// This method updates category master.
        /// </summary>
        /// <param name="categoryMasterIn">category master information</param>
        /// <returns>dynamic</returns>
        public async Task<dynamic> Update(CategoryMaster categoryMasterIn)
        {
            int isUpdated;

            m_Logger.LogInformation("CategoryMasterService: Calling \"Update\" method.");
            m_Logger.LogInformation("Fetching category master for update.");

            var categoryMaster = m_AdminContext.Categories.Find(categoryMasterIn.CategoryMasterId);

            m_Logger.LogInformation("CategoryMasterService: category master exists?");

            if (categoryMaster == null)
                return CreateResponse(null, false, "Category master not found for update.");
            else
                m_Logger.LogInformation("CategoryMasterService: Category master found.");

            m_Logger.LogInformation("CategoryMasterService: Category name already exists?");

            CategoryMaster categoryMasterAlreadyExits =
               await GetByCategoryName(categoryMasterIn.CategoryName);

            if (categoryMasterAlreadyExits != null &&
                categoryMasterAlreadyExits.CategoryMasterId != categoryMaster.CategoryMasterId)
                return CreateResponse(null, false, "Category name already exists.");
            else
                m_Logger.LogInformation("CategoryMasterService: Category name does not already exists");

            CategoryMaster parentCategory = null;
            if (categoryMasterIn.ParentId > 0)
            {
                parentCategory = await GetParentCategoryByCategoryMasterId(categoryMasterIn.ParentId);

                if (parentCategory == null)
                    return CreateResponse(null, false, "Parent category not found.");
                else
                    m_Logger.LogInformation("CategoryMasterService: Parent category found.");
            }
            else
                m_Logger.LogInformation("CategoryMasterService: Parent category not sent.");

            m_Logger.LogInformation("CategoryMasterService: Update feilds.");

            if (!categoryMasterIn.IsActive.HasValue)
                categoryMasterIn.IsActive = categoryMaster.IsActive;
                categoryMasterIn.CreatedBy = categoryMaster.CreatedBy;
                categoryMasterIn.CreatedDate = categoryMaster.CreatedDate;

            m_Logger.LogInformation("CategoryMasterService: assigning to automapper.");

            m_Mapper.Map<CategoryMaster, CategoryMaster>(categoryMasterIn, categoryMaster);

            m_Logger.LogInformation("CategoryMasterService: Calling SaveChanges method on DB Context.");

            isUpdated = await m_AdminContext.SaveChangesAsync();

            if (isUpdated > 0)
            {
                m_Logger.LogInformation("CategoryMasterService: Updating Category name record.");

                return CreateResponse(categoryMaster, true, string.Empty);
            }
            else
                return CreateResponse(null, false, "No record updated.");
        }

        #endregion

        //Helpers
        #region CreateResponse
        private dynamic CreateResponse(CategoryMaster categoryMaster, bool isSuccessful, string message)
        {
            m_Logger.LogInformation("CategoryMasterService: Calling CreateResponse method.");

            dynamic response = new ExpandoObject();
            response.CategoryMaster = categoryMaster;
            response.IsSuccessful = isSuccessful;
            response.Message = message;

            m_Logger.LogInformation("CategoryMasterService: Response object created.");

            return response;
        }
        #endregion

        #region GetParentCategoryByCategoryMasterId
        /// <summary>
        /// This method fetches all parent categories.
        /// </summary>
        /// <param name="CategoryMasterId">category Id</param>
        /// <returns>List<CategoryMaster></returns>
        private async Task<CategoryMaster> GetParentCategoryByCategoryMasterId(int? CategoryMasterId)
        {
            m_Logger.LogInformation("CategoryMasterService: Calling \"GetParentCategoryByCategoryMasterId\" method.");

            return await m_AdminContext.Categories.Where(cm => cm.CategoryMasterId == CategoryMasterId && cm.ParentId == 0).FirstOrDefaultAsync();
        }
        #endregion
    }
}
