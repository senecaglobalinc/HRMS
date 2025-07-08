import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as environmentInformation from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { ErrorLogDetails } from '../models/errorLog.model';

import { FormControl } from '@angular/forms';
import { KRAStatusCodes } from '../utility/enums';

declare var $: any;
@Injectable({
    providedIn: 'root'
})
export class CommonService {
    private _resources: any;
    private _serverURL: string;
    private EmployeeMicroService = environmentInformation.environment.EmployeeMicroService;
    errorlogDetails: ErrorLogDetails;
    constructor( private _http: HttpClient) {
        this._serverURL = environmentInformation.environment.ServerUrl;
        
        this._resources = servicePath.API.common;

    }
    public GetBusinessValues(valueKey: string) {
        var url = this.EmployeeMicroService+ this._resources.getGetBusinessValues + valueKey;
        return this._http.get(url)
    }

    onlyNumbers(event: any) {
        var keys = {
            'escape': 27, 'backspace': 8, 'tab': 9, 'enter': 13, 'period': 46,
            '0': 48, '1': 49, '2': 50, '3': 51, '4': 52, '5': 53, '6': 54, '7': 55, '8': 56, '9': 57
        };
        for (var index in keys) {
            if (!keys.hasOwnProperty(index)) continue;
            if (event.charCode == keys[index] || event.keyCode == keys[index]) {
                return; //default event
            }
        }
        event.preventDefault();
    };

    public LogError(fileName: string, errorMessage: string) {
        this.errorlogDetails = new ErrorLogDetails();
        this.errorlogDetails.FileName = fileName;
        this.errorlogDetails.ErrorMessage = errorMessage;
        let url = this._serverURL + this._resources.logError;
        return  this._http.post(url, this.errorlogDetails)
           
        //  new Promise((resolve, reject) => {
                // .map(res => res.json())
                // .subscribe((lData) => {
                //     resolve(lData);
                // }, error => reject(error));
        // });
    }

    unAllowednames_usingCustom( control : FormControl) : { [s:string]: boolean} {
        if(control.value != null && control.value != "")
        {
          let s : string = control.value;
          let atleastOneAlpha = s.trim();
          if(atleastOneAlpha != "")
            return null;
          else
            return {'unAllowed' :true};
        }
        else
        return null;
      }

    public getStatusCode(statusId: number) {
        let status: string;
        switch (statusId) {
            case KRAStatusCodes.Approved:
                status = "Approved";
                break;
            case KRAStatusCodes.Draft:
                status = "Drafted";
                break;
            case KRAStatusCodes.SendBackForHRMReview:
                status = "Sent Back For HRA/HRM Review";
                break;
            case KRAStatusCodes.SubmittedForDepartmentHeadReview:
                status = "Submitted For Department Head Review";
                break;
            // case KRAStatusCodes.SendBackForHRHeadReview:
            //     status = "Sent Back For HR Head Review";
            //     break;
            // case StatusCodes.KraSubmittedForHRHeadReview:
            //     status = "Submitted For HR Head Review";
            //     break;
            case KRAStatusCodes.SendBackForHRMReview:
                status = "Sent Back For HRM Review";
                break;
            // case StatusCodes.KraSubmittedForMDApproval:
            //     status = "Submitted For MD Approval";
            //     break;
            default:
                status = "Not available";
                break;
        }
        return status;
    }

    public htmlEncode(stringToEncode: string): string {
        return $('<div/>').text(stringToEncode).html();
    }

    public htmlDecode(stringToDecode: any): string {
        return $('<div/>').html(stringToDecode).text();
    }
    public omit_special_char(event: any) {
        let k: any;
        k = event.charCode; //         k = event.keyCode;  (Both can be used)
        return (
            (k > 64 && k < 91) ||
            (k > 96 && k < 123) ||
            k == 8 ||
            k == 32 ||
            (k >= 48 && k <= 57)
        );
    }

    public IsValidDate(fromDate: any, toDate: any): boolean {
        if (Date.parse(fromDate) <= Date.parse(toDate)) return true;
        return false;
    };
}
