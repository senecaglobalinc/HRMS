export const API = {
  'Menu': {
    'GetMenuDetails': '/Menu/GetMenuDetails?roleName='
  },

  'logError': '/ErrorLog/LogError',
  
  'PagingConfigValue': {
    'PageSize': 10,
    'PageDropDown': [
      10,
      15,
      20
    ]
  },
  'Roles': {
    'list': '/Role/GetRoles',
    // 'createRole': '/Role/CreateRole',
    // 'updateRole': '/Role/UpdateRole',
    'createRole': '/FunctionalRole/Create',
    'updateRole': '/FunctionalRole/Update',
    'createRoleSkills': '/Role/AddSkillsToRole',
    'updateRoleskills': '/Role/UpdateRoleSkills',
    'getRolebyID': '/Role/GetRoleByRoleID?roleID=',
    // 'getRoleSuffixAndPrefix': '/Role/getRoleSuffixAndPrefix?departmentId=',
    'getRoleSuffixAndPrefix': '/FunctionalRole/GetSGRoleSuffixAndPrefix?departmentId=',
    // 'GetRoleDetailsByDepartmentID': '/Role/GetRoleDetailsByDepartmentID?DepartmentId=',
    'GetRoleDetailsByDepartmentID': '/FunctionalRole/GetByDepartmentID?departmentId=',
    'GetLoggedInUserRoles': '/KRA/GetLoggedInUserRoles?employeeID=',
  },
  'WorkStation': {      
    'GetWorkStationListByBayId': '/WorkStation/GetWorkStationListByBayId?BayIds=',
    'GetWorkStationDataCount': '/WorkStation/WorkStationReportCount?bayId=',
    'GetWorkStationDetailByWorkStationCode':'/WorkStation/GetWorkStationDetailByWorkStationCode?workstationCode=',
    'DeskAllocation':'/WorkStation/EmployeeDeskAllocation?employeeId=',
    'ReleaseDesk':'/WorkStation/ReleaseDesk?employeeId=',
    'GetBaysList': '/Bay/GetBaysList',
    'create': '/Bay/CreateWorkStation'
  },
  'notificationemail': {
    'GetNotificationType': '/SystemNotification/GetNotificationType',
    'save': '/SystemNotification/SaveNotificationConfig',
    'list': '/SystemNotification/GetNotificationConfiguration?notificationType=',
    'GetAllEmailIds': '/SystemNotification/GetAllEmailIds',
  },
  'NotificationConfiguration': {
    'GetNotificationCofigurationByNotificationType': '/NotificationConfiguration/GetByNotificationTypeAndCategory?notificationTypeID=',
    'SaveNotificationCofiguration': '/NotificationConfiguration/Create',
    'UpdateNotificationCofiguration': '/NotificationConfiguration/Update',
    'FromEmail': 'it@senecaglobal.com',
    'GetNotificationType':  '/NotificationType/GetAll'
  },
  'NotificationType': {
    'GetNotificationType': '/NotificationType/GetAll',
    'AddNotificationType': '/NotificationType/Create',
    'UpdateNotificationType': '/NotificationType/Update',
    'DeleteNotificationType': '/NotificationType/Delete?notificationTypeId='
  },
  'common': {
    'getGetBusinessValues': '/Employee/GetBusinessValues/',
    'logError': '/ErrorLog/LogError'
  },
  'EmployeeStatus': {
    'GetUsers': '/MapAssociateId/GetUnMappedUsers',
    'GetNames': '/Employee/GetEmployeeNames',
    'UpdateEmployeeStatus': '/EmployeeStatus/UpdateEmployeeStatus/',
    'GetResignStatus': '/EmployeeStatus/GetEmployeeResignStatus',
    'MapAssociateId': '/MapAssociateId/MapAssociateId',
    'ReleaseTalentPool': '/AssociateAllocation/ReleaseOnExit/'
  },
  'AssignMenusRole': {
    'getSourceMenuRoles': '/Menu/GetSourceMenuRoles/',
    'getTargetMenuRoles': '/Menu/GetTargetMenuRoles/',
    'addTargetMenuRoles': '/Menu/AddTargetMenuRoles'
  },
  'Masterdata': {
    'getDepartmentsList': '/UserDepartment/GetUserDepartmentDetails',
    'getDepartments': '/Department/GetAll',
    'getRoles': '/UserRole/GetRoles',
    'getRolesByDepartmentID': '/Role/GetRolesByDepartmentID?DepartmentId=',
    'getRolesByProjectId': '/ProjectRoleAssignment/GetRolesByProjectId?projectId=',
    'getCompetencyAreas': '/CompetencyArea/GetCompetencyAreaDetails',
    'getSkillGroupsByCompetenctArea': '/SkillGroup/GetSkillGroupByCompetencyAreaID?competencyAreaID=',
    'getSkillsBySkillGroup': '/Skill/GetBySkillGroupId?skillGroupIds=',
    'getProficiencyLevels': '/ProficiencyLevel/GetProficiencyLevels',
    'getProjectList': '/Project/GetProjectsForAllocation',
    'getProjectsForDropdown':'/Project/GetProjectsForDropdown',
    'getManagersAndCompetencyLeads': '/Employee/GetManagersAndLeads',
    'getDomains': '/Domain/GetAll',
    'getFinancialYears': '/FinancialYear/GetAll',
    'getEmployeeNameByEmployeeId': '/MasterData/GetEmployeeNameByEmployeeId?employeeID=',
    'getAllEmailIDs': '/NotificationConfiguration/GetEmployeeWorkEmails?searchString=',
    'getEmployeesAndManagers': '/Project/GetEmployees?searchString=',
    'getAllLeadsManagers': '/Project/GetAllLeadsManagers?searchString=',
    'getAllAssociateList': '/Appreciation/GetAssociateNamesList',
    'getMasterSkillList': '/AssociateSkills/GetSkillsData',
    'getPractiseAreas': '/PracticeArea/GetAll',
    'getGradesDetails': '/UserGrade/GetGradesDetails',
    'getClientList': '/Client/GetAll',
    'getDesignationList': '/Designation/GetDesignationDetails',
    'getAllocationPercentages': '/AllocationPercentage/GetAll',
    'getProgramManagers': '/MasterData/GetProgramManagers',
    'getProjectTypes': '/ProjectType/GetAll',
    'getRoleCategory': '/KRA/GetRoleCategory',
    'getKRAOperators': '/KRA/GetOperators',
    'getKRAMeasurementType': '/KRA/GetKRAMeasurementType',
    'getKRATargetPeriods': '/KRA/GetTargetPeriods',
    'getKRAScales': '/KRA/GetKRAScales',
    'getKRAScaleValues': '/KRA/GetKRAScaleValues',
    'getDepartmentByDepartmentTypeId': '/MasterData/GetDepartmentByDepartmentTypeId?departmentTypeId=',
    'getDepartmentTypes': '/MasterData/GetDepartmentTypes',
    'getDesignationByString': '/Designation/GetBySearchString/',
    'getGradeByDesignation': '/Grade/GetGradeByDesignation/',
    'GetCategories': '/CategoryMaster/GetAll',
    'getAllDepartments': '/UserDepartment/GetDepartmentDetails',
    'getKRARoles':'/KRARole/GetKRARoles'
  },
  'Technology': {
    'list': '/PracticeArea/GetAll'
  },
  'KraDetailsForDepartmentHead': {
      "getDepartmentsByEmployeeId": "/KRA/GetDepartmentsByDepartmentHeadID?employeeID=",
      "getKraRolesForDH": "/KRA/GetKRARolesForDH?financialYearID=",
      "sendBackToHRHead": "/KRA/SendBackForHRHeadReview",
      "createKraByDH": "/KRA/CreateKRAByDepartmentHead",
      "updateKraByDH": "/KRA/UpdateKRARoleMetricByDepartmentHead",
      "getKraWorkFlowPendingWithEmployee": "/KRA/GetKRAWorkFlowPendingWithEmployeeID?financialYearID="
    },

  'CompetencyArea': {
    // 'list': '/CompetencyArea/GetCompetencyAreaDetails/',
    // 'create': '/CompetencyArea/CreateCompetencyArea/',
    // 'update': '/CompetencyArea/UpdateCompetencyArea/',
    'list': '/CompetencyArea/GetAll',
    'create': '/CompetencyArea/Create',
    'update': '/CompetencyArea/Update',

  },
  'Grades': {
    'list': '/Grade/GetAll',
    'create': '/Grade/Create/',
    'update': '/Grade/Update/',

  },
  'ApplicableRoleType': {
    'getApplicableRoleType': '/ApplicableRoleType/GetAll?FinancialYearId=',
    'addKRAApplicableRoleType': '/ApplicableRoleType/Create',
    'delete': "/ApplicableRoleType/Delete?applicableRoleTypeId=",
    'update': '/ApplicableRoleType/Update',
     'updateRoleTypeStatus': '/ApplicableRoleType/updateRoleTypeStatus',
  },

  'ProjectType': {
    // 'list': '/ProjectTypeMaster/GetProjectTypes/',
    // 'create': '/ProjectTypeMaster/CreateProjectType/',
    // 'update': '/ProjectTypeMaster/UpdateProjectType/',
    'list': '/ProjectType/GetAll',
    'create': '/ProjectType/Create',
    'update': '/ProjectType/Update',
  },
  'ProficiencyLevel': {
    // 'list': '/ProficiencyLevel/GetProficiencyLevels',
    // 'create': '/ProficiencyLevel/CreateProficiencyLevel/',
    // 'update': '/ProficiencyLevel/UpdateProficiencyLevel',

    'list': '/ProficiencyLevel/GetAll',
    'create': '/ProficiencyLevel/Create',
    'update': '/ProficiencyLevel/Update',
  },
  'ClientBillingRole': {
    'list': '/ClientBillingRoles/GetClientBillingRoles',
    'create': '/ClientBillingRoles/Create',
    'update': '/ClientBillingRoles/Update',
    'delete': '/ClientBillingRoles/Delete/',
    'getClientBillingRolesByProjectId': '/ClientBillingRoles/GetAllByProjectId?projectId=',
    'CloseClientBillingRole' : '/ClientBillingRoles/Close/',
    'GetEmployeesByClientBillingRoleId': '/ClientBillingRole/GetEmployeesByClientBillingRoleId?projectId=',
  },
  'Designation': {
    'list': '/Designation/GetAll',
    'create': '/Designation/Create',
    'update': '/Designation/Update'

  },
  'Department': {
    'list': '/Department/GetAll',
    'create': '/Department/Create',
    'update': '/Department/Update',
    'oldlist': '/UserDepartment/GetDepartmentDetails',
    // 'create': '/UserDepartment/CreateDepartment',
    // 'update': '/UserDepartment/UpdateUserDepartmentDetails',
    'GetRoleDetailsByDepartmentID': '/FunctionalRole/GetByDepartmentID?departmentId=',
    // 'GetRoleDetailsByDepartmentID': '/Role/GetRoleDetailsByDepartmentID?DepartmentId=',
    'GetDepartmentById': '/MasterData/GetDepartmentByDepartmentTypeId?departmentTypeId='
  },
  'DepartmentType' : {
    'list': '/DepartmentType/GetAll',
    'create': '/DepartmentType/Create',
    'update': '/DepartmentType/Update',
  },
  'UserNames': {
    'list': '/User/GetUsers',
    'getById': '/UserRole/GetUserRolesbyUserID/',
    'getRoleByDept': '/RoleMaster/GetRoleMasterDetails?departmentId=',
    'update': '/UserRole/UpdateUserRole',
    'create': '/UserRole/AssignRole'
  },
  
  "KRA": {
    "getFinancialYears": "/MasterData/GetFinancialYearList",
    "getKRAGroupsByYear": "/KRA/GetKRAGroups?financialYearId=",
    "getKRAGroupsByFinancialYear": "/KRA/GetKRAGroupsByFinancial?financialYearId=",
    "createKRAGroup": "/KRA/CreateKRAGroup",
    "createKRADefinition": "/KRA/CreateKRADefinition",
    "getKRADefinitionsById": "/KRA/GetKRADefinitionById?kraGroupId=",
    "updateKRADefinition": "/KRA/UpdateKRADefinition",
    "deleteKRADefinition": "/KRA/DeleteKRADefinition",
    "deleteKRAMetricByKRADefinitionId": "/KRA/DeleteKRAMetricByKRADefinitionId",
    "sendForDepartmentApproval": "/KRA/SubmitKRAForApproval",
    "approveKRA": "/KRA/ApproveKRA",
    "SendBackForHRMReview": "/KRA/SendBackForHRMReview",
    "SendBacktoDepartmentHead": "/KRA/SendBacktoDepartmentHead",
    "SendtoHRHeadReview": "/KRA/SendtoHRHeadReview",
    "getKRAComments": "/KRA/GetKRAComments?financialYearId=",
    "getRoles": "/Role/GetRoleSuffixAndPrefix",
    "getRolebyID": "/Role/GetRoleByRoleID?roleID=",
    "CloneKras": "/KRA/CloneKRAs",
    "getCurrentFinancialYear": "/MasterData/GetCurrentFinancialYear",
    "createKRAForRole": "/KRARole/CreateKRAForRole",
    "deleteKraRoleByYearIdAndRoleId": "/KRA/DeleteKRARoleByFinancialYearAndRoleId?roleID=",
    "submitKraForHRHeadReview": "/KRA/SubmitKRAForHRHeadReview",
    "getKRARolesByDepartmentID " : "/KRA/GetKRARolesByDepartmentID?departmentID=",
    "getKraRolesByDepartmentId": "/KRA/GetKRARolesForReview?financialYearID=",
    "getKraAspects": "/KRAAspects/GetKRAAspect?departmentId=",
    "getRoleName": "/KRARole/GetRoleName?roleId=",
    "getDepartmentName": "/KRARole/GetDepartmentName?departmentId=",
    "getFinancialYear": "/KRARole/GetFinancialYear?financialYearId=",
    "cloneYearwise": "/KRA/CloneKRATemplateForFinancialYear",
    "getKRAGroupList": "/Role/GetKRATitleByDepartmentID?departmentID=",
    "addKRAComments": "/KRA/AddKRAComments",
    "deleteKRAGroup": "/KRA/DeleteKRAGroup",
  },

  'FinancialYears': {
    "GetFinancialYearList": "/FinancialYear/GetFinancialYears",
    "GetCurrentFinancialYear": "/FinancialYear/GetCurrentFinancialYearByMonth",
    "CreateFinancialYear": "/FinancialYear/CreateFinancialYear",
    "UpdateFinancialYear": "/FinancialYear/UpdateFinancialYear",
  },

  'FinancialYear' : {
    "getFinancialYears": "/FinancialYear/GetAll",
  },

  "CustomKras": {
    "getProjectsByProgramManagerId": "/CustomKRAs/GetProjectsByProjectManagerID?projectManagerID=",
    "getEmployeesByProjectId": "/CustomKRAs/GetEmployeesByProjectID?projectID=",
    "getEmployeesByDepartment": "/CustomKRAs/GetEmployeesForDepartment?employeeID=",
    "saveCustomKras": "/CustomKRAs/SaveCustomKRAs",
    "editCustomKras": "/CustomKRAs/EditCustomKRAs",
    "deleteCustomKra": "/CustomKRAs/DeleteCustomKRA?customKRAID=",
    "getOrganizationAndCustomKrasOfEmployee": "/CustomKRAs/GetOrganizationAndCustomKRAsForPMDH?employeeID=",
  },
  
  'KraAspect': {    
    'getDepartmentsList': '/Department/GetUserDepartmentDetails',
    'getKraAspects': '/KRAAspects/GetKRAAspect/',
    'creatKraAspect': '/KRAAspects/Create',
    'updateKraAspect': '/KRAAspects/Update',
    'deleteKRAAspect' : '/KRAASpects/Delete',

  },
  "MeasurementType": {
    "getMeasurementType": "/MeasurementType/GetAll",
    "deleteMeasurementType": "/MeasurementType/Delete?MeasurementTypeId=",
    "createMeasurementType": "/MeasurementType/Create",
    "updateMeasurementType": "/MeasurementType/Update"
  },
  "AspectMaster": {
    "GetAspects": "/Aspect/GetAll",
    "CreateAspect": "/Aspect/Create",
    "UpdateAspect": "/Aspect/Update",
    "DeleteAspect": "/Aspect/Delete/"
  },
  "ScaleMaster": {
    "getKRAScale": "/Scale/GetAll",
    "getKRADescriptionDetails":"/Scale/GetScaleDetailsById/",
    "createKRAScale": "/Scale/Create",
    "updateKRAScale": "/Scale/Update",
    "deleteKRAScale": "/Scale/Delete?ScaleID="
  },
  "AssociateKras": {
    "GetAssociateKRAs": "/KRA/GetViewKRAByEmployee?employeeID=",
    "getEmployeesByProjectID": "/MasterData/GetEmployeesForViewKraInformation?departmentId=",
    "GenerateKRAPdfForAllAssociates" : "/AssociateKRAMapping/GenerateKRAPdfForAllAssociates?overideExisting=",
  },
  "MapAssociateRole": {
    "getEmployeesByProjectID": "/MasterData/GetEmployeesByDepartmentAndProjectID?departmentId=",
    "getRolesOfDeliveryDepartment": "/MasterData/GetKRARolesForProjectDeliveryDepartment",
    "mapRole": "/AssociateKRAMapping/UpdateAssociateRole",
    "getRolesbyDepartmentId": "/Role/GetRolesByDepartmentID?DepartmentId=",
    "getKraRolesByDepartmentId" : "/Role/GetKRARolesByDepartmentID?departmentID=",
    "getEmployeesByKraRoleIdAndFinancialYearId" : "/KRARole/GetEmployeesByKRARole?KRARoleId="
  },

  'Clients': {
    'list': '/Client/GetAll',
    'create': '/Client/Create',
    'update': '/Client/Update'
  },

  'PracticeArea': {
    'list': '/PracticeArea/GetAll',
    'create': '/PracticeArea/Create',
    'update': '/PracticeArea/Update'
  },
  'CategoryMaster': {
    'list': '/CategoryMaster/GetAll',
    'create': '/CategoryMaster/Create',
    'update': '/CategoryMaster/Update',
    'getParentCategoies': '/CategoryMaster/GetParentCategoies'
  },
  'Domain': {
    'list': '/Domain/GetAll',
    'create': '/Domain/Create',
    'update': '/Domain/Update',
  },
   'DefineKRA': {
     "createDefinition": "/Definition/Create",
     "updateKRA": "/Definition/Update",
      "getDefinitionDetails": "/Definition/GetDefinitionDetails?Id=",
     "getDefinitionsById": "/Definition/GetKRAs?financialYearId=",
     "deleteKRA": "/Definition/Delete?definitionDetailId=",
     "deleteKRAByHOD": "/Definition/DeleteByHOD?defintionDetailId=",
  },
   'KRAStatus': {    
     "getHRKRAStatuses": "/KRAStatus/GetKRAStatusByFinancialYearId",
      "getHODKRAStatuses": "/KRAStatus/GetKRAStatus",
      "updateKRAStatusToSendToHOD": "/KRAStatus/UpdateDefinitionDetails",
      "sendToCEO": "/KRAStatus/SendToCEO",
      "sendToHR": "/KRAStatus/UpdateRoleTypeStatus",
       
  },
  'KeyFunction':{
    'list': '/KeyFunction/GetAll',
    'create': '/KeyFunction/Create',
    'update': '/KeyFunction/Update',
  },
  'SkillGroup': {
    'list': '/SkillGroup/GetAll',
    'create': '/SkillGroup/Create/',
    'update': '/SkillGroup/Update/',
    'getSkillGroupsByCompetenctArea': '/SkillGroup/GetByCompetencyAreaId/'
  },

  'Skills': {
    'list': '/Skill/GetAll',
    'create': '/Skill/Create/',
    'update': '/Skill/Update/'
  },
  'Seniority':{
    'list': '/Seniority/GetAll',
    'create': '/Seniority/Create/',
    'update': '/Seniority/Update/'
  },
  'Speciality':{
    'list': '/Speciality/GetAll',
    'create': '/Speciality/Create/',
    'update': '/Speciality/Update/'
  },

  'projects': {
    'getProjectbyID': '/Project/GetByProjectId/',
    'getProjectTypes': '/ProjectType/GetAll', // Org - ProjectController
    'getEmpList': '/Employee/GetEmpList',// Emp - GetAll
    'getClients': '/Client/GetAll', //Org - Clients
    'getStatusDetails': '/Status/GetStatusMasterDetails',//Org
    'addProject': '/Project/Create', //Create
    'updateProject': '/Project/Update', //Update
    'getProgrammangers': '/Employee/GetProgramManagersList',//Employee
    'getProjectList': '/Project/GetProjectsList?userRole=',//Project
    'getManagers': '/UserRole/GetProgramManagers?userRole=',// Org
    'deleteProject': '/Project/DeleteProject?projectId=',//Project
    'submitForApproval' : '/Project/SubmitForApproval/',//ProjectController
    'approveOrRejectByDH' : '/Project/ApproveOrRejectByDH/',//ProjectController
    'GetProjectsStatuses' : '/status/GetProjectStatuses',// Org
    'canCloseProject' :'/Project/HasActiveClientBillingRoles',//ProjectController
    'closeProject' : '/Project/CloseProject',//ProjectController
  },
  'sow': {
    'getSOWDetailsById': '/SOW/GetAllByProjectId?projectId=',
    'createSOW': '/SOW/Create',
    'createAddendum':'/Addendum/Create',
    'GetAddendumsBySOWId': '/Addendum/GetAllBySOWIdAndProjectId?sowId=',
    'GetSowDetails': '/SOW/GetByIdAndProjectId?id=',
    'delete': '/SOW/Delete?Id=',
    'deleteAddendum':'/Addendum/Delete',
    'GetAddendumDetailsById': '/Addendum/GetByIdAndProjectId?id=',
    'updateSOWDetails' : '/SOW/Update',
    'UpdateAddendumDetails' : '/Addendum/Update'
    //'UpdateSOWAndAddendumDetails': '/project/UpdateSOWAndAddendumDetails'
  },
  'SkillSearch': {
    'getEmployeeDetailsBySkill': '/Reports/GetEmployeeDetailsBySkill/',
    'getAllSkillDetails': '/SkillSearch/GetAllSkillDetails?empId=',
    'GetSkillsBySearchString': '/Report/GetSkillsBySearchString?searchString=',
    'GetProjectDetailByEmployeeId' : '/SkillSearch/GetProjectDetailByEmployeeId?employeeId='
  },
  'AssociateJoining': {
    'getJoinedAssociates': '/Employee/GetJoinedEmployees'
  },
  'AssociateInformation': {
    'getInfoAssociates': '/Employee/GetEmployeeInfo'
  },
  'Dashboard':{
    "getPendingProfiles":'/Employee/GetPendingProfiles'
  },
  'personal':{
    'getBusinessValues': '/Employee/GetBusinessValues/',
    'getHRAdvisors': '/UserRole/GetHRAAdvisors',
    'getEmpTypes': '/Employee/GetEmpTypes',
    'getTechnologies': '/PracticeArea/GetAll',
    'getByPAId': '/ProspectiveAssociate/GetbyId/',
    'getByEmpId': '/EmployeePersonalDetails/GetPersonalDetailsByID/',
    'save': '/EmployeePersonalDetails/AddPersonalDetails',
    'update': '/EmployeePersonalDetails/UpdatePersonalDetails',
  },
  'PAssociate': {
    'list': '/ProspectiveAssociate/GetProspectiveAssociates',
    'create': '/ProspectiveAssociate/Create',
    'update': '/ProspectiveAssociate/Update',
    'get': '/ProspectiveAssociate/GetbyId/',
    'deletePA': '/ProspectiveAssociate/DeletePADetailsByID?empID=',
    'profileApproval':'/ProspectiveAssociate/UpdateEmployeeProfileStatus'
  },

  'associates': {
    'getAssociateDetails': '/AssociatePersonalDetail/GetAssociateDetails',
    'getAssociateDetailsByEmpID': '/AssociatePersonalDetail/GetAssociateDetailsByEmpID?empID=',
    'create': '/ProspectiveAssociate/AddPADetails',
    'update': '',
    'get': '',
    'getJoinedAssociates': '/ProspectiveAssociate/GetJoinedAssociates'
  },
  'upload': {
    // 'list': '/FileUploadAndDownload/GetFileUploadedData?empID=',
    'GetPAstatus': '/Employee/GetStatusbyId/',
    // 'save': '/FileUploadAndDownload/PostFile',
    // 'delete': '/FileUploadAndDownload/DeleteFileUploadedData',
    // 'submitForApproval': '/AssociatePersonalDetail/UpdateAssociateStatus?employeeId=',
    'save': '/EmployeeFiles/Save',
    'list': '/EmployeeFiles/GetByEmployeeId/',
    'delete': '/EmployeeFiles/Delete/',
    'submitForApproval': '/ProspectiveAssociate/UpdateEmployeeStatusToPending',
  },
  'associateprojects': {
    // 'get': '/AssociateSkills/GetAssociateProjectDetailsByID?empID=',
    // 'create': '/AssociateSkills/AddAssociateProjects',
    // 'update': '/AssociateSkills/UpdateAssociateProjectDetails'
    'get': '/EmployeeProject/GetByEmployeeId/',
    'update': '/EmployeeProject/Create'
  },
  'ProjectRoleAllocation': {
    'AssignProjectRole': '/ProjectRoleAssignment/AssignProjectRole',
    'GetRoleNotifications': '/ProjectRoleAssignment/GetRoleNotifications?employeeId=',
    'ApproveRole': '/ProjectRoleAssignment/RoleApproval',
    'GetAssignedRoles': '/ProjectRoleAssignment/GetAssignedRoles?projectId=',
    'RejectRole': '/ProjectRoleAssignment/RoleRejection',
    'GetProjectManagerOrLeadId': '/ProjectRoleAssignment/GetProjectManagerOrLeadId?employeeId=',
    'GetRolesByProjectId': '/ProjectRoleAssignment/GetRolesByProjectId?projectId=',
    'GetRoleDataById': '/ProjectRoleAssignment/GetRoleMasterDetailByRoleId?roleId='
  },
  'deliveryHead': {
    'getPendingRequisitionsForApproval': '/Dashboard/GetPendingRequisitionsForApproval?EmployeeId=',
    'getRolePositionDetailsByTRID': '/Dashboard/GetRolesAndPositionsByRequisitionId?talentRequisitionId=',
    'getTrRoleEmployeeList': '/Dashboard/GetTaggedEmployeeByTRAndRoleId?talentRequisitionId=',
    'getApproverList': '/Dashboard/GetFinanceHeadList',
    'ApproveTalentRequisition': '/Dashboard/ApproveTalentRequisition',
    'rejectTalentRequisition': '/TalentRequisition/RejectTalentRequisitionByDeliveryHead',
    'getProjectList': '/Project/GetProjectsList?userRole=',
    'ApproveOrRejectByDH' : '/Project/ApproveOrRejectByDH/'
  },
  'FinanceHead': {
    'getPendingRequisitionsForApproval': '/Dashboard/GetRequisitionsForApprovalForFinance?employeeId=',
    'requisitionApprovalByFinance': '/Dashboard/ApproveTalentRequisitionByFinance',
    'requisitionRejectionByFinance': '/TalentRequisition/RejectTalentRequisitionByFinance'
  },
  'associateSkills': {
    // 'getSkillsList': '/AssociateSkills/GetSkills',
    // 'getSkillsById': '/AssociateSkills/GetDraftApproveStatusSkills?empId=',
    'getCompetencyAreas': '/AssociateSkills/GetCompetencyArea',
    'getPracticeAreas': '/PracticeArea/GetPracticeAreas',
    'getProficiencyLevels': '/AssociateSkills/GetProficiencyLevel',
    // 'SaveAssociateSkills': '/AssociateSkills/AddAssociateSkills',
    'DeleteAssociateSkills': '/AssociateSkills/DeleteAssociateSkill?id=',
    'getSkillsByCompetenctArea': '/AssociateSkills/GetSkills?competenctAreaID=',
    'getSkillsByCompetenctAreaAndSkillGroup': '/AssociateSkills/GetSkillsByID?skillGroupID=',
    'SubmitAssociateSkills': '/AssociateSkills/SubmitAssociateSkills',

    'getSkillsById': '/EmployeeSkill/GetByEmployeeId/',
    'SaveAssociateSkills': '/EmployeeSkill/Create',
    'UpdateAssociateSkills': '/EmployeeSkill/Update',
    'getSkillsList': '/Skill/GetAll',
  },

  // 'SkillSearch': {
  //   'getEmployeeDetailsBySkill': '/SkillSearch/getEmployeeDetailsBySkill/',
  //   'GetSkillsBySearchString': '/SkillSearch/GetSkillsBySearchString?searchString='
  // },
  // 'AssociateJoining': {
  //   'getJoinedAssociates': '/ProspectiveAssociate/GetJoinedAssociates'
  // },
  // 'AssociateInformation': {
  //   'getInfoAssociates': '/AssociatePersonalDetail/GetAssociateDetails'
  // },
  // "personal": {
  //   "getByEmpId": "/AssociatePersonalDetail/GetPersonalDetailsByID?empID=",
  //   "getByPAId": "/ProspectiveAssociate/GetPADetailsByID?ID=",
  //   "save": "/AssociatePersonalDetail/AddPersonalDetails",
  //   "update": "/AssociatePersonalDetail/UpdatePersonalDetails",
  //   "getManagersAndLeads": "/Project/GetManagersAndLeads",
  //   "getUserDepartmentDetails": "/UserDepartment/GetUserDepartmentDetails",
  //   "getDesignations": "/ProspectiveAssociate/GetDesignationByGrade?gradeID=",
  //   "getGradesDetails": "/UserGrade/GetGradesDetails",
  //   "getHRAdvisors": "/ProspectiveAssociate/GetHRAdvisors",
  //   "getEmpTypes": "/ProspectiveAssociate/GetEmpTypes",
  //   "getTechnologies": "/PracticeArea/GetPracticeAreas"
  // },
  // "PAssociate": {
  //   "list": "/ProspectiveAssociate/GetPADetails",
  //   "create": "/ProspectiveAssociate/AddPADetails",
  //   "update": "/ProspectiveAssociate/UpdatePADetails",
  //   "get": "/ProspectiveAssociate/GetPADetailsByID?id=",
  //   "deletePA": "/ProspectiveAssociate/DeletePADetailsByID?empID="
  // },
  // "associates": {
  //   "getAssociateDetails": "/AssociatePersonalDetail/GetAssociateDetails",
  //   "getAssociateDetailsByEmpID": "/AssociatePersonalDetail/GetAssociateDetailsByEmpID?empID=",
  //   "create": "/ProspectiveAssociate/AddPADetails",
  //   "update": "",
  //   "get": "",
  //   "getJoinedAssociates": "/ProspectiveAssociate/GetJoinedAssociates"
  // },
  // "upload": {
  //   "list": "/FileUploadAndDownload/GetFileUploadedData?empID=",
  //   "GetPAstatus": "/ProspectiveAssociate/GetPAStatus?empID=",
  //   "save": "/FileUploadAndDownload/PostFile",
  //   "delete": "/FileUploadAndDownload/DeleteFileUploadedData",
  //   "submitForApproval": "/AssociatePersonalDetail/UpdateAssociateStatus?employeeId="
  // },
  // "associateprojects": {
  //   "get": "/AssociateSkills/GetAssociateProjectDetailsByID?empID=",
  //   "create": "/AssociateSkills/AddAssociateProjects",
  //   "update": "/AssociateSkills/UpdateAssociateProjectDetails"
  // },

  'Reports': {
    'GetProjectsList': '/Dashboard/GetProjectsList',
    'GetFinanceReport': '/Reports/GetFinanceReports',
    'getFinanceReportToFreez': '/Reports/GetFinanceReportToFreez',
    'getRmgReportDataByMonthYear': '/Reports/GetRmgReportDataByMonthYear?month=',
    'GetUtilizationReportsByMonth': '/Reports/GetUtilizationReportsByMonth',
    'GetResourceReportByProjectId': '/Reports/GetResourceByProject/',
    'GetResourceReports': '/Reports/GetUtilizationReports',
    'GetUtilizationReportsByTechnologyId': '/Reports/GetUtilizationReportsByTechnology',
    'GetTalentpoolResourceCount': '/Reports/GetTalentPoolReportCount',
    'GetEmployeesByTalentPoolProjectID': '/Reports/GetTalentPoolReport?projectId=',
    'GetProjectsListByTypeId': '/Project/GetProjectsListByTypeId?projectTypeId=',
    'GetEmpList': '/Project/GetEmpList',
    'GetRolesList': '/Project/GetRolesList',
    'GetGradesDetails': '/UserGrade/GetGradesDetails',
    'GetCustomers': '/Project/GetCustomers',
    'GetDepartmentHeads': '/UserDepartment/GetDepartmentHeads',
    'GetManagers': '/Project/GetManagers',
    'getProjectTypes': '/ProjectType/GetAll',
    'GetActiveSkillsData': '/Dashboard/GetActiveSkillsData',
    'SaveReportData': '/Dashboard/SaveReportData',
    'ImportAssociateUtilizeReport': '/AssociateUtilization/ImportAssociateUtilizeReport',
    'UploadRMGReport': '/AssociateUtilization/ImportAssociateUtilizeReport',
    'GetDomainCountReport': '/Reports/GetDomainReportCount',
    'getEmployeesByDomainId': '/Reports/GetDomainReport?domainID=',
    'getEmployeesBySkill': '/Reports/SkillReport?competencyID=',
    'getEmployeesByTalentPoolProjectID': '/Reports/TalentPoolReport?projectID=',
    'GetEmployeesBySkill': '/Reports/GetAssociateSkillsReport',
    'GetSkillsBySearchString': '/AssociateSkills/GetSkillsBySearchString?searchString=',
    'GetProjectHistoryByEmployee': '/Reports/GetProjectHistoryByEmployee?employeeID='
  },

  'professional': {
    // 'getProfessionalDetails': '/AssociateProfessionalDetail/GetProfessionalDetailsByEmployeeID?employeeID=',
    // 'getSkillGroupCertificate': '/AssociateProfessionalDetail/GetSkillGroupsByCertificate',
    // 'addCertificationDetails': '/AssociateProfessionalDetail/AddCertificationDetails',
    // 'addMembershipDetails': '/AssociateProfessionalDetail/AddMembershipDetails',
    // 'updateCertificationDetails': '/AssociateProfessionalDetail/UpdateCertificationDetails',
    // 'updateMembershipDetails': '/AssociateProfessionalDetail/UpdateMembershipDetails',
    // 'deleteMembershipDetails': '/AssociateProfessionalDetail/DeleteProfessionalDetailsByID?id='
    'getProfessionalDetails': '/EmployeeProfession/GetByEmployeeId/',
    'getSkillGroupCertificate': '/SkillGroup/GetByCompetencyAreaCode/Certification',
    'addCertificationDetails': '/EmployeeProfession/CreateCertificate',
    'addMembershipDetails': '/EmployeeProfession/CreateMembership',
    'updateCertificationDetails': '/EmployeeProfession/UpdateCertificate',
    'updateMembershipDetails': '/EmployeeProfession/UpdateMembership',
    'deleteMembershipDetails': '/EmployeeProfession/Delete/'
  },

  'education': {
    'list': '/EmployeeEducation/GetById/',
    'save': '/EmployeeEducation/Save/'
  },

  'employment': {
    'GetEmploymentDetails': '/EmployeeEmployment/GetPrevEmploymentDetailsById/',
    'GetProfReferenceDetails': '/EmployeeEmployment/GetProfReferencesById/',
    'SaveEmployementDetails': '/EmployeeEmployment/Save'
  },
  'TagAssociate': {
    'GetTagListDetailsByManagerId': '/TagAssociate/GetTagListDetailsByManagerId?managerId=',
    'GetTagListNamesByManagerId': '/TagAssociate/GetTagListNamesByManagerId?managerId=',
    'CreateTagList': '/TagAssociate/CreateTagList/',
    'UpdateTagList': '/TagAssociate/UpdateTagAssociateList/',
    'DeleteTagAssociate': '/TagAssociate/DeleteTagAssociate?tagAssociateId=',
    'DeleteTagList': '/TagAssociate/DeleteTagList?tagListName='
  },
  'AssignReportingManager': {
    'GetManagersByProjectId': '/ProjectManager/GetReportingDetailsByProjectId/',
    'GetProgramManagerList': '/Project/GetManagerList',
    'GetReportingManagersList': '/Project/GetAssociates',
    'AssignReportingManager': '/ProjectManager/SaveManagersToProject',
    'UpdateReportingManagerToAssociate': '/ProjectManager/UpdateReportingManagerToAssociate/',
    'getManagerName': '/ProjectManager/GetManagerandLeadByProjectIDandEmpId/',
    'getManagersAndLeads': '/Project/GetManagersAndLeads',
    'getAllocatedAssociates': '/AssociateAllocation/GetAllocatedAssociates',
    'getEmployeesAndManagers': '/Employee/GetEmployeeBySearchString/',
    'getProjectDetails': '/Project/GetProjectsByEmpId/',
    'getAllLeadsManagers': '/ProjectManager/GetLeadsManagersBySearchString/',
  },
  'ResourceRelease': {
    'getTalentPools': '/Project/GetTalentPools',
    'getAssociates': '/Project/GetAssociates',
    'getProjectDetails': '/Project/GetProjectDetails?employeeId=',
    'releaseAssociate': '/Project/ReleaseAssociate',
    'getAssociateTalentPool': '/Project/GetEmpTalentPool/',
    'GetAllocationPercentages': '/Common/GetAllocationPercentages',
    'GetEmployeesByProjectID': '/Project/GetEmployeesByProjectID?projectID=',
  },
  'family': {
    'list': '/EmployeeFamilyDetails/GetFamilyDetailsById/',
    'save': '/EmployeeFamilyDetails/UpdateFamilyDetails'
  },
  'EmployeeType': {
    'list': '/Employee/GetEmpTypes'
  },
  'HRAdvisor': {
    'list': '/UserRole/GetHRAAdvisors'
  },
  'TemporaryAllocationRelease': {
    'GetEmployeesAllocations': '/AssociateAllocation/GetEmployeesForAllocations',
    'TemporaryReleaseAssociate': '/AssociateAllocation/TemporaryReleaseAssociate',
    'GetAssociatesToRelease': '/AssociateAllocation/GetAssociatesToRelease/',
  },
  'AssociateAllocation': {
    'GetApprovedTalentRequisitionList': '/Allocation/GetApprovedTalentRequisitionList',
    'GetTalentRequisitionById': '/Allocation/GetTalentRequisitionById?tRId=',
    'GetRequiredSkillsDetails': '/Allocation/GetRequiredSkillsDetails?requisitionRoleDetailId=',
    'ResourceAllocate': '/AssociateAllocation/Create',
    'GetReportingManager': '/Allocation/GetReportingManager',
    'forceClose': '/Allocation/CloseTalentRequisition?tRId=',
    'GetMatchingProfiles': '/Allocation/GetMatchingProfiles?requisitionRoleId=',
    'GetMatchingskillsforAssociate': '/Allocation/GetMatchingskillsforAssociate?requisitionRoleId=',
    'CheckPrimaryRoleofAssociate': '/Allocation/CheckPrimaryRoleofAssociate?associateID=',
    'SetPrimaryRoleofAssociate': '/Allocation/SetPrimaryRoleofAssociate',
    'GetClientBillingRoles': '/ClientBillingRole/GetClientBillingRoles?isActive=true',
    'GetClientBillingRolesByProjectId': '/ClientBillingRoles/GetAllByProjectId?projectId=',
    'GetInternalBillingRoles': '/InternalBillingRole/GetInternalBillingRoles?isActive=true',
    'GetTaggedEmployeesByTalentRequisitionId': '/Allocation/GetTaggedEmployeesByTalentRequisitionId?talentRequisitionId=',
    'GetEmpAllocationHistory': '/AssociateAllocation/GetEmpAllocationHistory/',
    'GetTaggedEmployeesForTalentRequisition': '/Allocation/GetTaggedEmployeesForTalentRequisition',
    'GetEmployeePrimaryAllocationProject': '/AssociateAllocation/GetEmployeePrimaryAllocationProject/',
    'getProjectsForAllocation': '/Project/GetProjectsForAllocation',
    'GetAllocationDetailsByAllocationId': '/Allocation/GetAllocationDetailsByAllocationId?AllocationId=',
    'GetExecutionProjectsList': '/Allocation/GetExecutionProjectsList?userRole=',
    'getRolesByDepartmentID': '/AssociateAllocation/GetRolesByDepartmentId/',
  },

  'ADROrganisationDevelopment':{
    'get' :'/ADROrganisationalDevelopment/GetADROrganisationDevelopment?financialYearId=',
    'create': '/ADROrganisationalDevelopment/CreateADROrganisationDevelopment',
    'update' :'/ADROrganisationalDevelopment/UpdateADROrganisationDevelopment?financialYearId=',
    "getCurrentFinancialYear": "/MasterData/GetCurrentFinancialYear"
  },

  'ADROrganisationValue':{
    'get' :'/ADROrganisationValue/GetADROrganisationValues?financialYearId=',
    'create': '/ADROrganisationValue/CreateADROrganisationValue',
    'update' :'/ADROrganisationValue/UpdateADROrganisationValue?financialYearId=',
    "getCurrentFinancialYear": "/MasterData/GetCurrentFinancialYear",
  },

  'ADROrganisationDevelopmentEntry':{
    'get' :'/ADR/GetADROrganisationDevelopmentDetail?EmployeeID=',
    "getCurrentFinancialYear": "/MasterData/GetCurrentFinancialYear"
  },
  
  'ADRConfiguration':{
    'get' :'/ADRConfiguration/GetADRConfiguration',
    'create': '/ADRConfiguration/CreateADRConfiguration',
    'update' :'/ADRConfiguration/UpdateADRConfiguration'
  },

  'ADRSections' : {
    'get' :'/ADR/GetADRSection',
    'create': '/ADR/CreateADRSection',
    'update' :'/ADR/UpdateADRSection',
    'getADRMeasurementAreas' : '/ADR/GetCurrentYearADRMeasurementAreas'
  },

  "AssociateDevelopmentReview": {
    "GetADRCycleList": "/MasterData/GetADRCycle",
    "UpdateADRCycle": "/ADR/SetADRCycleActive"
  },
  
  "ADRAssociateAppreciation": {
    "GetSentAppreciationsList": "/Appreciation/GetAppreciationHistory?empID=",
    "GetReceiveAppreciationsList": "/Appreciation/GetMyAppreciation?empID=",
    "GetADRCycleList": "/MasterData/GetADRCycle",
    "GetAppreciationTypeList": "/Appreciation/GetAppreciationTypeList",
    "GetFinancialYearList": "/MasterData/GetFinancialYearList",
    "SendAnAppreciation": "/Appreciation/CreateAppreciation",
    "UpdateAnAppreciation": "/Appreciation/UpdateAnAppreciation",
    "DeleteAnAppreciation": "/Appreciation/DeleteAnAppreciation",
    "GetSourceOfOriginList": "/Appreciation/GetSourceOfOriginName"
  },
  "ADRKra": {
    "GetAssociateADRDetail": "/ADR/GetAssociateADRDetail?employeeID=",
    "UpdateAssociateADRDetail": "/ADR/UpdateAssociateADRDetail",
    "CreateAssociateADRDetail": "/ADR/CreateAssociateADRDetail",
  },
  "RoleType": {
    "Get": "/RoleType/GetAll",
    "Create": "/RoleType/Create",
    "Update": "/RoleType/Update",
    "Delete": "/RoleType/Delete",
    "getRoleTypesByGrade": "/RoleType/GetRoleTypesByGradeId/"
  },
};
