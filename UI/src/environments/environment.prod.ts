export const environment = {
  production: true, // set to true
  ServerUrl: 'https://hrms.senecaglobal.com/hrms.service/api', // prod url
  AdminMicroService:'http://sg-srv-vtsapps:2021/admin/api/v1',
  base64Key: '16rdKQfqN3L4TY7YktgxBw==',
  CryptoKey: '8080808080808080',
  logError: '/ErrorLog/LogError', 
  CryptoIv: '8080808080808080',
  CLIENT_ID: "45ec14c2-0179-4d81-8170-ae485bcdc078",
  TENANT_ID: "9b0861df-ad09-47df-b4d4-9a00828ab9f0",
  GRAPH_RESOURCE: "https://graph.microsoft.com",
  RedirectURL : "https://hrms.senecaglobal.com", //production redirect url,
  Image : "https://graph.microsoft.com/v1.0/me/photo/$value",
  LogOutURL :"https://login.microsoftonline.com/common/oauth2/logout?post_logout_redirect_uri=https://hrms.senecaglobal.com", // change localhost url ...
};
