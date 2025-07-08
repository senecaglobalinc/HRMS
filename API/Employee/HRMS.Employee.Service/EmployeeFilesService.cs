using AutoMapper;
using HRMS.Common;
using HRMS.Common.Enums;
using HRMS.Employee.Database;
using HRMS.Employee.Entities;
using HRMS.Employee.Infrastructure;
using HRMS.Employee.Infrastructure.Constants;
using HRMS.Employee.Infrastructure.Models.Domain;
using HRMS.Employee.Infrastructure.Models.Response;
using HRMS.Employee.Infrastructure.Response;
using HRMS.Employee.Types;
using HRMS.Employee.Types.External;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Employee.Service
{
    public class EmployeeFilesService : IEmployeeFilesService
    {
        #region Global Variables
        private readonly ILogger<EmployeeFilesService> m_Logger;
        private readonly EmployeeDBContext m_EmployeeContext;
        private readonly IMapper m_mapper;
        private readonly IOrganizationService m_OrgService;
        private readonly IConfiguration m_configuration;
        private readonly MiscellaneousSettings m_MiscellaneousSettings;
        #endregion

        #region Constructor

        public EmployeeFilesService(ILogger<EmployeeFilesService> logger,
                    EmployeeDBContext employeeDBContext, IConfiguration configuration,
                    IOrganizationService orgService,
                    IOptions<MiscellaneousSettings> miscellaneousSettings)
        {
            m_Logger = logger;
            m_EmployeeContext = employeeDBContext;
            m_configuration = configuration;
            m_OrgService = orgService;
            //Create mapper for EmployeeSkill
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UploadFiles, UploadFile>();
            });
            m_mapper = config.CreateMapper();
            m_MiscellaneousSettings = miscellaneousSettings?.Value;
        }

        #endregion

        #region Save
        /// <summary>
        /// Save uploded file details
        /// </summary>
        /// <param name="uploadFiles"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<UploadFile>> Save(UploadFiles uploadFilesIn)
        {
            int isCreated = 0;
            var response = new ServiceResponse<UploadFile>();
            m_Logger.LogInformation("EmployeeFilesService: Calling \"Save\" method.");
            try
            {
                UploadFile uploadFile = new UploadFile();

                //map fields
                m_mapper.Map<UploadFiles, UploadFile>(uploadFilesIn, uploadFile);
                uploadFile.IsActive = true;

                //Add file to list
                m_EmployeeContext.UploadFiles.Add(uploadFile);

                string employeeCode = m_EmployeeContext.Employees
                                        .Where(emp => emp.EmployeeId == uploadFilesIn.EmployeeId)
                                        .Select(emp => emp.EmployeeCode)
                                        .FirstOrDefault();

                //create folder structure with the employee code
                CreateRepository(employeeCode);

                string rootPath = m_MiscellaneousSettings.RepositoryPath;
                foreach (IFormFile file in uploadFilesIn.UploadedFiles)
                {
                    string path = string.Empty;
                    path = @rootPath + employeeCode + @"\" + StringConstants.ONBOARDING;
                    //Checks folder with that employee code exits or not.
                    bool isExists = Directory.Exists(path);
                    if (!isExists)
                    {
                        response.IsSuccessful = false;
                        response.Message = "File cannot be uploaded";
                        return response;
                    }

                    //Give fileName and serverpath to save the file/files.
                    var filePath = path + @"\" + uploadFilesIn.FileName;

                    //create file in the location
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in EmployeeProfessionalService");
                isCreated = await m_EmployeeContext.SaveChangesAsync();
                if (isCreated > 0)
                {
                    response.IsSuccessful = true;
                    response.Item = uploadFile;
                    m_Logger.LogInformation("File uploaded successfully");
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "No file uploaded";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while uploading files";
                m_Logger.LogError("Error occurred in \"Save\" of EmployeeFilesService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GetByEmployeeId
        /// <summary>
        /// Get Uploaded file details based on employee Id
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceListResponse<UploadFile>> GetByEmployeeId(int employeeId)
        {
            var response = new ServiceListResponse<UploadFile>();
            try
            {
                m_Logger.LogInformation("EmployeeFilesService: Calling \"GetByEmployeeId\" method.");
                List<UploadFile> files = await m_EmployeeContext.UploadFiles
                                                        .Where(file => file.EmployeeId == employeeId
                                                            && file.IsActive == true)
                                                        .ToListAsync();

                response.IsSuccessful = true;
                response.Items = files;
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while fetching uploaded files";
                m_Logger.LogError("Error occurred while fetching uploaded files in GetByEmployeeId() method of EmployeeFilesService" + ex.StackTrace);
            }
            return response;
        }

        #endregion

        #region Delete
        /// <summary>
        /// Delete uploded file details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<ServiceResponse<bool>> Delete(int Id, int employeeId)
        {
            int isDeleted = 0;
            var response = new ServiceResponse<bool>();
            m_Logger.LogInformation("EmployeeFilesService: Calling \"Delete\" method.");
            try
            {
                //check if file exists based on Id
                UploadFile file = m_EmployeeContext.UploadFiles.Find(Id);
                if (file == null)
                {
                    response.IsSuccessful = false;
                    response.Message = "File not found";
                    return response;
                }
                file.IsActive = false;

                string employeeCode = m_EmployeeContext.Employees
                                        .Where(emp => emp.EmployeeId == employeeId)
                                        .Select(emp => emp.EmployeeCode)
                                        .FirstOrDefault();
                string rootPath = m_MiscellaneousSettings.RepositoryPath;
                string path = string.Empty;
                path = @rootPath + employeeCode + @"\OnBoarding";
                var filePath = path + @"\" + file.FileName;

                //Check if file exists in the location
                if (File.Exists(filePath))
                {
                    //delete the file
                    File.Delete(filePath);
                    m_Logger.LogInformation("Calling SaveChangesAsync method on DBContext in EmployeeFilesService");
                    isDeleted = await m_EmployeeContext.SaveChangesAsync();
                    if (isDeleted > 0)
                    {
                        response.IsSuccessful = true;
                        response.Item = true;
                        m_Logger.LogInformation("File deleted successfully");
                    }
                    else
                    {
                        response.IsSuccessful = false;
                        response.Message = "No file deleted";
                        return response;
                    }
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Message = "File not found";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = "Error occurred while deleting files";
                m_Logger.LogError("Error occurred in \"Delete\" of EmployeeFilesService " + ex.StackTrace);
            }
            return response;
        }
        #endregion

        #region GeneratePDFReport
        /// <summary>
        /// Generate PDF
        /// </summary>
        /// <param name="empID"></param>
        public async Task<byte[]> GeneratePDFReport(int empID)
        {
            Paragraph para = null;
            Phrase pharse = null;
            PdfPTable table = null;
            PdfPCell cell = null;
            Document document = new Document(PageSize.A4, 30, 30, 30, 130); // PageSize.A4, left, right, top , bottom
            Font contentFont = FontFactory.GetFont(FontFactory.HELVETICA, 11f, BaseColor.BLACK);
            Font contentFontBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11f, BaseColor.BLACK);

            string pdfPassword = m_MiscellaneousSettings.PdfPassword;
            try
            {
                var employee = await m_EmployeeContext.Employees.Where(i => i.EmployeeId == empID && i.IsActive == true).ToListAsync();
                var departments = await m_OrgService.GetAllDepartments();
                var employeeTypes = await m_EmployeeContext.EmployeeTypes.Where(et => et.IsActive == true).OrderBy(x => x.EmpType).ToListAsync(); ;
                var grades = await m_OrgService.GetAllGrades();
                var designations = await m_OrgService.GetAllDesignations();
                var technologies = await m_OrgService.GetAllPracticeAreas();

                if (employee != null && employee.Count > 0)
                {
                    int reportingManagerId = employee[0].ReportingManager ?? 0;
                    string reportingManagerName = "";
                    if (reportingManagerId > 0)
                        reportingManagerName = m_EmployeeContext.Employees.Where(i => i.EmployeeId == reportingManagerId && i.IsActive == true).Select(c => c.FirstName + " " + c.LastName).FirstOrDefault();

                    var getEmployeeDetails = (from e in employee
                                              join d in departments.Items on e.DepartmentId equals d.DepartmentId into dep
                                              from dept in dep.DefaultIfEmpty()
                                              join et in employeeTypes on e.EmployeeTypeId equals et.EmployeeTypeId into etypes
                                              from etype in etypes.DefaultIfEmpty()
                                              join des in designations.Items on e.DesignationId equals des.DesignationId into designationsList
                                              from designation in designationsList.DefaultIfEmpty()
                                              join tes in technologies.Items on e.CompetencyGroup equals tes.PracticeAreaId into technologyList
                                              from technology in technologyList.DefaultIfEmpty()

                                              join g in grades.Items on e.GradeId equals g.GradeId

                                              select new PersonalDetails
                                              {
                                                  empCode = e.EmployeeCode,
                                                  workEmailID = e.WorkEmailAddress,
                                                  firstName = e.FirstName,
                                                  middleName = e.MiddleName,
                                                  lastName = e.LastName,
                                                  EncryptedPhoneNumber = e.TelephoneNo,
                                                  EncryptedMobileNo = e.MobileNo,
                                                  personalEmail = e.PersonalEmailAddress,
                                                  Birthdate = e.DateofBirth != null ? e.DateofBirth.Value.Date.ToShortDateString() : "",
                                                  gender = e.Gender,
                                                  maritalStatus = e.MaritalStatus,
                                                  bloodGroup = e.BloodGroup,
                                                  nationality = e.Nationality,
                                                  EncryptedPanNumber = e.Pannumber,
                                                  EncryptedPFNumber = e.Pfnumber,
                                                  EncryptedAadharNumber = e.AadharNumber,
                                                  EncryptedUANNumber = e.Uannumber,
                                                  EncryptedPassportNumber = e.PassportNumber,
                                                  passportValidDate = e.PassportDateValidUpto,
                                                  passportIssuingOffice = e.PassportIssuingOffice,
                                                  bgvStatus = e.Bgvstatus,
                                                  bgvStartDate = e.BgvinitiatedDate,
                                                  bgvCompletedDate = e.BgvcompletionDate,
                                                  //Experience = e.Experience.ToString(),
                                                  EmploymentStartDate = e.EmploymentStartDate,
                                                  joiningDate = e.JoinDate,
                                                  CareerBreak = e.CareerBreak ?? 0,
                                                  hrAdvisor = e.Hradvisor,
                                                  designation = designation.DesignationName,
                                                  department = dept.DepartmentCode,
                                                  Technology = technology.PracticeAreaDescription,
                                                  GradeName = g.GradeName,
                                                  employmentType = etype.EmpType,
                                                  ReportingManager = reportingManagerName
                                              }).FirstOrDefault();



                    var getContactDetails = (from contact in m_EmployeeContext.Contacts
                                             where contact.EmployeeId == empID && (contact.AddressType == "CurrentAddress" || contact.AddressType == "PermanentAddress")
                                             select new ContactDetails
                                             {
                                                 currentAddress1 = contact.AddressLine1,
                                                 currentAddress2 = contact.AddressLine2,
                                                 currentAddCity = contact.City,
                                                 currentAddState = contact.State,
                                                 currentAddZip = contact.PostalCode,
                                                 currentAddCountry = contact.Country,
                                                 addressType = contact.AddressType
                                             }).ToList();

                    var emergencyPrimaryContactDetails = (from ePrimeContact in m_EmployeeContext.EmergencyContactDetails
                                                          where ePrimeContact.EmployeeId == empID && (ePrimeContact.ContactType == "PrimaryContact" || ePrimeContact.ContactType == "")
                                                          select new EmergencyContactData
                                                          {
                                                              ID = ePrimeContact.Id,
                                                              contactName = ePrimeContact.ContactName,
                                                              relationship = ePrimeContact.Relationship,
                                                              addressLine1 = ePrimeContact.AddressLine1,
                                                              addressLine2 = ePrimeContact.AddressLine2,
                                                              city = ePrimeContact.City,
                                                              state = ePrimeContact.State,
                                                              zip = ePrimeContact.PostalCode,
                                                              country = ePrimeContact.Country,
                                                              mobileNo = Utility.DecryptStringAES(ePrimeContact.MobileNo),
                                                              EncryptedMobileNo = ePrimeContact.MobileNo,
                                                              EncryptedTelePhoneNo = ePrimeContact.TelePhoneNo,
                                                              emailAddress = ePrimeContact.EmailAddress,
                                                              contactType = ePrimeContact.ContactType
                                                          }).OrderByDescending(e => e.contactType).ToList();

                    var familyDetails = (from family in m_EmployeeContext.FamilyDetails
                                         where family.EmployeeId == empID
                                         select new RelationDetails
                                         {
                                             EncryptedName = family.Name,
                                             relationship = family.RelationShip,
                                             birthDate = family.DateOfBirth.HasValue ? family.DateOfBirth.Value.ToShortDateString() : "",
                                             occupation = family.Occupation
                                         }).ToList();

                    var educationDetails = (from ed in m_EmployeeContext.EducationDetails
                                            where ed.EmployeeId == empID & ed.IsActive == true
                                            select new EmpEducationDetails
                                            {
                                                qualificationName = ed.EducationalQualification,
                                                completedYear = ed.AcademicCompletedYear.HasValue ? ed.AcademicCompletedYear.Value.ToShortDateString() : "",
                                                institution = ed.Institution,
                                                specialization = ed.Specialization,
                                                programType = ed.ProgramType,
                                                grade = ed.Grade,
                                                marks = ed.Marks
                                            }).ToList();

                    var getPrevEmploymentDetails = (from ed in m_EmployeeContext.PreviousEmploymentDetails
                                                    where ed.EmployeeId == empID
                                                    select new EmploymentDetails
                                                    {
                                                        name = ed.Name,
                                                        address = ed.Address,
                                                        designation = ed.Designation,
                                                        fromYear = ed.ServiceFrom.HasValue ? ed.ServiceFrom.Value.ToShortDateString() : "",
                                                        toYear = ed.ServiceTo.HasValue ? ed.ServiceTo.Value.ToShortDateString() : "",
                                                        leavingReason = ed.LeavingReason
                                                    }).ToList();

                    var getProfReferenceDetils = (from pr in m_EmployeeContext.ProfessionalReferences
                                                  where pr.EmployeeId == empID
                                                  select new ProfRefDetails
                                                  {
                                                      ID = pr.Id,
                                                      name = pr.Name,
                                                      designation = pr.Designation,
                                                      companyName = pr.CompanyName,
                                                      companyAddress = pr.CompanyAddress,
                                                      officeEmailAddress = pr.OfficeEmailAddress,
                                                      mobileNo = pr.MobileNo
                                                  }).ToList();
                    var skillsList = await m_OrgService.GetAllSkills(true);
                    var domainList = await m_OrgService.GetAllDomains();
                    var proficiencyLevels = await m_OrgService.GetAllProficiencyLevels(true);
                    var competencyAreas = await m_OrgService.GetCompetencyAreas(true);
                    var empSkills = await m_EmployeeContext.EmployeeSkills.Where(x => x.EmployeeId == empID).ToListAsync();
                    var skills = (from es in empSkills
                                  join skill in skillsList.Items on es.SkillId equals skill.SkillId
                                  join pl in proficiencyLevels.Items on es.ProficiencyLevelId equals pl.ProficiencyLevelId
                                  join ca in competencyAreas.Items on es.CompetencyAreaId equals ca.CompetencyAreaId
                                  where es.EmployeeId == empID
                                  select new EmployeeSkillDetails
                                  {
                                      Id = es.Id,
                                      EmployeeId = es.EmployeeId,
                                      SkillId = es.SkillId,
                                      SkillName = skill.SkillName,
                                      Experience = es.Experience,
                                      ProficiencyLevelId = es.ProficiencyLevelId,
                                      ProficiencyLevelCode = pl.ProficiencyLevelCode,
                                      CompetencyAreaCode = ca.CompetencyAreaCode,
                                      IsPrimary = es.IsPrimary,
                                      LastUsed = es.LastUsed
                                  }).ToList();

                    var projects = (from ep in m_EmployeeContext.EmployeeProjects
                                    where ep.EmployeeId == empID
                                    select new EmployeeProjectDetails
                                    {
                                        organizationName = ep.OrganizationName,
                                        projectName = ep.ProjectName,
                                        duration = ep.Duration,
                                        keyAchievement = ep.KeyAchievements,
                                        DomainID = ep.DomainId ?? 0
                                    }).ToList();

                    List<ProfessionalDetails> certificationList;
                    List<ProfessionalDetails> membershipsList;


                    //Fetch certifications of the given employee
                    var certifications = await m_EmployeeContext.AssociateCertifications.
                                            Where(membership => membership.EmployeeId == empID).ToListAsync();

                    if (certifications.Count > 0)
                    {
                        //Fetch distinct SkillGroupId
                        List<int> certificationSkillGroupIds = certifications.Select(c => c.SkillGroupId).Distinct().ToList();

                        //Fetch the skills based on the above obtained distinct SkillGroupId 
                        var skillItems = await m_OrgService.GetSkillsBySkillGroupId(certificationSkillGroupIds);


                        certificationList = (from certificate in certifications
                                             join skill in skillItems.Items on certificate.CertificationId equals skill.SkillId
                                             select new ProfessionalDetails
                                             {
                                                 Id = certificate.Id,
                                                 EmployeeId = certificate.EmployeeId,
                                                 ValidFrom = certificate.ValidFrom,
                                                 Institution = certificate.Institution,
                                                 Specialization = certificate.Specialization,
                                                 ValidUpto = certificate.ValidUpto,
                                                 SkillGroupName = skill.SkillGroup.SkillGroupName,
                                                 SkillGroupId = certificate.SkillGroupId,
                                                 SkillName = skill.SkillName,
                                                 CertificationId = certificate.CertificationId,
                                                 ProgramType = (int)ProgramType.Certification,
                                                 ProgramTitle = ""
                                             }).ToList();
                    }
                    else
                    {
                        certificationList = new List<ProfessionalDetails>();
                    }

                    membershipsList = (from membership in m_EmployeeContext.AssociateMemberships
                                       where membership.EmployeeId == empID
                                       select new ProfessionalDetails
                                       {
                                           Id = membership.Id,
                                           EmployeeId = membership.EmployeeId,
                                           ProgramTitle = membership.ProgramTitle,
                                           ValidFrom = membership.ValidFrom,
                                           Institution = membership.Institution,
                                           Specialization = membership.Specialization,
                                           ValidUpto = membership.ValidUpto,
                                           SkillGroupName = "",
                                           SkillGroupId = 0,
                                           SkillName = "",
                                           CertificationId = 0,
                                           ProgramType = (int)ProgramType.MemberShip
                                       }).ToList();

                    if (membershipsList.Count == 0)
                    {
                        membershipsList = new List<ProfessionalDetails>();
                    }

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfWriter.GetInstance(document, memoryStream);
                        document.Open();

                        table = PDFHRSection(document, para, contentFontBold, contentFont, table, cell, pharse, getEmployeeDetails);
                        document.Add(table);

                        table = PDFPersonalSection(document, para, contentFontBold, contentFont, table, cell, pharse, getEmployeeDetails, getContactDetails);
                        document.Add(table);

                        table = PDFEmergencyContactSection(document, para, contentFontBold, contentFont, table, cell, pharse, emergencyPrimaryContactDetails);
                        document.Add(table);

                        table = PDFFamilySection(document, para, contentFontBold, contentFont, table, cell, pharse, familyDetails);
                        document.Add(table);

                        table = PDFEducationalSection(document, para, contentFontBold, contentFont, table, cell, pharse, educationDetails);
                        document.Add(table);

                        table = PDFEmploymentSection(document, para, contentFontBold, contentFont, table, cell, pharse, getPrevEmploymentDetails);
                        document.Add(table);

                        table = PDFCertificationSection(document, para, contentFontBold, contentFont, table, cell, pharse, certificationList);
                        document.Add(table);

                        table = PDFMemberShipSection(document, para, contentFontBold, contentFont, table, cell, pharse, membershipsList);
                        document.Add(table);

                        table = PDFSkillSection(document, para, contentFontBold, contentFont, table, cell, pharse, skills);
                        document.Add(table);

                        table = PDFProjectSection(document, para, contentFontBold, contentFont, table, cell, pharse, projects);
                        document.Add(table);

                        table = PDFProfessionalReferencesSection(document, para, contentFontBold, contentFont, table, cell, pharse, getProfReferenceDetils);
                        document.Add(table);

                        document = PDFDeclarationSection(document, para, contentFontBold, contentFont, table, cell, pharse);

                        document.Close();
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Close();

                        using (MemoryStream input = new MemoryStream(bytes))
                        {
                            using (MemoryStream output = new MemoryStream())
                            {
                                PdfReader reader = new PdfReader(input);
                                PdfEncryptor.Encrypt(reader, output, true, pdfPassword, pdfPassword, PdfWriter.ALLOW_SCREENREADERS);
                                bytes = output.ToArray();
                                return bytes;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region PDFHRSection
        /// <summary>
        /// PDF HR Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="personalDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFHRSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, PersonalDetails personalDetails)
        {
            para = ParagraphContent("1.TO BE FILLED BY HR", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(2);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            pharse.Add(new Chunk("Associate ID No:", contentFontBold));
            cell = PhraseCell(pharse);
            table.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk(personalDetails.empCode, contentFont));
            cell = PhraseCell(pharse);
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Associate Email Address", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase(personalDetails.workEmailID, contentFont));
            table.AddCell(cell);
            table.AddCell(cell);

            return table;
        }
        #endregion

        #region PDFPersonalSection
        /// <summary>
        /// PDFPersonalSection
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="personalDetails"></param>
        /// <param name="contactDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFPersonalSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, PersonalDetails personalDetails, List<ContactDetails> contactDetails)
        {
            para = ParagraphContent("2.	PERSONAL DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            cell = PhraseCell(new Phrase("First Name:", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.firstName, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Middle Name:", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.middleName, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Last Name:", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.lastName, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Gender", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.gender, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            string address = string.Empty;
            foreach (var addr in contactDetails)
            {
                if (addr.addressType.ToLower().Contains("current"))
                {
                    if (addr.currentAddress1 != null && addr.currentAddress1 != "")
                        address = addr.currentAddress1;
                    if (addr.currentAddress2 != null && addr.currentAddress2 != "")
                        address = address + "," + addr.currentAddress2;
                    if (addr.currentAddCity != null && addr.currentAddCity != "")
                        address = address + "," + addr.currentAddCity;
                    if (addr.currentAddState != null && addr.currentAddState != "")
                        address = address + "," + addr.currentAddState;
                    if (addr.currentAddZip != null && addr.currentAddZip != "")
                        address = address + "," + addr.currentAddZip;
                    if (addr.currentAddCountry != null && addr.currentAddCountry != "")
                        address = address + "," + addr.currentAddCountry;
                    address = address.TrimStart(',');
                    cell = PhraseCell(new Phrase("Current  Address:\n ", contentFontBold));
                    table.AddCell(cell);
                    cell = PhraseCell(new Phrase(address, contentFont));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
            }
            cell = PhraseCell(new Phrase("Telephone No \n(with STD code)", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.phoneNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            foreach (var addr in contactDetails)
            {
                if (addr.addressType.ToLower().Contains("permanent"))
                {
                    address = string.Empty;
                    cell = PhraseCell(new Phrase("Permanent Address:\n ", contentFontBold));
                    table.AddCell(cell);
                    if (addr.currentAddress1 != null && addr.currentAddress1 != "")
                        address = addr.currentAddress1;
                    if (addr.currentAddress2 != null && addr.currentAddress2 != "")
                        address = address + "," + addr.currentAddress2;
                    if (addr.currentAddCity != null && addr.currentAddCity != "")
                        address = address + "," + addr.currentAddCity;
                    if (addr.currentAddState != null && addr.currentAddState != "")
                        address = address + "," + addr.currentAddState;
                    if (addr.currentAddZip != null && addr.currentAddZip != "")
                        address = address + "," + addr.currentAddZip;
                    if (addr.currentAddCountry != null && addr.currentAddCountry != "")
                        address = address + "," + addr.currentAddCountry;
                    address = address.TrimStart(',');
                    cell = PhraseCell(new Phrase(address, contentFont));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
            }
            cell = PhraseCell(new Phrase("Mobile No  ", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.mobileNo, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Personal Email Address", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.personalEmail, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Date of Birth", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.Birthdate, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Marital Status", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.maritalStatus, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Blood Group", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.bloodGroup, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Nationality", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.nationality, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("PAN Number", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.panNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("PF Number", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.pfNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Aadhar Number", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.aadharNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Passport Number", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.passportNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("UAN Number", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.uanNumber, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Employment Start Date", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase((personalDetails.EmploymentStartDate.HasValue ? personalDetails.EmploymentStartDate.Value.Date.ToShortDateString() : ""), contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Joining Date", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase((personalDetails.joiningDate.HasValue ? personalDetails.joiningDate.Value.Date.ToShortDateString() : ""), contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Designation", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.designation, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Grade", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.GradeName, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("HR Advisor", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.hrAdvisor, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Department", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.department, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Reporting Manager", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.ReportingManager, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Employee Type", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.employmentType, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Technology", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.Technology, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Career Break", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase((personalDetails.CareerBreak ?? 0).ToString(), contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Passport Issuing Office", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.passportIssuingOffice, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("Passport Valid Upto", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.passportValidDate, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("BGV Start Date", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase((personalDetails.bgvStartDate.HasValue ? personalDetails.bgvStartDate.Value.Date.ToShortDateString() : ""), contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("BGV Status", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase(personalDetails.bgvStatus, contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("BGV Completed Date", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase((personalDetails.bgvCompletedDate.HasValue ? personalDetails.bgvCompletedDate.Value.Date.ToShortDateString() : ""), contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("", contentFontBold));
            table.AddCell(cell);
            cell = PhraseCell(new Phrase("", contentFont));
            cell.Colspan = 2;
            table.AddCell(cell);

            return table;
        }
        #endregion

        #region PDFEmergencyContactSection
        /// <summary>
        /// PDF Emergency Contact Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="contactDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFEmergencyContactSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<EmergencyContactData> contactDetails)
        {
            para = ParagraphContent("3.	EMERGENCY CONTACT DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(2);
            table.WidthPercentage = 100;
            foreach (var contact in contactDetails)
            {
                if (contact.contactType == "PrimaryContact")
                {
                    pharse = new Phrase();
                    cell = PhraseCell(new Phrase("Primary Contact", contentFontBold));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
                else
                {
                    pharse = new Phrase();
                    cell = PhraseCell(new Phrase("", contentFontBold));
                    cell.Colspan = 2;
                    table.AddCell(cell);

                    pharse = new Phrase();
                    cell = PhraseCell(new Phrase("Other Contact", contentFontBold));
                    cell.Colspan = 2;
                    table.AddCell(cell);
                }
                pharse = new Phrase();
                cell = PhraseCell(new Phrase("Name :", contentFontBold));
                table.AddCell(cell);
                pharse.Add(new Chunk(contact.contactName, contentFont));
                cell = PhraseCell(pharse);
                table.AddCell(cell);

                pharse = new Phrase();
                cell = PhraseCell(new Phrase("Relationship :", contentFontBold));
                table.AddCell(cell);
                pharse.Add(new Chunk(contact.relationship, contentFont));
                cell = PhraseCell(pharse);
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Current Address :", contentFontBold));
                table.AddCell(cell);
                string emergencyPrimaryContactAddress = string.Empty;
                //contact.addressLine1 + ',' + contact.addressLine2 + ',' + contact.city + ',' + contact.state + ',' + contact.country;
                if (contact.addressLine1 != null && contact.addressLine1 != "")
                    emergencyPrimaryContactAddress = contact.addressLine1;
                if (contact.addressLine2 != null && contact.addressLine2 != "")
                    emergencyPrimaryContactAddress = emergencyPrimaryContactAddress + "," + contact.addressLine2;
                if (contact.city != null && contact.city != "")
                    emergencyPrimaryContactAddress = emergencyPrimaryContactAddress + "," + contact.city;
                if (contact.state != null && contact.state != "")
                    emergencyPrimaryContactAddress = emergencyPrimaryContactAddress + "," + contact.state;
                if (contact.zip != null && contact.zip != "")
                    emergencyPrimaryContactAddress = emergencyPrimaryContactAddress + "," + contact.zip;
                if (contact.country != null && contact.country != "")
                    emergencyPrimaryContactAddress = emergencyPrimaryContactAddress + "," + contact.country;

                cell = PhraseCell(new Phrase(emergencyPrimaryContactAddress, contentFont));
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Mobile No. :", contentFontBold));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(contact.mobileNo, contentFont));
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Telephone No. :", contentFontBold));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(contact.telephoneNo, contentFont));
                cell.Colspan = 2;
                table.AddCell(cell);

                cell = PhraseCell(new Phrase("Email Address:", contentFontBold));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(contact.emailAddress, contentFont));
                cell.Colspan = 2;
                table.AddCell(cell);
            }
            return table;
        }
        #endregion

        #region PDFFamilySection
        /// <summary>
        /// PDFFamilySection
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="familyDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFFamilySection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<RelationDetails> familyDetails)
        {
            para = ParagraphContent("4.	FAMILY DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(4);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            pharse.Add(new Chunk("Relationship", contentFontBold));
            cell = PhraseCell(pharse);
            table.AddCell(cell);

            pharse = new Phrase();
            pharse.Add(new Chunk("Name ", contentFontBold));
            cell = PhraseCell(pharse);
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Date of Birth", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Occupation", contentFontBold));
            table.AddCell(cell);

            foreach (var familyDetail in familyDetails)
            {
                cell = PhraseCell(new Phrase(familyDetail.relationship, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(familyDetail.name, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(familyDetail.birthDate, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(familyDetail.occupation, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFEducationalSection
        /// <summary>
        /// PDF Educational Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="educationDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFEducationalSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<EmpEducationDetails> educationDetails)
        {
            para = ParagraphContent("5.	EDUCATION DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(7);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 6f, 5f, 6f, 5f, 5f, 4f, 5f });

            pharse = new Phrase();
            pharse.Add(new Chunk("Qualification", contentFontBold));

            cell = PhraseCell(pharse);
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Program Type", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Specialization ", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Completed Year", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Institute/ University", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Grade", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Marks/Percentage", contentFontBold));
            table.AddCell(cell);

            foreach (var ed in educationDetails)
            {
                cell = PhraseCell(new Phrase(ed.qualificationName, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.programType, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.specialization, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.completedYear, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.institution, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.grade, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(ed.marks, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFCertificationSection
        /// <summary>
        /// PDF Certification Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="certificateDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFCertificationSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<ProfessionalDetails> certificateDetails)
        {
            para = ParagraphContent("6.	PROFESSIONAL CERTIFICATIONS DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            cell = PhraseCell(new Phrase("Certification Title", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Certification Year", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Certification Institution ", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Specialization", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Current Validity", contentFontBold));
            table.AddCell(cell);

            foreach (var certificate in certificateDetails)
            {
                cell = PhraseCell(new Phrase(certificate.SkillGroupName + " - " + certificate.SkillName, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(certificate.ValidFrom, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(certificate.Institution, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(certificate.Specialization, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(certificate.ValidFrom + " - " + certificate.ValidUpto, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFMemberShipSection
        /// <summary>
        /// PDF MemberShip Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="membershipDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFMemberShipSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<ProfessionalDetails> membershipDetails)
        {
            para = ParagraphContent("7.	PROFESSIONAL MEMBERSHIP DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            cell = PhraseCell(new Phrase("Membership Title", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Membership Year", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Institution", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Specialization", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Current Validity", contentFontBold));
            table.AddCell(cell);

            foreach (var membership in membershipDetails)
            {
                cell = PhraseCell(new Phrase(membership.ProgramTitle, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(membership.ValidFrom, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(membership.Institution, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(membership.Specialization, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(membership.ValidFrom + " - " + membership.ValidUpto, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFEmploymentSection
        /// <summary>
        /// PDF Employment Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="employmentDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFEmploymentSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<EmploymentDetails> employmentDetails)
        {
            para = ParagraphContent("6.	EMPLOYMENT DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            pharse.Add(new Chunk("Name & Address of Previous Employer", contentFontBold));

            cell = PhraseCell(pharse);
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Designation", contentFontBold));
            table.AddCell(cell);

            PdfPTable nested = new PdfPTable(2);
            nested.WidthPercentage = 100;
            cell = PhraseCell(new Phrase("Service Duration", contentFontBold));
            cell.Colspan = 2;
            nested.AddCell(cell);
            cell = PhraseCell(new Phrase("From", contentFontBold));
            nested.AddCell(cell);
            cell = PhraseCell(new Phrase("To", contentFontBold));
            nested.AddCell(cell);
            cell = new PdfPCell(nested);
            cell.Colspan = 2;
            table.AddCell(cell);

            //cell = PhraseCell(new Phrase("Last Drawn Salary", contentFontBold));
            //table.AddCell(cell);

            cell = PhraseCell(new Phrase("Reason for Leaving", contentFontBold));
            table.AddCell(cell);

            foreach (var prevEmpDetails in employmentDetails)
            {
                cell = PhraseCell(new Phrase(prevEmpDetails.name + "\n" + prevEmpDetails.address, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(prevEmpDetails.designation, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(prevEmpDetails.fromYear, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(prevEmpDetails.toYear, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(prevEmpDetails.leavingReason, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFSkillSection
        /// <summary>
        /// PDF Skill Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="employeeSkillDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFSkillSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<EmployeeSkillDetails> employeeSkillDetails)
        {
            para = ParagraphContent("7.	SKILL DETAILS", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            cell = PhraseCell(new Phrase("Skill / Expertize Title", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Domain", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Experience (In months)", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Proficiency Level", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Last Used", contentFontBold));
            table.AddCell(cell);

            foreach (var skill in employeeSkillDetails)
            {
                cell = PhraseCell(new Phrase((string.IsNullOrWhiteSpace(skill.SkillName) ? "" : skill.SkillName.ToString()), contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(skill.CompetencyAreaCode.ToString(), contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(skill.Experience.ToString(), contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(skill.ProficiencyLevelCode.ToString(), contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(skill.LastUsed.ToString(), contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFProjectSection
        /// <summary>
        /// PDF Project Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="projectDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFProjectSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<EmployeeProjectDetails> projectDetails)
        {
            para = ParagraphContent("8.	PROJECT DETAILS (Worked in last 4 years)", contentFontBold, spacingAfter: 20);
            document.Add(para);

            table = new PdfPTable(4);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            cell = PhraseCell(new Phrase("Organization Name", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Project Name", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Duration (Months)", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Key Achievements", contentFontBold));
            table.AddCell(cell);

            foreach (var projcts in projectDetails)
            {
                cell = PhraseCell(new Phrase(projcts.organizationName, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(projcts.projectName, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(projcts.duration.ToString(), contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(projcts.keyAchievement, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFProfessionalReferencesSection
        /// <summary>
        /// PDF Professional References Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <param name="profRefDetails"></param>
        /// <returns></returns>
        private PdfPTable PDFProfessionalReferencesSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse, List<ProfRefDetails> profRefDetails)
        {
            para = ParagraphContent("9.	PROFESSIONAL REFERENCES", contentFontBold);
            document.Add(para);

            para = ParagraphContent(@"Please provide details of two professional references from the previous organization where you were employed:", contentFont);
            document.Add(para);

            para = ParagraphContent(@"1. Your Reporting Manager or Department Head      2. HR Manager", contentFont, spacingBefore: 0, spacingAfter: 10);
            document.Add(para);

            table = new PdfPTable(5);
            table.WidthPercentage = 100;

            pharse = new Phrase();
            cell = PhraseCell(new Phrase("Name", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Designation Name", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Company Name & Address", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Official Email Address", contentFontBold));
            table.AddCell(cell);

            cell = PhraseCell(new Phrase("Contact Number", contentFontBold));
            table.AddCell(cell);

            foreach (var profRef in profRefDetails)
            {
                cell = PhraseCell(new Phrase(profRef.name, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(profRef.designation, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(profRef.companyName + "\n" + profRef.companyAddress, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(profRef.officeEmailAddress, contentFont));
                table.AddCell(cell);
                cell = PhraseCell(new Phrase(profRef.mobileNo, contentFont));
                table.AddCell(cell);
            }

            return table;
        }
        #endregion

        #region PDFDeclarationSection
        /// <summary>
        /// PDF Declaration Section
        /// </summary>
        /// <param name="document"></param>
        /// <param name="para"></param>
        /// <param name="contentFontBold"></param>
        /// <param name="contentFont"></param>
        /// <param name="table"></param>
        /// <param name="cell"></param>
        /// <param name="pharse"></param>
        /// <returns></returns>
        private Document PDFDeclarationSection(Document document, Paragraph para, Font contentFontBold, Font contentFont, PdfPTable table, PdfPCell cell, Phrase pharse)
        {
            para = ParagraphContent(@"10.	DECLARATION AND AUTHORIZATION:", contentFontBold);
            document.Add(para);

            para = ParagraphContent(@"I hereby authorize Seneca Global (or a third party agent appointed by the organization) to carry out Pre-employment Screening and permission to access any appropriate databases. I authorize former employers, agencies, education institutes etc. to release any information pertaining to my employment / education and I release them from any liability in doing so.", contentFont);
            document.Add(para);

            para = ParagraphContent(@"I confirm that the information furnished above is correct and I understand that any misrepresentation of information in this Associates On boarding form may result in my employment being terminated.", contentFont, spacingAfter: 20);
            document.Add(para);

            para = ParagraphContent("Signature: ________________________", contentFontBold);
            document.Add(para);

            para = ParagraphContent("Name (in Capital Letters): ____________________________________________", contentFontBold);
            document.Add(para);

            para = ParagraphContent("Date: ____________________________", contentFontBold);
            document.Add(para);

            return document;
        }
        #endregion

        #region PhraseCell
        /// <summary>
        /// PhraseCell
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="align"></param>
        /// <returns></returns>
        private static PdfPCell PhraseCell(Phrase phrase, int align = Element.ALIGN_LEFT)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.VerticalAlignment = Element.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingLeft = 7f;
            cell.PaddingRight = 7f;
            cell.PaddingBottom = 7f;
            cell.PaddingTop = 7f;
            return cell;
        }
        #endregion

        #region ParagraphContent
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentFont"></param>
        /// <param name="align"></param>
        /// <param name="spacingBefore"></param>
        /// <param name="spacingAfter"></param>
        /// <returns></returns>
        private static Paragraph ParagraphContent(string content, Font contentFont, int align = Element.ALIGN_JUSTIFIED, int spacingBefore = 10, int spacingAfter = 0)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SpacingBefore = spacingBefore;
            paragraph.SpacingAfter = spacingAfter;
            paragraph.Font = contentFont;
            paragraph.Alignment = align;
            paragraph.Add(content);
            return paragraph;
        }
        #endregion

        //private methods
        #region CreateRepository
        void CreateRepository(string employeeCode)
        {
            try
            {
                string filepath = m_MiscellaneousSettings.RepositoryPath;

                //Checks root folder available or not
                bool isExists = Directory.Exists(filepath);
                if (!isExists)
                    Directory.CreateDirectory(filepath);

                // Checks Onboarding folder with that employee code exits or not.
                isExists = Directory.Exists(filepath + "/" + employeeCode + StringConstants.ONBOARDING);
                if (!isExists)
                    Directory.CreateDirectory(filepath + "/" + employeeCode + StringConstants.ONBOARDING);

                // Checks KRA folder with that employee code exits or not.
                isExists = Directory.Exists(filepath + "/" + employeeCode + StringConstants.KRA);
                if (!isExists)
                    Directory.CreateDirectory(filepath + "/" + employeeCode + StringConstants.KRA);
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occurred while creating directories for employee : " + employeeCode + ex.StackTrace);
            }
            return;
        }
        #endregion
    }
}
