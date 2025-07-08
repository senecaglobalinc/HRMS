using AutoMapper;
using HRMS.Common;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Domain;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeePersonalDetailsService : IEmployeePersonalDetailsService
    {
        #region Global Varibles

        private readonly ILogger<EmployeePersonalDetailsService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IEmployeeService m_EmployeeService;
        private readonly IKRAService m_KRAService;
        private readonly IServiceTypeToEmployeeService m_ServiceTypeToEmployeeService;
        private readonly IProjectService m_ProjectService;
        private readonly IEmployeeSkillService m_EmployeeSkillService;

        #endregion

        #region Constructor
        public EmployeePersonalDetailsService(EmployeeDBContext employeeDBContext,
                                ILogger<EmployeePersonalDetailsService> logger,
                                IOrganizationService orgService,
                                IEmployeeService employeeService,
                                IKRAService kraService,
                                IServiceTypeToEmployeeService serviceTypeToEmployeeService,
                                IProjectService projectService,
                                IEmployeeSkillService employeeSkillService)
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee.Entities.Employee, Employee.Entities.Employee>();
                cfg.CreateMap<EmployeePersonalDetails, Employee.Entities.Employee>();
            });
            m_mapper = config.CreateMapper();
            m_OrgService = orgService;
            m_EmployeeService = employeeService;
            m_KRAService = kraService;
            m_ServiceTypeToEmployeeService = serviceTypeToEmployeeService;
            m_ProjectService = projectService;
            m_EmployeeSkillService = employeeSkillService;
        }

        #endregion

        //TO DO LIST: mapper function needs to be used in some of the below methods to optimize the code, for that request object in the front end 
        //needs to be modified according to the Entitiy class.
        #region AddPersonalDetails
        /// <summary>
        /// Adds personal details and contacts of an employee and updates the data in prospective associate table.
        /// </summary>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeePersonalDetails>> AddPersonalDetails(EmployeePersonalDetails personalDetails)
        {
            var response = new ServiceResponse<EmployeePersonalDetails>();

            try
            {
                m_Logger.LogInformation("Calling AddPersonalDetails method in EmployeePersonalDetaiilsService");

                var prospectiveAssociate = m_EmployeeContext.ProspectiveAssociates.Where(e => e.Id == personalDetails.ID).FirstOrDefault();

                Entities.Employee existingEmployee = m_EmployeeContext.Employees.Where(code => code.EmployeeCode == personalDetails.EmployeeCode)
                                                                                .FirstOrDefault();
                if (existingEmployee == null)
                {
                    Entities.Employee newEmployee = new Entities.Employee();
                    newEmployee = m_mapper.Map<EmployeePersonalDetails, Entities.Employee>(personalDetails);

                    newEmployee.DateofBirth = Utility.GetDateTimeInIST(personalDetails.DateofBirth);
                    newEmployee.TelephoneNo = Utility.EncryptStringAES(personalDetails.TelephoneNo);
                    newEmployee.MobileNo = Utility.EncryptStringAES(personalDetails.MobileNo);
                    newEmployee.Pannumber = Utility.EncryptStringAES(personalDetails.Pannumber);
                    newEmployee.PassportNumber = Utility.EncryptStringAES(personalDetails.PassportNumber);
                    newEmployee.AadharNumber = Utility.EncryptStringAES(personalDetails.AadharNumber);
                    newEmployee.Pfnumber = Utility.EncryptStringAES(personalDetails.Pfnumber);
                    newEmployee.Uannumber = Utility.EncryptStringAES(personalDetails.Uannumber);
                    newEmployee.EmploymentStartDate = Utility.GetDateTimeInIST(personalDetails.EmploymentStartDate);
                    newEmployee.JoinDate = Utility.GetDateTimeInIST(personalDetails.JoinDate);

                    //newEmployee.Hradvisor = prospectiveAssociate != null ? prospectiveAssociate.HRAdvisorName : null; ;
                    //newEmployee.DepartmentId = personalDetails.DepartmentId == null ? prospectiveAssociate.DepartmentId 
                    //                                                                : personalDetails.DepartmentId;
                    //newEmployee.DesignationId = personalDetails.DesignationId == null ? prospectiveAssociate.DepartmentId  
                    //                                                                  :  personalDetails.DepartmentId;
                    //newEmployee.GradeId = personalDetails.GradeId == null ? prospectiveAssociate.GradeId : personalDetails.GradeId;
                    //newEmployee.JoinDate = personalDetails.JoinDate == null ? prospectiveAssociate.JoinDate : personalDetails.JoinDate;
                    //newEmployee.CompetencyGroup = personalDetails.TechnologyId == null ? prospectiveAssociate.TechnologyID : 
                    //                                              personalDetails.TechnologyId; //Here TechnologyID refers to PracticeAreaId

                    newEmployee.CompetencyGroup = personalDetails.TechnologyId;
                    newEmployee.CareerBreak = personalDetails.CareerBreak;
                    newEmployee.IsActive = true;
                    //ServiceType serviceType = new ServiceType()
                    //{
                    //    EmployeeId = personalDetails.EmployeeId,
                    //    ServiceTypeId = personalDetails.ServiceTypeId
                    //};
                    //ServiceResponse<int> isCreated =await m_ServiceTypeToEmployeeService.Create(serviceType);
                    //if(isCreated.Item==0)
                    //{
                    //    response.IsSuccessful = false;
                    //    response.Message = "Error while updating serviceTypeToEmployee service";
                    //    response.Item = null;
                    //    return response;
                    //}



                    m_EmployeeContext.Employees.Add(newEmployee);
                    int rowInserted = await m_EmployeeContext.SaveChangesAsync();
                    personalDetails.EmployeeId = newEmployee.EmployeeId;

                    if (rowInserted != 0)
                    {
                        //Updates data in the prospective associate table
                        prospectiveAssociate.EmployeeID = personalDetails.EmployeeId;
                        prospectiveAssociate.IsActive = false;
                        m_EmployeeContext.Entry(prospectiveAssociate).State = EntityState.Modified;

                        //Saves contacts of an employee
                        if (personalDetails.contacts != null)
                        {
                            AddAssociatePersonalContacts(personalDetails);
                        }
                        //Add roletype into EmployeeKRARoleTypeHistory table 
                        EmployeeKRARoleTypeHistory employeeKRARoleType = new EmployeeKRARoleTypeHistory();
                        if (personalDetails.RoleTypeId != null)
                        {
                            employeeKRARoleType.EmployeeId = personalDetails.EmployeeId;
                            employeeKRARoleType.RoleTypeId = (int)personalDetails.RoleTypeId;
                            employeeKRARoleType.RoleTypeValidFrom = DateTime.Now;
                            m_EmployeeContext.EmployeeKRARoleTypeHistory.Add(employeeKRARoleType);
                        }

                        await m_EmployeeContext.SaveChangesAsync();
                    }

                    //Commenting this section for further analysis.
                    //new Common().CreateHRMSRepository(personalDetails.empCode);
                    //AddKRARole(personalDetails.empCode, personalDetails.empID, personalDetails.KRARoleId); //Adding KRA Role to Associate
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "Employee code already exists.";
                    response.Item = personalDetails;
                    return response;
                }

                response.IsSuccessful = true;
                response.Message = "Employee personal details saved suceesfully,";
                response.Item = personalDetails;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to save personal details of an employee";
                m_Logger.LogError("Error occured in AddPersonalDetails() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }
            return response;

        }
        #endregion

        #region GetPersonalDetailsById
        /// <summary>
        /// Gets personal details by given employee Id.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeePersonalDetails>> GetPersonalDetailsById(int empId)
        {
            var response = new ServiceResponse<EmployeePersonalDetails>();
            try
            {
                m_Logger.LogInformation("Calling GetPersonalDetailsById method in EmployeePersonalDetaiilsService");

                var departments = await m_OrgService.GetAllDepartments();
                var employeeTypes = await m_EmployeeService.GetEmpTypes();
                var grades = await m_OrgService.GetAllGrades();
                var designations = await m_OrgService.GetAllDesignations();
                var employee = m_EmployeeContext.Employees.Where(e => e.EmployeeId == empId && e.IsActive == true).ToList();
                var contacts = m_EmployeeContext.Contacts.Where(c => c.EmployeeId == empId).ToList();
                int gradeId = (int)employee.Where(emp => emp.EmployeeId == empId).Select(emp => emp.GradeId).FirstOrDefault();

                //RoleType is KRA related and there is no available data so breaking the functionality so commenting 
                //var Roletype = await m_KRAService.GetEmpKRARoleTypeByGrade(gradeId);
                var personalDetails = (from e in employee
                                       join d in departments.Items on e.DepartmentId equals d.DepartmentId into dep
                                       from dept in dep.DefaultIfEmpty()
                                       join et in employeeTypes.Items on e.EmployeeTypeId equals et.EmployeeTypeId into etypes
                                       from etype in etypes.DefaultIfEmpty()
                                       join des in designations.Items on e.DesignationId equals des.DesignationId into designationsList
                                       from designation in designationsList.DefaultIfEmpty()
                                       join g in grades.Items on e.GradeId equals g.GradeId
                                       //join rt in Roletype.Items on e.RoleTypeId equals rt.RoleTypeId into roletypeList
                                       //from roltype in roletypeList.DefaultIfEmpty()
                                       select new EmployeePersonalDetails
                                       {
                                           EmployeeId = e.EmployeeId,
                                           EmployeeCode = e.EmployeeCode,
                                           //empName = e.FirstName + " " + e.LastName,
                                           FirstName = e.FirstName,
                                           LastName = e.LastName,
                                           MiddleName = e.MiddleName,
                                           TelephoneNo = Utility.DecryptStringAES(e.TelephoneNo),
                                           MobileNo = Utility.DecryptStringAES(e.MobileNo),
                                           BgvstatusId = e.BgvstatusId,
                                           Bgvstatus = e.Bgvstatus,
                                           PersonalEmailAddress = e.PersonalEmailAddress,
                                           DateofBirth = e.DateofBirth,
                                           Gender = e.Gender,
                                           JoinDate = e.JoinDate,
                                           MaritalStatus = e.MaritalStatus,
                                           BloodGroup = e.BloodGroup,
                                           Nationality = e.Nationality,
                                           Pannumber = Utility.DecryptStringAES(e.Pannumber),
                                           AadharNumber = Utility.DecryptStringAES(e.AadharNumber),
                                           Pfnumber = Utility.DecryptStringAES(e.Pfnumber),
                                           PassportNumber = Utility.DecryptStringAES(e.PassportNumber),
                                           Uannumber = Utility.DecryptStringAES(e.Uannumber),
                                           PassportDateValidUpto = e.PassportDateValidUpto,
                                           PassportIssuingOffice = e.PassportIssuingOffice,
                                           BgvinitiatedDate = e.BgvinitiatedDate,
                                           BgvcompletionDate = e.BgvcompletionDate,
                                           //Experience = e.Experience.ToString(),
                                           EmploymentStartDate = e.EmploymentStartDate,
                                           CareerBreak = e.CareerBreak,
                                           Hradvisor = e.Hradvisor,
                                           Designation = designation.DesignationName,
                                           DesignationId = designation.DesignationId,
                                           DepartmentId = e.DepartmentId,
                                           // department = dept.DepartmentCode,
                                           TechnologyId = e.CompetencyGroup,
                                           GradeId = e.GradeId,
                                           GradeName = g.GradeName,
                                           EmployeeTypeId = etype != null ? (int?)etype.EmployeeTypeId : null,
                                           ReportingManager = e.ReportingManager == null ? 0 : e.ReportingManager.Value,
                                           //RoleTypeId = roltype == null ? 0 : roltype.RoleTypeId,
                                           //RoleTypeName = roltype == null ? null : roltype.RoleTypeName

                                       }).FirstOrDefault();
                //personalDetails.Birthdate = personalDetails.dob != null ? Convert.ToDateTime(personalDetails.dob).
                //                                        ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : null;
                if (contacts.Count > 0)
                {
                    personalDetails.contactDetails = (from c in contacts
                                                      where c.AddressType == StringConstants.CurrentAddress
                                                      select new EmployeeContactDetails
                                                      {
                                                          ID = c.ID,
                                                          currentAddress1 = c.AddressLine1,
                                                          currentAddress2 = c.AddressLine2,
                                                          currentAddCity = c.City,
                                                          currentAddCountry = c.Country,
                                                          currentAddState = c.State,
                                                          currentAddZip = c.PostalCode,
                                                          addressType = c.AddressType,
                                                          permanentAddress1 = "",
                                                          permanentAddress2 = "",
                                                          permanentAddCity = "",
                                                          permanentAddCountry = "",
                                                          permanentAddState = "",
                                                          permanentAddZip = "",
                                                      }).AsEnumerable()
                                                      .Union(from c in contacts
                                                             where c.AddressType == StringConstants.PermanentAddress
                                                             select new EmployeeContactDetails
                                                             {
                                                                 ID = c.ID,
                                                                 currentAddress1 = "",
                                                                 currentAddress2 = "",
                                                                 currentAddCity = "",
                                                                 currentAddCountry = "",
                                                                 currentAddState = "",
                                                                 currentAddZip = "",
                                                                 addressType = c.AddressType,
                                                                 permanentAddress1 = c.AddressLine1,
                                                                 permanentAddress2 = c.AddressLine2,
                                                                 permanentAddCity = c.City,
                                                                 permanentAddCountry = c.Country,
                                                                 permanentAddState = c.State,
                                                                 permanentAddZip = c.PostalCode,
                                                             }).ToList();
                }
                //Commenting this part as we need to include KRARole related code here.
                //personalDetails.KRARoleId = GetKRARole(empID);

                response.IsSuccessful = true;
                response.Item = personalDetails;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to get personal details of an employee";
                m_Logger.LogError("Error occured in GetPersonalDetailsByID() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region UpdatePersonalDetails
        /// <summary>
        /// Updates personal details and contacts of an employee.
        /// </summary>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeePersonalDetails>> UpdatePersonalDetails(EmployeePersonalDetails personalDetails)
        {
            bool retValue = false;
            var response = new ServiceResponse<EmployeePersonalDetails>();
            int? NewRoletype = personalDetails.RoleTypeId;
            int? OldRoletype;
            try
            {
                m_Logger.LogInformation("Calling UpdatePersonalDetails method in EmployeePersonalDetailsService");
                Entities.Employee existingEmployee = m_EmployeeContext.Employees.FirstOrDefault(x => x.EmployeeId == personalDetails.EmployeeId);
                //existingEmployee = m_mapper.Map<EmployeePersonalDetails, Entities.Employee>(personalDetails);

                OldRoletype = existingEmployee.RoleTypeId;

                personalDetails.DateofBirth = Utility.GetDateTimeInIST(personalDetails.DateofBirth);
                personalDetails.TelephoneNo = Utility.EncryptStringAES(personalDetails.TelephoneNo);
                personalDetails.MobileNo = Utility.EncryptStringAES(personalDetails.MobileNo);
                personalDetails.Pannumber = Utility.EncryptStringAES(personalDetails.Pannumber);
                personalDetails.AadharNumber = Utility.EncryptStringAES(personalDetails.AadharNumber);
                personalDetails.Pfnumber = Utility.EncryptStringAES(personalDetails.Pfnumber);
                personalDetails.Uannumber = Utility.EncryptStringAES(personalDetails.Uannumber);
                personalDetails.PassportNumber = Utility.EncryptStringAES(personalDetails.PassportNumber);
                personalDetails.JoinDate = Utility.GetDateTimeInIST(personalDetails.JoinDate);
                personalDetails.EmploymentStartDate = Utility.GetDateTimeInIST(personalDetails.EmploymentStartDate);
                personalDetails.IsActive = true;
                personalDetails.StatusId = existingEmployee.StatusId;

                //personalDetails.CareerBreak = personalDetails.CareerBreak;
                personalDetails.CompetencyGroup = personalDetails.TechnologyId;
                personalDetails.WorkEmailAddress = existingEmployee.WorkEmailAddress;
                personalDetails.UserId = existingEmployee.UserId;

                AllocationPercentage associatePercentage = m_ProjectService.GetAllocationPercentage().Result?.Items.Where(p => p.Percentage == 100).FirstOrDefault();
                ServiceListResponse<AssociateAllocation> associateAllocation = await m_ProjectService.GetAssociateAllocationsByEmployeeId(personalDetails.EmployeeId);
                List<int?> talentPoolIds = m_ProjectService.GetAllTalentPoolData().Result?.Items.Select(x => x.ProjectId).ToList();

                if (associateAllocation.Items != null && associateAllocation.IsSuccessful && personalDetails.TechnologyId != null)
                {
                    var allocDtls = associateAllocation.Items
                           .Where(e => e.EmployeeId == personalDetails.EmployeeId && e.IsActive == true
                          && e.AllocationPercentage == associatePercentage.AllocationPercentageId && talentPoolIds.Contains(e.ProjectId.Value))
                           .FirstOrDefault();

                    if (associateAllocation.Items.Count == 1 && allocDtls != null)
                    {
                        var rmId = m_ProjectService.GetPMByPracticeAreaId(personalDetails.TechnologyId ?? 0).Result?.Item.ProgramManagerId;
                        personalDetails.ReportingManager = rmId != null ? rmId : personalDetails.ReportingManager;
                    }
                }

                personalDetails.CreatedBy = existingEmployee.CreatedBy;
                personalDetails.CreatedDate = existingEmployee.CreatedDate;
                //m_EmployeeContext.Entry(existingEmployee).State = EntityState.Modified;
                m_mapper.Map<EmployeePersonalDetails, Entities.Employee>(personalDetails, existingEmployee);

                //Saves or updates the current address of an employee
                Entities.Contacts contact = m_EmployeeContext.Contacts.FirstOrDefault(x => x.EmployeeId == personalDetails.EmployeeId &&
                                                                   (x.AddressType == StringConstants.CurrentAddress
                                                                   || x.AddressType == StringConstants.Current));
                if (contact != null)
                {
                    if (personalDetails.contacts != null)
                    {
                        if (personalDetails.contacts.currentAddress1 != null || personalDetails.contacts.currentAddress2 != null
                            || personalDetails.contacts.currentAddCity != null || personalDetails.contacts.currentAddCountry != null
                            || personalDetails.contacts.currentAddState != null || personalDetails.contacts.currentAddZip != null)
                        {
                            contact.AddressLine1 = personalDetails.contacts.currentAddress1;
                            contact.AddressLine2 = personalDetails.contacts.currentAddress2;
                            contact.City = personalDetails.contacts.currentAddCity;
                            contact.Country = personalDetails.contacts.currentAddCountry;
                            contact.State = personalDetails.contacts.currentAddState;
                            contact.PostalCode = personalDetails.contacts.currentAddZip;
                            contact.IsActive = true;
                        }
                    }

                    //else if (personalDetails.contacts != null)
                    //{
                    //    contact = new Entities.Contacts();
                    //    contact.EmployeeId = existingEmployee.EmployeeId;
                    //    contact.AddressType = StringConstants.CurrentAddress;
                    //    contact.AddressLine1 = personalDetails.contacts.currentAddress1;
                    //    contact.AddressLine2 = personalDetails.contacts.currentAddress2;
                    //    contact.City = personalDetails.contacts.currentAddCity;
                    //    contact.Country = personalDetails.contacts.currentAddCountry;
                    //    contact.State = personalDetails.contacts.currentAddState;
                    //    contact.PostalCode = personalDetails.contacts.currentAddZip;
                    //    contact.IsActive = true;
                    //    m_EmployeeContext.Contacts.Add(contact);
                    //}
                }
                else if (personalDetails.contacts != null)
                {
                    contact = new Entities.Contacts();
                    contact.EmployeeId = existingEmployee.EmployeeId;
                    contact.AddressType = StringConstants.CurrentAddress;
                    contact.AddressLine1 = personalDetails.contacts.currentAddress1;
                    contact.AddressLine2 = personalDetails.contacts.currentAddress2;
                    contact.City = personalDetails.contacts.currentAddCity;
                    contact.Country = personalDetails.contacts.currentAddCountry;
                    contact.State = personalDetails.contacts.currentAddState;
                    contact.PostalCode = personalDetails.contacts.currentAddZip;
                    contact.IsActive = true;
                    m_EmployeeContext.Contacts.Add(contact);
                }

                //Saves or updates the permanent address of an employee
                Entities.Contacts contactOne = m_EmployeeContext.Contacts.FirstOrDefault(x => x.EmployeeId == personalDetails.EmployeeId &&
                                                                (x.AddressType == StringConstants.PermanentAddress
                                                                || x.AddressType == StringConstants.Permanent));
                if (contactOne != null)
                {
                    if (personalDetails.contacts != null)
                    {
                        if (personalDetails.contacts.permanentAddress1 != null || personalDetails.contacts.permanentAddress2 != null
                            || personalDetails.contacts.permanentAddCity != null || personalDetails.contacts.permanentAddCountry != null
                            || personalDetails.contacts.permanentAddState != null || personalDetails.contacts.permanentAddZip != null)
                        {
                            contactOne.AddressLine1 = personalDetails.contacts.permanentAddress1;
                            contactOne.AddressLine2 = personalDetails.contacts.permanentAddress2;
                            contactOne.City = personalDetails.contacts.permanentAddCity;
                            contactOne.Country = personalDetails.contacts.permanentAddCountry;
                            contactOne.State = personalDetails.contacts.permanentAddState;
                            contactOne.IsActive = true;
                            contactOne.PostalCode = personalDetails.contacts.permanentAddZip;
                        }
                    }
                    //else if (personalDetails.contacts != null)
                    //{
                    //    contactOne = new Entities.Contacts();
                    //    contactOne.EmployeeId = existingEmployee.EmployeeId;
                    //    contactOne.AddressType = StringConstants.PermanentAddress;
                    //    contactOne.AddressLine1 = personalDetails.contacts.permanentAddress1;
                    //    contactOne.AddressLine2 = personalDetails.contacts.permanentAddress2;
                    //    contactOne.City = personalDetails.contacts.permanentAddCity;
                    //    contactOne.Country = personalDetails.contacts.permanentAddCountry;
                    //    contactOne.State = personalDetails.contacts.permanentAddState;
                    //    contactOne.PostalCode = personalDetails.contacts.permanentAddZip;
                    //    contactOne.CreatedDate = DateTime.Now;
                    //    contactOne.IsActive = true;
                    //    m_EmployeeContext.Contacts.Add(contactOne);
                    //}
                }
                else if (personalDetails.contacts != null)
                {
                    contactOne = new Entities.Contacts();
                    contactOne.EmployeeId = existingEmployee.EmployeeId;
                    contactOne.AddressType = StringConstants.PermanentAddress;
                    contactOne.AddressLine1 = personalDetails.contacts.permanentAddress1;
                    contactOne.AddressLine2 = personalDetails.contacts.permanentAddress2;
                    contactOne.City = personalDetails.contacts.permanentAddCity;
                    contactOne.Country = personalDetails.contacts.permanentAddCountry;
                    contactOne.State = personalDetails.contacts.permanentAddState;
                    contactOne.PostalCode = personalDetails.contacts.permanentAddZip;
                    contactOne.IsActive = true;
                    m_EmployeeContext.Contacts.Add(contactOne);
                }
                //ServiceType serviceType = new ServiceType()
                //{
                //    EmployeeId = personalDetails.EmployeeId,
                //    ServiceTypeId = personalDetails.ServiceTypeId
                //};
                //ServiceResponse<int> isCreated = await m_ServiceTypeToEmployeeService.Update(serviceType);
                //if (isCreated.Item == 0)
                //{
                //    response.IsSuccessful = false;
                //    response.Message = "Error while updating serviceTypeToEmployee service";
                //    response.Item = null;
                //    return response;
                //}
                retValue = await m_EmployeeContext.SaveChangesAsync() > 0 ? true : false;

                //Commenting this section for further analysis.
                //UpdateKRARole(personalDetails.empID, personalDetails.KRARoleId);

                if (retValue)
                {
                    //roletype is updating
                    if (OldRoletype != null && OldRoletype != NewRoletype)
                    {
                        EmployeeKRARoleTypeHistory existingKRARoleTypeHistory = m_EmployeeContext.EmployeeKRARoleTypeHistory.Where(roletype => roletype.RoleTypeId == OldRoletype
                        && roletype.EmployeeId == personalDetails.EmployeeId).FirstOrDefault();
                        EmployeeKRARoleTypeHistory employeeKRARoleType;
                        if (existingKRARoleTypeHistory == null)
                        {
                            //when no roletype exist in history
                            employeeKRARoleType = new EmployeeKRARoleTypeHistory();
                            employeeKRARoleType.EmployeeId = personalDetails.EmployeeId;
                            employeeKRARoleType.RoleTypeId = (int)NewRoletype;
                            employeeKRARoleType.RoleTypeValidFrom = DateTime.Now;
                            m_EmployeeContext.EmployeeKRARoleTypeHistory.Add(employeeKRARoleType);
                        }
                        else
                        {
                            //when updating the rolytype need update the existing role type validTo date.
                            existingKRARoleTypeHistory.RoleTypeValidTo = DateTime.Today.AddDays(-1);
                            m_EmployeeContext.EmployeeKRARoleTypeHistory.Update(existingKRARoleTypeHistory);
                            m_EmployeeContext.SaveChanges();

                            //when updating the rolytype add new roletype into another row.
                            if (NewRoletype != null)
                            {
                                employeeKRARoleType = new EmployeeKRARoleTypeHistory();
                                employeeKRARoleType.RoleTypeId = (int)NewRoletype;
                                employeeKRARoleType.EmployeeId = personalDetails.EmployeeId;
                                employeeKRARoleType.RoleTypeValidFrom = DateTime.Now;
                                m_EmployeeContext.EmployeeKRARoleTypeHistory.Add(employeeKRARoleType);
                            }

                        }

                        m_EmployeeContext.SaveChanges();
                    }
                    //roletype is adding
                    if (OldRoletype == null && NewRoletype != null)
                    {
                        EmployeeKRARoleTypeHistory employeeKRARoleType = new EmployeeKRARoleTypeHistory();
                        employeeKRARoleType.EmployeeId = personalDetails.EmployeeId;
                        employeeKRARoleType.RoleTypeId = (int)personalDetails.RoleTypeId;
                        employeeKRARoleType.RoleTypeValidFrom = DateTime.Now;
                        m_EmployeeContext.EmployeeKRARoleTypeHistory.Add(employeeKRARoleType);
                    }
                    m_EmployeeContext.SaveChanges();
                    response.IsSuccessful = true;
                    response.Message = "Successfully updated the personal details of an employee";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to update personal details of an employee";
                m_Logger.LogError("Error occured in UpdatePersonalDetails() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region ValidatePersonalData
        /// <summary>
        /// Validates personal details of an employee and checks whether the given data already exists or not.
        /// </summary>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        public string ValidatePersonalData(EmployeePersonalDetails personalDetails)
        {
            string message = null;
            try
            {
                m_Logger.LogError("Calling ValidatePersonalData() method in EmployeePersonalDetailsService ");
                if (!string.IsNullOrEmpty(personalDetails.PersonalEmailAddress))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.PersonalEmailAddress.ToLower() == personalDetails.PersonalEmailAddress.ToLower()).FirstOrDefault()) != null)
                        message = "Personal email";

                if (!string.IsNullOrEmpty(personalDetails.MobileNo))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.MobileNo.ToLower() == Utility.EncryptStringAES(personalDetails.MobileNo).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "Mobile number" : "Mobile number";

                if (!string.IsNullOrEmpty(personalDetails.AadharNumber))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.AadharNumber.ToLower() == Utility.EncryptStringAES(personalDetails.AadharNumber).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "Aadhar number" : "Aadhar number";

                if (!string.IsNullOrEmpty(personalDetails.Pannumber))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                         e.Pannumber.ToLower() == Utility.EncryptStringAES(personalDetails.Pannumber).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "Pan number" : "Pan number";

                if (!string.IsNullOrEmpty(personalDetails.Pfnumber))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.Pfnumber.ToLower() == Utility.EncryptStringAES(personalDetails.Pfnumber).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "Pf number" : "Pf number";


                if (!string.IsNullOrEmpty(personalDetails.Uannumber))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.Uannumber.ToLower() == Utility.EncryptStringAES(personalDetails.Uannumber).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "UAN number" : "UAN number";


                if (!string.IsNullOrEmpty(personalDetails.PassportNumber))
                    if ((m_EmployeeContext.Employees.Where(e => e.EmployeeId != personalDetails.EmployeeId && e.IsActive == true &&
                        e.PassportNumber.ToLower() == Utility.EncryptStringAES(personalDetails.PassportNumber).ToLower()).FirstOrDefault()) != null)
                        message = message != null ? message + ", " + "Passport Number" : "Passport Number";

                if (message != null)
                {
                    message = message.Contains(',') ? message + " are already exist" : message + " is already exist";
                }
            }
            catch (Exception ex)
            {
                message = "Failed to validate";
                m_Logger.LogError("Error occured in ValidatePersonalData() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return message;
        }
        #endregion

        #region UpdateReporting ManagerId
        /// <summary>
        /// Update Reporting Manager after project allocation.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="reportingManagerId"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateReportingManagerId(int employeeId, int reportingManagerId)
        {
            bool isUpdated = false;
            var response = new BaseServiceResponse();

            try
            {
                m_Logger.LogInformation("Calling UpdateReportingManagerId method in EmployeePersonalDetailsService");

                var employee = m_EmployeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId && x.IsActive == true);
                if (employee != null && employee.ReportingManager != reportingManagerId)
                {
                    employee.ReportingManager = reportingManagerId;
                    isUpdated = await m_EmployeeContext.SaveChangesAsync() > 0 ? true : false;
                }

                if (isUpdated)
                {
                    response.IsSuccessful = true;
                    response.Message = "Successfully updated the ReportingManagerId of an employee";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to update ReportingManagerId of an employee";
                m_Logger.LogError("Error occured in UpdateReportingManagerId() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region UpdateExternal
        /// <summary>
        /// Update External data
        /// </summary>
        /// <param name="employeeDetails"></param>
        /// <returns></returns>
        public async Task<BaseServiceResponse> UpdateExternal(EmployeeDetails employeeDetails)
        {
            var response = new BaseServiceResponse();
            response.IsSuccessful = false;
            try
            {
                m_Logger.LogInformation("Calling UpdateExternal method in EmployeePersonalDetailsService");

                var employee = m_EmployeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeDetails.EmployeeId && x.IsActive == true);
                if (employee != null)
                {
                    if (employeeDetails.External == "ReportingManager")
                    {
                        employee.ReportingManager = employeeDetails.ReportingManagerId;
                    }
                    else if (employeeDetails.External == "Associate Release")
                    {
                        employee.CompetencyGroup = employeeDetails.CompetencyGroup;
                        employee.DepartmentId = employeeDetails.DepartmentId;
                    }

                    response.IsSuccessful = await m_EmployeeContext.SaveChangesAsync() > 0 ? true : false;
                }

                if (response.IsSuccessful)
                {
                    response.Message = "Successfully updated employee";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Failed to update employee";
                m_Logger.LogError("Error occured in UpdateExternal() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region AddAssociatePersonalContacts
        /// <summary>
        /// Adds personal contacts of an employee.
        /// </summary>
        /// <param name="personalDetails"></param>
        public void AddAssociatePersonalContacts(EmployeePersonalDetails personalDetails)
        {
            try
            {
                m_Logger.LogError("Calling AddAssociatePersonalContacts() method in EmployeePersonalDetailsService.");

                //Saves the current address of an employee
                if (personalDetails.contacts.currentAddress1 != null || personalDetails.contacts.currentAddress2 != null
                                          || personalDetails.contacts.currentAddCity != null || personalDetails.contacts.currentAddCountry != null
                                          || personalDetails.contacts.currentAddState != null || personalDetails.contacts.currentAddZip != null)
                {
                    Entities.Contacts contactOne = new Entities.Contacts();
                    contactOne.EmployeeId = personalDetails.EmployeeId;
                    contactOne.AddressType = StringConstants.CurrentAddress;
                    contactOne.AddressLine1 = personalDetails.contacts.currentAddress1;
                    contactOne.AddressLine2 = personalDetails.contacts.currentAddress2;
                    contactOne.City = personalDetails.contacts.currentAddCity;
                    contactOne.Country = personalDetails.contacts.currentAddCountry;
                    contactOne.State = personalDetails.contacts.currentAddState;
                    contactOne.PostalCode = personalDetails.contacts.currentAddZip;
                    m_EmployeeContext.Contacts.Add(contactOne);
                }

                //Saves the permanent address of an employee
                if (personalDetails.contacts.permanentAddress1 != null || personalDetails.contacts.permanentAddress2 != null
                    || personalDetails.contacts.permanentAddCity != null || personalDetails.contacts.permanentAddCountry != null
                    || personalDetails.contacts.permanentAddState != null || personalDetails.contacts.permanentAddZip != null)
                {
                    Entities.Contacts contactTwo = new Entities.Contacts();
                    contactTwo.EmployeeId = personalDetails.EmployeeId;
                    contactTwo.AddressType = StringConstants.PermanentAddress;
                    contactTwo.AddressLine1 = personalDetails.contacts.permanentAddress1;
                    contactTwo.AddressLine2 = personalDetails.contacts.permanentAddress2;
                    contactTwo.City = personalDetails.contacts.permanentAddCity;
                    contactTwo.Country = personalDetails.contacts.permanentAddCountry;
                    contactTwo.State = personalDetails.contacts.permanentAddState;
                    contactTwo.PostalCode = personalDetails.contacts.permanentAddZip;
                    m_EmployeeContext.Contacts.Add(contactTwo);
                }
                //m_EmployeeContext.SaveChanges();
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in AddAssociatePersonalContacts() method in EmployeePersonalDetailsService " + ex.StackTrace);
            }
        }
        #endregion

        #region GetEmployeeDetailsDashBoard
        public async Task<ServiceResponse<EmployeeDetailsDashboard>> GetEmployeeDetailsDashboard(int empId)
        {
            var response = new ServiceResponse<EmployeeDetailsDashboard>();
            var empDetailsObj = new EmployeeDetailsDashboard();
            try
            {
                m_Logger.LogInformation("Calling GetPersonalDetailsByEmpId method in EmployeePersonalDetaiilsService");

                empDetailsObj.EmployeePersonalDetails = await GetPersonalDetailsByEmpId(empId).ConfigureAwait(false);
                empDetailsObj.EmployeeAllocationDetails = await GetAllocationDetailsByEmpId(empId).ConfigureAwait(false);
                empDetailsObj.EmployeeSkillDetails = await GetSkillsByEmpId(empId).ConfigureAwait(false);
                empDetailsObj.EmployeeFileDetails = await GetFileDetailsByEmpId(empId).ConfigureAwait(false);

                response.IsSuccessful = true;
                response.Item = empDetailsObj;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Failed to get personal details of an employee";
                m_Logger.LogError("Error occured in GetPersonalDetailsByEmpId method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return response;
        }
        #endregion

        //private methods
        #region PrivateMethods
        //Get personal details by emp id
        private async Task<EmployeePersonalDetails> GetPersonalDetailsByEmpId(int empId)
        {
            EmployeePersonalDetails employeePersonalDetails = new EmployeePersonalDetails();
            try
            {
                m_Logger.LogInformation("Calling GetPersonalDetailsByEmpId method in EmployeePersonalDetaiilsService");

                var empDetails = await GetPersonalDetailsById(empId);
                employeePersonalDetails = empDetails.Item;

                return employeePersonalDetails;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetPersonalDetailsByEmpId method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return null;
        }

        //Get project details by emp id
        private async Task<IList<AssociateAllocation>> GetAllocationDetailsByEmpId(int empId)
        {
            try
            {
                m_Logger.LogInformation("Calling GetAllocationDetailsByEmpId method in EmployeePersonalDetaiilsService");

                var allocationDetails = await m_ProjectService.GetAssociateAllocationsByEmployeeId(empId);
                return allocationDetails.Items;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetAllocationDetailsByEmpId method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return null;
        }

        //Get personal details by emp id
        private async Task<IList<EmployeeSkillDetails>> GetSkillsByEmpId(int empId)
        {
            try
            {
                m_Logger.LogInformation("Calling GetSkillsByEmpId method in EmployeePersonalDetaiilsService");

                var skillsDetails = await m_EmployeeSkillService.GetByEmployeeId(empId);
                return skillsDetails.Items;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetSkillsByEmpId method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return null;
        }

        //Get personal details by emp id
        private async Task<EmployeePersonalDetails> GetFileDetailsByEmpId(int empId)
        {
            EmployeePersonalDetails employeePersonalDetails = new EmployeePersonalDetails();
            try
            {
                m_Logger.LogInformation("Calling GetPersonalDetailsByEmpId method in EmployeePersonalDetaiilsService");

                var empDetails = await GetPersonalDetailsById(empId);
                employeePersonalDetails = empDetails.Item;

                return employeePersonalDetails;
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occured in GetPersonalDetailsByEmpId method in EmployeePersonalDetailsService " + ex.StackTrace);
            }

            return null;
        }
        #endregion
    }
}
