// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: true, 
  BiometricAttendanceStartDate:new Date(2023, 11, 1),
  minYearOfAttendance:2023,
  /* HTTP port URLs*/
  ServerUrl: '',
  IsDecryptionRequired : {
    'Corporate': true,
    'Associate': true,
  },

  AdminMicroService: 'https://hrms-admin.senecaglobal.com/admin/api/v1',
  EmployeeMicroService: 'https://hrms-employee.senecaglobal.com/employee/api/v1',
  ProjMicroService: 'https://hrms-project.senecaglobal.com/project/api/v1',
  KRAMicroService: 'https://uat-hrms-kra.senecaglobal.in/kra/api/v1',
  ReportMicroService: 'https://hrms-report.senecaglobal.com/report/api/v1',
  

    //sg authentication
    Url :'https://auth.senecaglobal.com/get_access_token',
    authUrl :`https://auth.senecaglobal.com/login?redirect_url=${window.location.origin}`,
    client_id : 'a60ea2dd-4ea0-4ff5-a305-84070f85ddb6',
    logoutUrl : `https://auth.senecaglobal.com/logout?redirect_url=https://hrms.senecaglobal.com/`
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
