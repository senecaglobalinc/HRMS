using AutoMapper;
using HRMS.Common;
using HRMS.Employee.Database;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Entities;

namespace HRMS.Employee.Service
{
    public class EmployeeFamilyDetailsService: IEmployeeFamilyDetailsService
    {
        #region Global Varibles

        private readonly ILogger<EmployeeFamilyDetailsService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;

        #endregion

        #region Constructor
        public EmployeeFamilyDetailsService(EmployeeDBContext employeeDBContext,
                                ILogger<EmployeeFamilyDetailsService> logger
            )
        {
            m_EmployeeContext = employeeDBContext;
            m_Logger = logger;
            //CreateMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee.Entities.Employee, Employee.Entities.Employee>();
                cfg.CreateMap<EmergencyContactDetails, EmergencyContactDetails>();
            });
            m_mapper = config.CreateMapper();
        }

        #endregion

        #region UpdateFamilyDetails
        /// <summary>
        /// Updates family details of an employee
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeePersonalDetails>> UpdateFamilyDetails(EmployeePersonalDetails details)
        {
            bool retValue = false;
            var response = new ServiceResponse<EmployeePersonalDetails>();

            try
            {
                m_Logger.LogInformation("Calling UpdateFamilyDetails method in EmployeeFamilyDetailsService");
                var duplicateFamilyDetails = await m_EmployeeContext.FamilyDetails.Where(d => d.EmployeeId == details.EmployeeId).ToListAsync();

                duplicateFamilyDetails.ForEach(d => d.IsActive = false);

                //Updates relation details of an employee
                foreach (FamilyDetails relationDetails in details.RelationsInfo)
                {
                    FamilyDetails fd = await m_EmployeeContext.FamilyDetails.SingleOrDefaultAsync(x => x.Id == relationDetails.Id 
                                                                                                           && x.EmployeeId == details.EmployeeId);
                    if (!Object.ReferenceEquals(fd, null))
                    {
                        fd = m_mapper.Map<FamilyDetails, FamilyDetails>(relationDetails);
                        //fd.RelationShip = relationDetails.RelationShip;
                        //fd.Occupation = relationDetails.Occupation;
                        fd.Id = 0;
                        fd.EmployeeId = details.EmployeeId;
                        fd.Name = Utility.EncryptStringAES(relationDetails.Name);
                        fd.DateOfBirth = relationDetails.DateOfBirth;
                        fd.IsActive = true;
                        m_EmployeeContext.FamilyDetails.Add(fd);
                        //m_EmployeeContext.Entry(fd).State = EntityState.Modified;
                    }
                    else
                    {
                        fd = new Entities.FamilyDetails();
                        fd = m_mapper.Map<FamilyDetails, FamilyDetails>(relationDetails);
                        //fd.RelationShip = relationDetails.RelationShip;
                        //fd.Occupation = relationDetails.Occupation;

                        fd.EmployeeId = details.EmployeeId;
                        fd.Name = Utility.EncryptStringAES(relationDetails.Name);
                        fd.DateOfBirth = relationDetails.DateOfBirth;
                        fd.IsActive = true;
                        m_EmployeeContext.FamilyDetails.Add(fd);
                    }
                }

                //Updates emergency contact details of an employee
                List<EmergencyContactDetails> ecd = new List<EmergencyContactDetails>();
                if (details.contactDetailsOne != null)
                    ecd.Add(details.contactDetailsOne);
                if (details.contactDetailsTwo != null)
                    ecd.Add(details.contactDetailsTwo);

                foreach (EmergencyContactDetails contactData in ecd)
                {
                    if (!Object.ReferenceEquals(contactData, null))
                    {
                        EmergencyContactDetails contact = m_EmployeeContext.EmergencyContactDetails
                                                                 .SingleOrDefault(x => x.EmployeeId == details.EmployeeId && x.Id == contactData.Id);
                        if (!Object.ReferenceEquals(contact, null))
                        {
                            
                            contactData.TelePhoneNo = Utility.EncryptStringAES(contactData.TelePhoneNo);
                            contactData.MobileNo = Utility.EncryptStringAES(contactData.MobileNo);
                            contactData.ContactType = (bool)contactData.IsPrimary ? StringConstants.PrimaryContact : string.Empty;
                            m_mapper.Map<EmergencyContactDetails, EmergencyContactDetails>(contactData, contact);
                            //m_EmployeeContext.Entry(contact).State = EntityState.Modified;
                            
                        }
                        else
                        {
                            contact = new Entities.EmergencyContactDetails();
                            contact = m_mapper.Map<EmergencyContactDetails, EmergencyContactDetails>(contactData);
                            //contact.ContactName = contactData.contactName;
                            //contact.Relationship = contactData.relationship;
                            //contact.AddressLine1 = contactData.addressLine1;
                            //contact.AddressLine2 = contactData.addressLine2;
                            //contact.IsPrimary = contactData.isPrimary;
                            //contact.City = contactData.city;
                            //contact.State = contactData.state;
                            //contact.Country = contactData.country;
                            //contact.PostalCode = contactData.zip;
                            //contact.TelePhoneNo = Utility.EncryptStringAES(contactData.telephoneNo);
                            //contact.MobileNo = Utility.EncryptStringAES(contactData.mobileNo);
                            //contact.EmailAddress = contactData.emailAddress;
                            //contact.ContactType = (bool)contactData.isPrimary ? StringConstants.PrimaryContact : string.Empty;

                            contact.EmployeeId = details.EmployeeId;
                            contact.TelePhoneNo = Utility.EncryptStringAES(contactData.TelePhoneNo);
                            contact.MobileNo = Utility.EncryptStringAES(contactData.MobileNo);
                            contact.ContactType = (bool)contactData.IsPrimary ? StringConstants.PrimaryContact : string.Empty;
                            m_EmployeeContext.EmergencyContactDetails.Add(contact);
                        }
                    }
                }
                retValue = await m_EmployeeContext.SaveChangesAsync() > 0 ? true : false;

                if (retValue)
                {
                    response.IsSuccessful = true;
                    response.Message = "Successfully updated the employee's family details.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while updating the employee's family details.";
                m_Logger.LogError("Error occured in UpdateFamilyDetails() method" + ex.StackTrace);
            }

            return response;
        }
        #endregion

        #region GetFamilyDetailsById
        /// <summary>
        /// Gets the family details of an employee by the given Id
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<EmployeePersonalDetails>> GetFamilyDetailsById(int empId)
        {
            EmployeePersonalDetails userDetails = null;
            var response = new ServiceResponse<EmployeePersonalDetails>();

            try
            {
                m_Logger.LogInformation("Calling GetFamilyDetailsById method in EmployeeFamilyDetailsService");
                userDetails = new EmployeePersonalDetails();
                var familyDetails = await m_EmployeeContext.FamilyDetails.Where(f => f.EmployeeId == empId && f.IsActive == true).ToListAsync();
                var contactDetails = await m_EmployeeContext.EmergencyContactDetails.Where(e => e.EmployeeId == empId).ToListAsync();
                var relationDetails = (from fd in familyDetails
                                       select new FamilyDetails
                                       {
                                           Id = fd.Id,
                                           Name = Utility.DecryptStringAES(fd.Name),
                                           EmployeeId = fd.EmployeeId,
                                           DateOfBirth = fd.DateOfBirth,
                                           RelationShip = fd.RelationShip,
                                           Occupation = fd.Occupation,
                                           //BirthDate = string.Format("{0:dd/MM/yyyy}", fd.DateOfBirth)
                                       }).ToList();

                var contactDetailsList = (from ecd in contactDetails
                                          select new EmergencyContactDetails
                                          {
                                              Id = ecd.Id,
                                              EmployeeId = ecd.EmployeeId,
                                              ContactName = ecd.ContactName,
                                              ContactType = ecd.ContactType,
                                              Relationship = ecd.Relationship,
                                              AddressLine1 = ecd.AddressLine1,
                                              AddressLine2 = ecd.AddressLine2,
                                              TelePhoneNo = Utility.DecryptStringAES(ecd.TelePhoneNo),
                                              MobileNo = Utility.DecryptStringAES(ecd.MobileNo),
                                              EmailAddress = ecd.EmailAddress,
                                              City = ecd.City,
                                              State = ecd.State,
                                              Country = ecd.Country,
                                              PostalCode = ecd.PostalCode,
                                              IsPrimary = ecd.IsPrimary
                                          }).Take(3).ToList();

                userDetails.RelationsInfo = relationDetails;
                userDetails.contactDetailsOne = contactDetailsList.Count >= 1 ? contactDetailsList[0] : null;
                userDetails.contactDetailsTwo = contactDetailsList.Count >= 2 ? contactDetailsList[1] : null;
                response.IsSuccessful = true;
                response.Item = userDetails;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occured while fetching the employee's family details";
                m_Logger.LogError("Error occured in GetFamilyDetailsById() method" + ex.StackTrace);
            }

            return response;
        }
        #endregion
    }
}
