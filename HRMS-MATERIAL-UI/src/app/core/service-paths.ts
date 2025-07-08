export const API = {
  'Menu': {
    'GetMenuDetails': '/Menu/GetMenuDetails/'
  },
  'Role': {
    'GetUserDetailsByUserName': '/UserRole/GetUserDetailsByUserName/',
    'GetLoggedInUserEmail':'/User/GetUserEmail'
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
  'ProjectClosure':{
    'SubmitForClosureApproval': '/ProjectClosure/SubmitForClosureApproval',
    'reject':'/ProjectClosure/RejectClosure',
    'ApproveOrRejectClosureByDH': '/ProjectClosure/ApproveOrRejectClosureByDH'
  },
  'Activity':{
    'Create': '/Activity/Create',
    'GetAll': '/Activity/GetAll',
    'ID': '/Activity/',
    'GetClosureActivitiesByDepartment': '/Activity/GetClosureActivitiesByDepartment?departmentId=',
    'GetExitActivitiesByDepartment': '/Activity/GetExitActivitiesByDepartment',
    'GetActivitiesForDropdown': '/Activity/GetActivitiesForDropdown',
    'Update': '/Activity/Update',
    'GetTransitionPlanActivities': '/Activity/GetTransitionPlanActivities',
    'getActivitiesByProjectIdAndDepartmentId': '/ProjectClosureActivity/GetActivitiesByProjectIdAndDepartmentId?projectId=',
    'CreateActivityChecklist': '/ProjectClosureActivity/CreateActivityChecklist',
    'UpdateActivityChecklist': '/ProjectClosureActivity/UpdateActivityChecklist',
    'GetActivitiesByProjectIdForPM': '/ProjectClosureActivity/GetActivitiesByProjectIdForPM?projectId='
  },
  'Roles': {
    'list': '/Role/GetRoles',
    'createRole': '/FunctionalRole/Create',
    'updateRole': '/FunctionalRole/Update',
    'createRoleSkills': '/Role/AddSkillsToRole',
    'updateRoleskills': '/Role/UpdateRoleSkills',
    'getRolebyID': '/Role/GetRoleByRoleID?roleID=',
    'getRoleSuffixAndPrefix': '/FunctionalRole/GetSGRoleSuffixAndPrefix?departmentId=',
    'GetRoleDetailsByDepartmentID': '/FunctionalRole/GetByDepartmentID?departmentId=',
    'GetLoggedInUserRoles': '/KRA/GetLoggedInUserRoles?employeeID=',
    'GetRolesAndDepartments': '/FunctionalRole/GetRolesAndDepartments'
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
    'GetAssociatesByDepartment' : '/Employee/GetAssociatesByDepartmentId',
    'GetDepartmentHeadByDepartment' : '/Employee/GetDepartmentHeadByDepartmentId/'
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
    "getUserDepartmentDetailsByEmployeeID": "/Department/GetUserDepartmentDetailsByEmployeeID",
    'getRolesByProjectId': '/ProjectRoleAssignment/GetRolesByProjectId?projectId=',
    'getCompetencyAreas': '/CompetencyArea/GetCompetencyAreaDetails',
    'getSkillGroupsByCompetenctArea': '/SkillGroup/GetSkillGroupByCompetencyAreaID?competencyAreaID=',
    'getSkillsBySkillGroup': '/Skill/GetBySkillGroupId?skillGroupIds=',
    'getProficiencyLevels': '/ProficiencyLevel/GetAll',
    'getProjectList': '/Project/GetProjectsForAllocation',
    'getManagersAndCompetencyLeads': '/Employee/GetManagersAndLeads',
    'getDomains': '/Domain/GetAll',
    'getFinancialYears': '/FinancialYear/GetAll',
    'getEmployeeNameByEmployeeId': '/MasterData/GetEmployeeNameByEmployeeId?employeeID=',
    'getAllEmailIDs': '/NotificationConfiguration/GetEmployeeWorkEmails',
    'getEmployeesAndManagers': '/Project/GetEmployees?searchString=',
    'getAllLeadsManagers': '/Project/GetAllLeadsManagers?searchString=',
    'getAllAssociateList': '/Appreciation/GetAssociateNamesList',
    'getMasterSkillList': '/AssociateSkills/GetSkillsData',
    'getPractiseAreas': '/PracticeArea/GetAll',
    'getGradesDetails': '/UserGrade/GetGradesDetails',
    'getClientList': '/Client/GetAll',
    'getDesignationList': '/Designation/GetAll',
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
    'getKRARoles':'/KRARole/GetKRARoles',
    'GetAssociateProjectsForRelease':'/Project/GetAssociateProjectsForRelease/'
  },
  'DropDownLists': {
    'GetAssociates': '/Employee/GetAssociatesForDropdown',
    'GetAssociatesByProject': '/Employee/GetAssociatesByProjectId/',
    'GetProjects': '/Project/GetProjectsForDropdown',
    'GetGrades': '/Grade/GetGradesForDropdown',
    'GetClients': '/Client/GetClientsForDropdown',
    'GetProgramManagers': '/ProjectManager/GetProgramManagersForDropdown',
    'GetDesignations': '/Designation/GetDesignationsForDropdown',
    'GetPercentages': '/AllocationPercentage/GetAllocationPercentageForDropdown',
    'GetTechnologies': '/PracticeArea/GetTechnologyForDropdown',
    'GetExitReasons':'/Reason/GetReasonsForDropdown',
    'GetExitTypes':'/ExitType/GetExitTypesForDropdown',
    'GetListAssociatesByRoles':'/Employee/GetListAssociatesByRoles/',
  },
  'Technology': {
    'list': '/PracticeArea/GetAll',
    'activelist': '/PracticeArea/GetAll?isActive=true'
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
    'getDepartments':'/GradeRoleType/GetDepartments',
    'getGradesByDepartment':'/GradeRoleType/GetGradesByDepartment',
    'getRoleTypesByGrade':'/GradeRoleType/GetRoleTypesByGrade',
    'getById':'/GradeRoleType/getById?',
    'getRoleTypesByDeptAndFY':'/RoleType/GetRoleTypesForDropdown?financialYearId=',
    'getGradesBySelectedRoleType': '/GradeRoleType/GetGradesByDepartment?financialYearId='
  },

  'ProjectType': {
    'list': '/ProjectType/GetAll',
    'create': '/ProjectType/Create',
    'update': '/ProjectType/Update',
  },
  'ProficiencyLevel': {
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
    'GetRoleDetailsByDepartmentID': '/FunctionalRole/GetByDepartmentID?departmentId=',
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
    'create': '/UserRole/AssignRole',
    'GetUsersBySearchString': '/User/GetUsersBySearchString/'
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
    "GenerateKRAPdf":"/KRA/GenerateKRAPdf",
    'getEmployeeRoleTypes':"/Employee/GetEmployeeRoleTypes", 
    'downloadKRA':"/Employee/DownloadKRA",   
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
     'DefineKRA': {
      "createDefinition": "/Definition/Create",
      "updateKRA": "/Definition/Update",
      "importKRA": "/Definition/ImportKRA",
      "getDefinitionDetails": "/Definition/GetDefinitionDetails?Id=",
      "getDefinitionsById": "/Definition/GetDefinitions?financialYearId=",
      "deleteKRA": "/Definition/Delete/",
      "deleteKRAByHOD": "/Definition/DeleteByHOD?defintionDetailId=",
      "deleteKRAByHODHardDelete": "/Definition/DeleteKRA?definitionDetailId=",
      "setPreviousMetricValues": "/Definition/SetPreviousMetricValues?defintionDetailId=",
      "setPreviousTargetValues": "/Definition/SetPreviousTargetValues?defintionDetailId=",
      "acceptTargetValue": "/Definition/AcceptTargetValue?defintionTransactionId=",
      "acceptMetricValue": "/Definition/AcceptMetricValue?defintionTransactionId=",
      "rejectTargetValue": "/Definition/RejectTargetValue?defintionTransactionId=",
      "rejectMetricValue": "/Definition/RejectMetricValue?defintionTransactionId=",
      "acceptDeletedKRAByHOD": "/Definition/AcceptDeletedKRAByHOD?defintionTransactionId=",
      "rejectDeletedKRAByHOD": "/Definition/RejectDeletedKRAByHOD?defintionTransactionId=",
      "acceptAddedKRAByHOD": "/Definition/AcceptAddedKRAByHOD?defintionTransactionId=",
      "rejectAddedKRAByHOD": "/Definition/RejectAddedKRAByHOD?defintionTransactionId=",
      "addKRAAgain": "/Definition/AddKRAAgain?defintionDetailId=",
      "updateKRAStatus": "/KRA/UpdateKRAStatus?financialYearId=",
  },
   'KRAStatus': {    
     "getHRKRAStatuses": "/KRAWorkFlow/GetOperationHeadStatus",
      "getHODKRAStatuses": "/KRAStatus/GetKRAStatus",
      "getCEOKRAStatuses": "/KRAStatus/GetKRAStatusByFinancialYearIdForCEO",
      "updateKRAStatusToSendToHOD": "/KRAWorkFlow/SendTOHod",
      "sendToCEO": "/KRAStatus/SendToCEO",
      "sendToHR": "/KRAStatus/UpdateRoleTypeStatus",
      "editByHR": "/KRAStatus/EditByHR",
      "ceoAccept": "/KRAStatus/UpdateRoleTypeStatusForCEO",       
  },

  'KRAWorkFlow': {
    "GetHODDefinitionsAsync": "/KRAWorkFlow/GetHODDefinitions",
    "HODDelete":"/KRAWorkFlow/HODDelete",
    "HODAdd":"/KRAWorkFlow/HODAdd",
    "HODUpdate":"/KRAWorkFlow/HODUpdate",
    "ApprovedbyHOD":"/KRAWorkFlow/ApprovedbyHOD",
    "SentToOpHead":"/KRAWorkFlow/SentToOpHead",
    "EditedByHOD":"/KRAWorkFlow/EditedByHOD",
    "GetOperationHeadDefinitions":"/KRAWorkFlow/GetOperationHeadDefinitions",
    "AcceptedByOperationHead":"/KRAWorkFlow/AcceptedByOperationHead",
    "RejectedByOperationHead":"/KRAWorkFlow/RejectedByOperationHead",
    "getCEOKRAStatuses": "/KRAWorkFlow/GetKRAStatusByFinancialYearIdForCEO",
    "approvedByCEO": "/KRAWorkFlow/ApprovedByCEO",
    "SendToCEO":"/KRAWorkFlow/SendToCEO"
  },

  'KRAComment': {
    "createComment": "/Comment/Create",
    "getComment": "/Comment/GetAll?financialYearId=",
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
    'update': '/Skill/Update/',
    'getSkillsBySkillGroup': '/Skill/GetskillsBySkillGroupId?SkillGroupId=',
    'GetSkillsBySearchString': '/Skill/GetSkillsBySearchString?skillsearchstring='
    
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
    'closeProject' : '/ProjectClosure/ProjectClosureInitiation',//ProjectController
    'createprojectclosurereport':'/ProjectClosureReport/Create',
    'updateprojectclosurereport':'/ProjectClosureReport/Update',
    'getclosurereportbyprojectid' : '/ProjectClosureReport/GetClosureReportByProjectId/',
    'savefiles' : '/ProjectClosureReport/Save'  ,
    'download':'/ProjectClosureReport/Download?',
    'delete':'/ProjectClosureReport/Delete/',
     
  },
  'sow': {
    'getSOWDetailsById': '/SOW/GetAllByProjectId?projectId=',
    'createSOW': '/SOW/Create',
    'createAddendum':'/Addendum/Create',
    'GetAddendumsBySOWId': '/Addendum/GetAllBySOWIdAndProjectId?sowId=',
    'GetSowDetails': '/SOW/GetByIdAndProjectId?id=',
    'delete': '/SOW/Delete',
    'deleteAddendum':'/Addendum/Delete',
    'GetAddendumDetailsById': '/Addendum/GetByIdAndProjectId?id=',
    'updateSOWDetails' : '/SOW/Update',
    'UpdateAddendumDetails' : '/Addendum/Update'
  },
  'SkillSearch': {
    'getEmployeeDetailsBySkill': '/SkillSearch/getEmployeeDetailsBySkill/',
    'GetAllSkillDetails': '/SkillSearch/GetAllSkillDetails?empID=',
    'GetActiveSkillsForDropdown': '/Skill/GetActiveSkillsForDropdown',
    'GetSkillsBySearchString': '/Report/GetSkillsBySearchString?searchString=',
    'GetProjectDetailByEmployeeId' : '/SkillSearch/GetProjectDetailByEmployeeId?employeeId='
  },
  'AssociateJoining': {
    'getJoinedAssociates': '/Employee/GetJoinedEmployees'
  },
  'AssociateInformation': {
    'getInfoAssociates': '/Employee/GetEmployeeInfo',
    'getInfoAssociatesbyPage': '/Employee/GetEmployeeOnPagination?searchString=',
    'getEmployeeCount': '/Employee/GetEmployeeCount?searchString=',
    'prospectiveToHrms':'/ProspectiveDetailsSync/ReadRepository'
  },
  'Dashboard':{
    "getPendingProfiles":'/Employee/GetPendingProfiles',
    "getRejectedProfiles":'/Employee/GetRejectedProfiles'
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
    'GetPAstatus': '/Employee/GetStatusbyId/',
    'save': '/EmployeeFiles/Save',
    'list': '/EmployeeFiles/GetByEmployeeId/',
    'delete': '/EmployeeFiles/Delete/',
    'submitForApproval': '/ProspectiveAssociate/UpdateEmployeeStatusToPending',
  },
  'associateprojects': {   
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
    'getCompetencyAreas': '/AssociateSkills/GetCompetencyArea',
    'getPracticeAreas': '/PracticeArea/GetPracticeAreas',
    'getProficiencyLevels': '/AssociateSkills/GetProficiencyLevel',     
    'DeleteAssociateSkills': '/AssociateSkills/DeleteAssociateSkill?id=',
    'getSkillsByCompetenctArea': '/AssociateSkills/GetSkills?competenctAreaID=',
    'getSkillsByCompetenctAreaAndSkillGroup': '/AssociateSkills/GetSkillsByID?skillGroupID=',
    'SubmitAssociateSkills': '/AssociateSkills/SubmitAssociateSkills',

    'getSkillsById': '/EmployeeSkill/GetByEmployeeId/',
    'SaveAssociateSkills': '/EmployeeSkill/Create',
    'UpdateAssociateSkills': '/EmployeeSkill/Update',
    'getSkillsList': '/Skill/GetAll',
    'deleteSkill':'/EmployeeSkill/DeleteSkill/'
  },
   'Reports': {
    'GetEmployeeDetailsBySkill': '/Reports/GetEmployeeDetailsBySkill',
    'GetProjectsList': '/Dashboard/GetProjectsList',
    'GetFinanceReport': '/Reports/GetFinanceReports',
    'getFinanceReportToFreez': '/Reports/GetFinanceReportToFreez',
    'getRmgReportDataByMonthYear': '/Reports/GetRmgReportDataByMonthYear?month=',
    'GetUtilizationReportsByMonth': '/Reports/GetUtilizationReportsByMonth',
    'GetResourceReportByProjectId': '/Reports/GetResourceByProject/',
    'GetResourceReports': '/Reports/GetUtilizationReports',
    'GetUtilizationReportsByTechnologyId': '/Reports/GetUtilizationReportsByTechnology',
    'GetTalentpoolResourceCount': '/Reports/GetTalentPoolReportCount',
    'GetEmployeesByTalentPoolProjectID': '/Reports/GetTalentPoolReport?projectID=',
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
    'GetEmployeesBySkill': '/Reports/GetAssociateSkillsReport',
    'GetSkillsBySearchString': '/AssociateSkills/GetSkillsBySearchString?searchString=',
    'GetProjectHistoryByEmployee': '/Reports/GetProjectHistoryByEmployee?employeeID=',
    'GetServiceTypeReportCount': '/Reports/GetServiceTypeReportCount?filter=',
    'GetServiceTypeReportEmployee': '/Reports/GetServiceTypeReportEmployee?serviceTypeId=',
    'GetServiceTypeReportProject': '/Reports/GetServiceTypeReportProject?serviceTypeId=',
    'GetAllProjectsForReport': '/Report/GetAllProjects',
    'GetProjectDetailsReport':'/Reports/GetProjectDetailsReport',
    'GetCriticalResourceReport':'/Reports/GetCriticalResourceReport',
    'GetNonCriticalResourceReport':'/Reports/GetNonCriticalResourceReport',
    'GetUtilizationReport':'/Report/GetUtilizationReport',
    'GetTalentPoolResourceReport':'/Reports/GetTalentPoolResourceReport',
    'GetAssociateExitReport':'/Report/GetAssociateExitReport',
    'GetAssociateExitChartReport': '/Report/GetAssociateExitChartReport',
    'GetAssociateExitReportTypes': '/Report/GetAssociateExitReportTypes',
    'GetAttendanceSummaryReport': '/AttendanceReport/GetAttendanceSummaryReport',
    'GetAttendanceDetailReport': '/AttendanceReport/GetAttendanceDetailReport',
    'GetAdvanceAttendanceReport':'/BioMetricAttendance/GetAdvanceAttendanceReport',
    'GetBiometricAttendanceSummaryReport': '/BioMetricAttendance/GetAttendanceSummaryReport',
    'GetBiometricAttendanceDetailReport': '/BioMetricAttendance/GetAttendanceDetailReport',
    'GetAssociates': '/AttendanceReport/GetAssociatesReportingToManager',
    'GetProjects': '/AttendanceReport/GetProjectsByManager',
    'GetAttendanceMaxDate': '/AttendanceReport/GetAttendanceMaxDate',
    'GetAssociatesForFutureAllocation':'/Reports/GetAssociatesForFutureAllocation',
    'IsDeliveryDepartment': '/AttendanceReport/IsDeliveryDepartment',
    'GetNonCriticalResourceBillingReport':'/Reports/GetNonCriticalResourceBillingReport',
    'IsDeliveryDepartmentforBiometric': '/BioMetricAttendance/IsDeliveryDepartment',
    'GetBiometricAttendanceMaxDate': '/BioMetricAttendance/GetAttendanceMaxDate',
    'GetProjectsForBiometricAttendance': '/BioMetricAttendance/GetProjectsByManager',
    'GetAssociatesForBiometricAttendance': '/BioMetricAttendance/GetAssociatesReportingToManager',
    'GetMusterReport':'/BioMetricAttendance/GetAttendanceMuster/'
  },

  'professional': {
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
    'list': '/UserRole/GetHRAAdvisors',
    'activelist': '/UserRole/GetHRAAdvisors?isActive=true'
  },

  
  'TemporaryAllocationRelease': {
    'GetEmployeesAllocations': '/AssociateAllocation/GetEmployeesForAllocations',
    'TemporaryReleaseAssociate': '/AssociateAllocation/TemporaryReleaseAssociate',
    'GetAssociatesToRelease': '/AssociateAllocation/GetAssociatesToRelease/',
    'GetAssociatesForAllocation': '/AssociateAllocation/GetAssociatesForAllocation'
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
    'GetCurrentAllocationByEmpIdAndProjectId': '/AssociateAllocation/GetCurrentAllocationByEmpIdAndProjectId?employeeId=',
    'UpdateAssociateAllocation' : '/AssociateAllocation/UpdateAssociateAllocation',
    'GetEmployeeAllocationsByName': '/Employee/GetEmployeeDetailsByNameString/',
    'AddAssociateFutureProject': '/AssociateAllocation/AddAssociateFutureProject',
    'GetAssociateFutureProjectByEmpId': '/AssociateAllocation/GetAssociateFutureProjectByEmpId/',
    'DiactivateAssociateFutureProjectByEmpId': '/AssociateAllocation/DiactivateAssociateFutureProjectByEmpId/',
    'GetAllAllocationByEmployeeId': '/AssociateAllocation/GetAllAllocationByEmployeeId/'
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
    "Create": "/GradeRoleType/Create",
    "Update": "/GradeRoleType/Update",
    "Delete": "/GradeRoleType/Delete",
    "getRoleTypesByGrade": "/RoleType/GetRoleTypesByGradeId/",
    "GetGradesByDepartment": "/GradeRoleType/GetGradesByDepartment?financialYearId=",
    "GetRoleTypesForDropdown": "/RoleType/GetRoleTypesForDropdown?financialYearId=",
  },
  "ServiceType":{
    "GetServiceTypeForDropdown": "/ServiceType/GetServiceTypeForDropdown",
    "GetAll": "/ServiceType/GetAll",
    "Create": "/ServiceType/Create",
    "Update": "/ServiceType/Update",

  },
  "EmployeeSkillWorkFlow":{
    "Create": "/EmployeeSkillWorkFlow/Create/",
    "updateEmpSkillDetailsByRM": "/EmployeeSkillWorkFlow/UpdateEmpSkillDetails/",
    "SkillStatusApprovedByRM": "/EmployeeSkillWorkFlow/SkillStatusApprovedByRM/",
    "getSkillSubmittedEmps": "/EmployeeSkillWorkFlow/GetSkillSubmittedByEmployee/",
    "getAllSubmittedSkillsByEmpid": "/EmployeeSkillWorkFlow/GetSubmittedSkillsByEmpid/",
    "UpdateEmpSkillProficienyByRM": "/EmployeeSkillWorkFlow/UpdateEmpSkillProficienyByRM",
    "GetEmployeeSkillHistory": "/EmployeeSkillWorkFlow/GetEmployeeSkillHistory/"
  },

  "AssociateResignation": {
    'GetAssociatesById': '/AssociateResignation/GetAssociatesById?resignEmployeeId=',
    'CreateAssociateResignation': '/AssociateResignation/CreateAssociateResignation',
    'CalculateNoticePeriod': '/AssociateResignation/CalculateNoticePeriod?resignationDate=',
    'RevokeResignation': '/AssociateResignation/RevokeResignationByID?empID=',
  },

  'AssociateLongLeave': {
    'CreateAssociateLongLeave': '/AssociateLongLeave/CreateAssociateLongLeave',
    'CalculateMaternityPeriod': '/AssociateLongLeave/CalculateMaternityPeriod?longLeaveStartDate=',
    'RejoinAssociate': '/AssociateLongLeave/ReJoinAssociateByID?empID=',
  },

  "Resignation": {
    "Reason": "/Reason/GetVoluntaryExitReasons",
    "Create": "/AssociateExit/Create",
    "GetExitDetailsById": "/AssociateExit/GetByEmployeeId",
    "GetAll": "/AssociateExit/GetAll",
    "GetAssociateExitDashbaord" : "/AssociateExit/GetAssociatesForExitDashboard/",
    "PMApproval": "/AssociateExit/Approve",
    "Revoke": "/AssociateExit/RevokeExit",
    "WithdrawRequest": "/AssociateExit/RequestForWithdrawResignation?employeeId=",
    "ApproveOrRejectRevoke":"/AssociateExit/ApproveOrRejectRevoke",
    "ReviewByPM": "/AssociateExit/ReviewByPM",
    "GetResignationSubStatus": "/AssociateExit/GetResignationSubStatus/",
    "ReviewReminderNotification":"/AssociateExit/ReviewReminderNotification/"
  },

  "TransitionPlan":{
    "GetForTeamLead": "/TransitionPlan/GetTransitionPlanByAssociateIdandProjectId/",
    "GetForAssociate": "/TransitionPlan/GetTransitionPlanByAssociateId",
    "Update": "/TransitionPlan/UpdateTransitionPlan/",
  	"DeleteActivity": "/TransitionPlan/DeleteTransitionActivity"
  },
  'AssociateExitActivity':{
    'GetActivitiesByEmployeeIdAndDepartmentId':'/AssociateExitActivity/GetActivitiesByEmployeeIdAndDepartmentId',
    'updateActivityChecklist':'/AssociateExitActivity/UpdateActivityChecklist',
    'GetActivitiesByemployeeIdForHRA' : "/AssociateExitActivity/GetActivitiesByemployeeIdForHRA",
    "ExitClearance":"/AssociateExit/ExitClearance",
    "GetClearanceRemarks":"/AssociateExit/GetClearanceRemarks",
    "CreateActivityChecklist":"/AssociateExitActivity/CreateActivityChecklist",

  },
    "AssociateExitInterview":{
      "CreateExitFeedback": "/AssociateExitInterview/CreateExitFeedback",
      "GetExitInterview": "/AssociateExitInterview/GetExitInterview"
      },
    "AssociateExitAnalysis":{
      "CreateExitAnalysis": "/AssociateExitAnalysis/CreateExitAnalysis",   
      "GetAssociateExitAnalysis": "/AssociateExitAnalysis/GetAssociateExitAnalysis",
      "GetExitEmployeeReport":"/Report/ExitEmployeeReport"
    },
    'AssociateExit':{
      'Create': '/AssociateExit/Create',
      'GetAll': '/AssociateExit/GetAll',
      'GetByEmployeeId': '/AssociateExit/GetByEmployeeId/',
      'Approve': '/AssociateExit/Approve',
      'getExitDashboards': '/AssociateExit/GetByEmployeeId/'
    },
    'WelcomeEmail':{
      'WelcomeEmail':'/WelcomeEmail/GetWelcomeEmployeeInfo',
      'SendWelcomeEmail':'/WelcomeEmail/SendWelcomeEmail'
    },
  'AssociateExitInterviewReviewInfo': {
    'AssociateExitInterviewReview': '/AssociateExitInterviewReview/GetAll',
    'Create': '/AssociateExitInterviewReview/Create'
  },
  'AssociateAbscond':{
    'GetAssociateByLead':'/AssociateAbscond/GetAssociateByLead/',
    'CreateAbscond':'/AssociateAbscond/CreateAbscond',
    'GetAssociatesAbscondDashboard':"/AssociateAbscond/GetAssociatesAbscondDashboard/",
    'AcknowledgeAbscond':'/AssociateAbscond/AcknowledgeAbscond',
    'ConfirmAbscond':'/AssociateAbscond/ConfirmAbscond',
    'GetAbscondDetailByAssociateId':'/AssociateAbscond/GetAbscondDetailByAssociateId/',
    'AbscondClearance':'/AssociateAbscond/AbscondClearance',
    'GetAbscondSubStatus': '/AssociateAbscond/GetAbscondSubStatus/'
  },
  'ChangeRmForNonDelivery' : {
    'GetAssociateRMDetailsByDepartmentId':'/Employee/GetAssociateRMDetailsByDepartmentId/',
    'GetNonDeleveryAssociates':'/Employee/GetServiceDepartmentAssociates',
    'UpdateAssociateRMDetails' : '/Employee/UpdateServiceDepartmentAssociateRM',
    'GetAllDepartment' : '/Department/GetAll?isActive=true'
  },

  'ParkingBookingSlot' : {
    'BookParkingSlot' : '/BookParkingSlot/Create',
    'GetSlotDetails' : '/BookParkingSlot/GetSlotDetails/',
    'GetSlotDetailsByEmailID' : '/BookParkingSlot/GetSlotDetailsByEmailID/',
    'ReleaseParkingSlot': '/BookParkingSlot/ReleaseSlot?email=',
    'GetParkingSlotReport': '/Report/GetParkingSlotReport'
  },

  'wfhAttendance' : {
    'GetloginStatus' : '/WorkFromHomeAttendance/GetloginStatus/',
    'SaveAttendanceDetails' : '/WorkFromHomeAttendance/SaveAttendanceDetais',
    'GetAttendanceDetails' : '/WorkFromHomeAttendance/GetAttendanceDetais/'
  },

  'attendanceRegularization' : {
    'GetNotPunchInDate' : '/AttendanceRegularization/GetNotPunchInDates',
    'SaveAttendanceRegularizationDetails' : '/AttendanceRegularization/SaveAttendanceRegularizationDetails',
    'GetAllAssociateSubmittedAttendanceRegularization' : '/AttendanceRegularization/GetAllAssociateSubmittedAttendanceRegularization/',
    'GetAssociateSubmittedAttendanceRegularization' : '/AttendanceRegularization/GetAssociateSubmittedAttendanceRegularization/',
    'ApproveOrRejectAttendanceRegularizationDetails' : '/AttendanceRegularization/ApproveOrRejectAttendanceRegularizationDetails'
  },

  'uploadLeaveData' : {
    'UploadLeaveData' : '/AssociateLeave/UploadLeaveData',
    'GetTemplateFile' : '/AssociateLeave/GetTemplateFile' 
  }
};
