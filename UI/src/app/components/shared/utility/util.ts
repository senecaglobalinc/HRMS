import { Response } from '@angular/http';
//import { Observable } from 'rxjs/Observable';

export class Util {
    public static extractData(res: Response) {
        if (res.status < 200 || res.status >= 300) {
            throw new Error('Bad response status: ' + res.status);
        }
        return res.json();
    }

    public static handleError(error: any) {
        let errMsg = error._body || error.message || 'Server error';
        //console.error(errMsg); // log to console instead
        return (errMsg);
    }

    public static getEmployeeId() {
        let employeeId = JSON.parse(sessionStorage["AsscoiatePortal_UserInformation"]).employeeId;
        return employeeId;
    }

    public static getroleName() {
        let roleName = JSON.parse(sessionStorage["AsscoiatePortal_UserInformation"]).roleName;
        return roleName;
    }

     public static getLoginInfo() {
        let loginInfo = JSON.parse(sessionStorage["AsscoiatePortal_UserInformation"]);
        return loginInfo;
    }
}