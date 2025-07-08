// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  //,'http://hrmsdev/HRMS2Dev.Service/api',// http://localhost/AP.Services/api
     
  /* HTTP port URLs*/
  //ServerUrl: 'http://sg-srv-kas:7071/api',
  
  AdminMicroService: 'http://localhost:5009/admin/api/v1',

  ProjMicroService: 'http://localhost:5010/project/api/v1',
  
  EmployeeMicroService: 'http://localhost:5011/employee/api/v1',
  
  KRAMicroService: 'http://localhost:5011/kra/api/v1',
  
  ReportMicroService: 'http://192.168.11.50:2025/report/api/v1',
  

  base64Key: '16rdKQfqN3L4TY7YktgxBw==',
  CryptoKey: '8080808080808080',
  logError: '/ErrorLog/LogError',
  CryptoIv: '8080808080808080',
  CLIENT_ID:  "b98d2445-204a-4dee-a2a5-dc48818e0d77",
  TENANT_ID: "9b0861df-ad09-47df-b4d4-9a00828ab9f0",
  GRAPH_RESOURCE: "https://graph.microsoft.com",
  RedirectURL : "http://localhost:4200/authLogin",
  LogOutURL :"https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri=http://localhost:4200",
  Image : "https://graph.microsoft.com/v1.0/me/photo/$value",  
  departmentHeadDepartmentId:0
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
