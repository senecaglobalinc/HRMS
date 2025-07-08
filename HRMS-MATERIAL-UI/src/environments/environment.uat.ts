// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
    production: false,
    BiometricAttendanceStartDate:new Date(2023, 11, 1),
    minYearOfAttendance:2023,
    /* HTTP port URLs*/
    ServerUrl: '',
    FutureMarkingValidDays: 14,
     IsDecryptionRequired : {
      'Corporate': true,
      'Associate': true,
    },
    
 AdminMicroService: 'https://uat-hrms-admin.senecaglobal.in/admin/api/v1',
  EmployeeMicroService: 'https://uat-hrms-employee.senecaglobal.in/employee/api/v1',
  ProjMicroService: 'https://uat-hrms-project.senecaglobal.in/project/api/v1',
  KRAMicroService: 'http://192.168.11.50:6024/kra/api/v1',
  ReportMicroService: 'https://uat-hrms-report.senecaglobal.in/report/api/v1',
    
    //AdminMicroService: 'https://hrms.senecaglobal.com:4021/admin/api/v1',
    //EmployeeMicroService: 'https://hrms.senecaglobal.com:4022/employee/api/v1',
    //ProjMicroService: 'https://hrms.senecaglobal.com:4023/project/api/v1',
    //KRAMicroService: 'https://hrms.senecaglobal.com:4024/kra/api/v1',
    //ReportMicroService: 'https://hrms.senecaglobal.com:4025/report/api/v1', 
  

      //sg authentication
      Url :'https://dev-auth.senecaglobal.in/get_access_token',
   authUrl :`https://dev-auth.senecaglobal.in/login?redirect_url=${window.location.origin + '/auth-callback'}`,
   client_id : 'a1841e4e-9864-4160-8ae4-506d94448a2d',
   logoutUrl : `https://dev-auth.senecaglobal.in/logout?redirect_url=http://192.168.11.50:4020/`
  };
  
  /*
   * For easier debugging in development mode, you can import the following file
   * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
   *
   * This import should be commented out in production mode because it will have a negative impact
   * on performance if an error is thrown.
   */
  // import 'zone.js/dist/zone-error';  // Included with Angular CLI.