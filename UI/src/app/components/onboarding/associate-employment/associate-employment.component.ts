import { Component, OnInit } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Qualification } from '../models/education.model';
import { Associate } from '../models/associate.model';
import { Inject } from '@angular/core';
import { Http } from '@angular/http';
import { EducationService } from '../services/education.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { Injector } from '@angular/core';
import { AppInjector } from '../../shared/injector';
import * as moment from 'moment';
import { CommonService } from '../../../services/common.service';
import { ProfessionalReferences, EmploymentDetails } from '../models/employmentdetails.model';
import { EmploymentService } from '../services/employment.service';
import {MessageService} from 'primeng/api';
import { ConfirmationService } from 'primeng/components/common/confirmationservice';


@Component({
  selector: 'app-associate-employment',
  templateUrl: './associate-employment.component.html',
  styleUrls: ['./associate-employment.component.scss'],
  providers: [MessageService, ConfirmationService]
})
export class AssociateEmploymentComponent implements OnInit {
    id: number;
    _Associate = new Associate();
    currentempID: number;
    @ViewChild('messageToaster') messageToaster: any;
    @ViewChild('PrevEmploymentDialog') PrevEmploymentDialog: any;
    @ViewChild('profDialog') profDialog: any;
    private _serverURL: string;
    type: string = 'new';
    lastDate: Date;
    index: number;
    yearRange: string;
    emp:any;
    constructor( private _injector: Injector = AppInjector(),
    private _dialogservice : ConfirmationService,
        @Inject(EmploymentService) private _service: EmploymentService,
        private messageService: MessageService,
        @Inject(Router) private _router: Router, private actRoute: ActivatedRoute) {
        this._Associate.PrevEmploymentDetails = new Array<EmploymentDetails>();
        this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: "", Designation: "", Address: "", LeavingReason: "" });
        this._Associate.ProfReferences = new Array<ProfessionalReferences>();
        this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: "", MobileNo: "", CompanyAddress: "", OfficeEmailAddress: "" });
    }

    ngOnInit() {
        this.actRoute.params.subscribe(params => { this.id = params['id']; });
        this.currentempID = this.id;
        this.yearRange  = (new Date().getFullYear() - 50) + ':' + (new Date().getFullYear() + 0);
        this.getEmploymentdetails(this.currentempID);
        this.getDates();
    }

    forAddress(event: any){
        let k: any;  
        k = event.charCode;  //         k = event.keyCode;  (Both can be used)
        return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 92 ||  k == 35 || (k >= 44 && k <= 57));
    }

    omit_special_char(event: any)
    {   
       let k: any;  
       k = event.charCode;  //         k = event.keyCode;  (Both can be used)
       return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 ||  k == 46 || k == 44 || (k >= 48 && k <= 57));
    }

    onlychar(event: any)
    {   
       let k: any;  
       k = event.charCode;  //         k = event.keyCode;  (Both can be used)
       return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32);
    }

    onNewEmployment(empdetails: EmploymentDetails) {
        this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: "", Designation: "", Address: "", LeavingReason: "" });
    }

    onDeleteEmployment(index: number) {
        this.index = index;
        this.OpenConfirmationDialog("employement"); 
        
    }

    DeleteEmployment() {
        this._Associate.PrevEmploymentDetails.splice(this.index, 1);
        this.PrevEmploymentDialog.nativeElement.close();
    }
    OpenConfirmationDialog(type:string) {   // method to open dialog
        this._dialogservice.confirm({
            message: 'Are you sure, you want to delete this?',
            accept: () => {
            if(type=="employement")
            this.DeleteEmployment();  
            else
            this.Delete();
            } ,
            reject : ()=>{
                this.Cancel();
            
            }
        });
    }

    Cancel() {
        this.PrevEmploymentDialog.nativeElement.close();
    }
    onNewProfRef(profdetails: ProfessionalReferences) {
        this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: "", MobileNo: "", CompanyAddress: "", OfficeEmailAddress: "" });
    }

    onDeleteProfessionalRef(index: number) {
        // this.profDialog.nativeElement.open();
         this.index = index;
        this.OpenConfirmationDialog("Professinal"); 
        
    }

    Delete() {
        this._Associate.ProfReferences.splice(this.index, 1);
        this.profDialog.nativeElement.close();
    }

    onCancel() {
        this.profDialog.nativeElement.close();
    }

    getDates() {
        var date = new Date(), y = date.getFullYear(), m = date.getMonth(), d = date.getDate();
        this.lastDate = new Date(y, m, d);
    }

    validate(toYear: Date, fromYear: Date): number  {
        let count = 0;
        if(toYear && fromYear) {
        this._Associate.PrevEmploymentDetails.forEach((details: any) => {
           if (details.ServiceTo == toYear && details.ServiceFrom == fromYear)
                count++;
        });
    }
        if (count == 2) {
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'You cant select same service duration'});
        }
        return count;
    }

    getEmploymentdetails = function (empID: number) {
        this._Associate.empID=empID;
        this._service.GetEmploymentDetails(this.id).subscribe((res: any) => {
            for (var i = 0; i < res.length; i++) {
                res[i].ServiceFrom =  moment(res[i].ServiceFrom).format('YYYY-MM-DD');
                res[i].ServiceTo =  moment(res[i].ServiceTo).format('YYYY-MM-DD');
            }
            this._Associate.PrevEmploymentDetails = res;

            if (this._Associate.PrevEmploymentDetails.length == 0)
                this._Associate.PrevEmploymentDetails.push({ ServiceFrom: null, ServiceTo: null, Name: "", Designation: "", Address: "", LeavingReason: "" });
        });
        this._service.GetProfReferenceDetails(this.id).subscribe((res: any) => {
            this._Associate.ProfReferences = res;
            if (this._Associate.ProfReferences.length != 0) {
                this.type = "edit";
            }
            if (this._Associate.ProfReferences.length == 0)
                this._Associate.ProfReferences.push({ Name: null, CompanyName: null, Designation: "", MobileNo: "", CompanyAddress: "", OfficeEmailAddress: "" });
        });
    };


    IsValidDate = function (fromDate: any, toDate: any) {
        if (Date.parse(fromDate) <= Date.parse(toDate))
            return true;
        return false;
    }

    ValidatePrevEmployerdetails = function () {
        var prevEmployerdetails = this._Associate.PrevEmploymentDetails;
        if (prevEmployerdetails.length == 1 && !prevEmployerdetails[0].Name)
            return 3;
        for (var count = 0; count < prevEmployerdetails.length; count++) {

            if (!prevEmployerdetails[count].Name || prevEmployerdetails[count].Name.trim().length ==0 
            || !prevEmployerdetails[count].Address || prevEmployerdetails[count].Address.trim().length ==0 
            || !prevEmployerdetails[count].Designation || prevEmployerdetails[count].Designation.trim().length ==0 
            || !prevEmployerdetails[count].ServiceFrom 
            || !prevEmployerdetails[count].ServiceTo) {

                if (count == (prevEmployerdetails.length - 1) && !prevEmployerdetails[count].Name) {
                    return 3;
                }

                return 3;
            }
            if (!this.IsValidDate(prevEmployerdetails[count].ServiceFrom, prevEmployerdetails[count].ServiceTo)) {
                return 2;
            }
        };
        return 1;
    }

    ValidateProfReference = function () {
        var profReference = this._Associate.ProfReferences;
        if (profReference.length == 1 && !profReference[0].Name)
            return false;
        for (var count = 0; count < profReference.length; count++) {
            if (!profReference[count].Name || profReference[count].Name.trim().length == 0
                || !profReference[count].Designation || profReference[count].Name.trim().Designation == 0
                || !profReference[count].CompanyName || profReference[count].Name.trim().CompanyName == 0
                || !profReference[count].CompanyAddress || profReference[count].Name.trim().CompanyAddress == 0
                || !profReference[count].OfficeEmailAddress || profReference[count].Name.trim().OfficeEmailAddress == 0
                || !profReference[count].MobileNo) {
                if (count == (profReference.length - 1) && !profReference[count].Name) {
                    return false;
                }
                return false;
            }
        };
        return true;
    }

    
    
    onSaveorUpdate(emp: Associate) {
        var today: any = new Date();
        today = today.getFullYear() + "/" + (today.getMonth() + 1) + "/" + today.getDate();
        var IsValidPrevEmployerdetail = this.ValidatePrevEmployerdetails();

        if (IsValidPrevEmployerdetail == 2) {
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Invalid service duration'});
            return;
        }

        if (IsValidPrevEmployerdetail == 3) {
           
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete Employer Reference details'});
            return;
        }

        if (!this.ValidateProfReference()) {
            
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete Professional Reference details'});
            return;
        }

        var profReference = this._Associate.ProfReferences;
        for (var count = 0; count < profReference.length; count++) {
            if (!this.validateEmail(profReference[count].OfficeEmailAddress)) {
                
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please enter valid email address. For ex:- abc@xyz.com'});
                return;
            }
            if(profReference[count].MobileNo.length < 10){
               
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please enter valid contact number.'});
                return;
            }
        }

        var prevEmployerdetails = this._Associate.PrevEmploymentDetails;
        for (var count = 0; count < prevEmployerdetails.length; count++) {
            if (!this.IsValidDate(prevEmployerdetails[count].ServiceFrom, today)) {
               
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'From date should not be greater than today date.'});
                return;
            }
            if (!this.IsValidDate(prevEmployerdetails[count].ServiceTo , today)) {
               
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'To date should not be greater than or equal to today date.'});
                return;
            }           
        }

        this._Associate.EmpId = this.currentempID;
        this._service.SaveEmploymentDetails(this._Associate).subscribe((data) => {
           
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Employment details saved successfully.'});
            this.getEmploymentdetails(this.currentempID);
        }, (error) => {
            this.messageService.add({severity:'error', summary: 'Error Message', detail:'Failed to save employment details!'});
        });
    }

    onlyForNumbers(event: any){
        var keys = {
            'escape': 27, 'backspace': 8, 'tab': 9, 'enter': 13, 
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

    validateEmail(email: any) {
        var filter = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
        if (filter.test(email)) {
            return true;
        }
        else {
            return false;
        }
    }

    onlyStrings(event: any)
    {   
       let k: any;  
       k = event.charCode;  //         k = event.keyCode;  (Both can be used)
       return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 44 || k == 46);
    }

}



