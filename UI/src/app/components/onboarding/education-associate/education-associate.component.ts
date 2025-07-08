import { Component, OnInit, Injector, ViewChild, Inject } from '@angular/core';
import { Qualification } from '../models/education.model';
import * as moment from 'moment';
import { Router, ActivatedRoute, } from '@angular/router';
import { EducationService } from '../services/education.service';
import { CommonService } from '../../../services/common.service';
import { HttpClient } from '@angular/common/http';
import { AppInjector } from '../../shared/injector';
import {MessageService} from 'primeng/api';
import { Associate } from 'src/app/components/onboarding/models/associate.model';
import {ConfirmationService} from 'primeng/api';
import { DropDownType } from '../../../models/dropdowntype.model';

@Component({
  selector: 'app-education-associate',
  templateUrl: './education-associate.component.html',
  styleUrls: ['./education-associate.component.scss'],
  providers: [EducationService, CommonService,MessageService,ConfirmationService]
})
export class EducationComponent implements OnInit {

    id: number;
    qualifications: Array<Qualification>;
    _Associate = new Associate();
    currentempID: number;
    @ViewChild('messageToaster') messageToaster: any;
    @ViewChild('educationDialog') educationDialog: any;
    _resources: any;
    _dataService: Array<Qualification>;
    index: number;
    buttonType: string;
    type: string;
    ddlQualifications: any[];
    ddlGrades: any[];
    valueKey: string = "Qualification";
    valueKey1: string = "GradeType";
    programType = new Array<DropDownType>();
    // programType: any[];
    yearRange: string;
    is10th : boolean = false;
  constructor(@Inject(HttpClient) private _http: HttpClient, private _injector: Injector = AppInjector(), private _dialogservice : ConfirmationService,
      @Inject(EducationService) private _service: EducationService, private _commonService: CommonService,private messageService: MessageService,
      @Inject(Router) private _router: Router, private actRoute: ActivatedRoute){
        this.qualifications = new Array<Qualification>();
        this.qualifications.push({ AcademicCompletedYear: null, EducationalQualification: "", Grade: "", ProgramTypeId: null, Institution: "", Specialization: "", Marks: null }); 
    }

  ngOnInit() {
    this.actRoute.params.subscribe(params => { this.id = params['id']; });
    this.yearRange = (new Date().getFullYear() - 50) + ':' + (new Date().getFullYear() + 0);
    this.currentempID = this.id;
    this.getBusinessValues(this.valueKey);
    this.getGrades(this.valueKey1);
    this.GetQualifications(this.currentempID);
    // this.programType = [];
    // this.programType.push({ label: 'Select ProgramType', value: null });
    this.programType.push({ label: 'Full Time', value: 0 });
    this.programType.push({ label: 'Part Time', value: 1 });
    this.programType.push({ label: 'Distance Education', value: 2 });    
  }
  onlyStrings(event: any) {
    let k: any;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || k == 44 || k == 46);
}
  GetQualifications = function (empId: number) {
    this._service.GetQualifications(empId).subscribe((res: any) => {

        for (var i = 0; i < res.length; i++) {                
            res[i].AcademicCompletedYear = moment(res[i].AcademicCompletedYear).format('YYYY-MM-DD');
        }
        this.qualifications = res;
        if (this.qualifications.length != 0) {
          this.type = "edit";
        }
        else
          this.type = "new";

        if (this.qualifications.length == 0)                
            this.qualifications.push({ AcademicCompletedYear: null, EducationalQualification: "", Grade: "", ProgramTypeId: null, Institution: "", Specialization: "", Marks: null  });
    
    });
 }
 getBusinessValues(valueKey: string) {
    this._commonService.GetBusinessValues(valueKey).subscribe((res: any) => { this.ddlQualifications = res });
 }

 getGrades(valueKey1: string) {
    this._commonService.GetBusinessValues(valueKey1).subscribe((res: any) => {
        this.ddlGrades = res;
    });
}

onNewQualification() {     
    this.qualifications.push({ AcademicCompletedYear: null, EducationalQualification: "", Grade: "", ProgramTypeId: null, Institution: "", Specialization: "", Marks: null });
    return false;
}
OpenConfirmationDialog( ) {   // method to open dialog
    this._dialogservice.confirm({
        message: 'Are you sure, you want to delete this?',
        accept: () => {
        this.Delete();  
        } ,
        reject : ()=>{
            this.onCancel();
        
        }
    });
}

checkqualification(event){
    if(event == "10th"){
        this.is10th = true;
    }
    else if(event != "10th"){
        this.is10th = false;
    }
}
onDelete(index: number) {
     this.OpenConfirmationDialog(); 
   // this.showConfirm();
     this.index = index;
}

// showConfirm() {
//     this.messageService.clear();
//     this.messageService.add({key: 'c', sticky: true, severity:'warn', summary:'Are you sure?', detail:'Confirm to proceed'});
// }
Delete() {
    this.qualifications.splice(this.index, 1);
    for (var i = 0; i < this.qualifications.length; i++) {  
    if(this.qualifications.length == 1){
    if(this.qualifications[i].EducationalQualification == "10th")
    this.is10th = true;
    }
    }
}


onCancel() {
    this.educationDialog.nativeElement.close();
}

IsValidDate = function (fromDate: any, toDate: any) {
    if (Date.parse(fromDate) <= Date.parse(toDate))
        return true;
    return false;
}
OnSubmit() {
    if (this.buttonType == "Save" || this.buttonType == "Update") {
        this.onSaveorUpdate(this.qualifications);
        return true;
    }
    else if (this.buttonType == "NewQualification")
        return this.onNewQualification();
}
OnUpdate() {
    this.buttonType = "Update";
}

OnSave() {
    this.buttonType = "Save";
}
setonNewQualification() {
    this.buttonType = "NewQualification";
}


onSaveorUpdate(qual: Array<Qualification>) {
    var today: any = new Date();
    for (var i = 0; i < qual.length; i++) {  

        if(qual[i].EducationalQualification == "10th"){
            if ((qual[i].EducationalQualification.trim().length == 0 || !qual[i].EducationalQualification) ||!qual[i].Grade || !qual[i].Institution || !qual[i].Marks || !qual[i].AcademicCompletedYear) {
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete qualification details'});
                return false;
            }
        }
        
         if(qual[i].EducationalQualification != "10th"){
            if ((qual[i].EducationalQualification.trim().length == 0 || !qual[i].EducationalQualification) ||!qual[i].Grade || !qual[i].Institution || !qual[i].Specialization || !qual[i].Marks || !qual[i].AcademicCompletedYear) {
                this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete education details'});
                return false;
            }
        }
        
        if (qual[i].Grade == "Percentage" && qual[i].Marks > 100) {
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Percentage should not be greater than 100'});
            return false;
        }
        if ((qual[i].Grade == "CPI" || qual[i].Grade == "GPI") && qual[i].Marks > 10) {
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'CPI/GPA should not be greater than 10'});
            return false;
        }                     

        if(this.IsValidDate(today,qual[i].AcademicCompletedYear) )
        {           
            this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Completed Year should not be greater than or equal to todays date'});
            return false;
        }           
       
        qual[i].AcademicCompletedYear = qual[i].AcademicCompletedYear;
    }        
   
    var IsDuplicate = 0;
    var duplicate = false;
    for (var i = 0; i < qual.length; i++) {
        if (!duplicate) {
            IsDuplicate = 0;
            for (var q = 0; q < qual.length; q++) {
                if (qual[i].EducationalQualification == qual[q].EducationalQualification && qual[i].EducationalQualification != "Post Graduation" && qual[i].EducationalQualification != "Graduation") {
                    IsDuplicate++;
                    if (IsDuplicate > 1) {
                        duplicate = true;
                        break;
                    }
                }
            }
        }
    }
    if (IsDuplicate > 1) {
        this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Same qualification is selected multiple times'});

        return false;
    }
    else {
        this._Associate.Qualifications = [];
        for (var i = 0; i < qual.length; i++) {
            this._Associate.Qualifications.push(qual[i]);
        }
        this._Associate.EmpId = this.currentempID;
        this._service.SaveEducationDetails(this._Associate).subscribe((data) => {
            this.messageService.add({severity:'success', summary: 'Success Message', detail:'Qualifications saved successfully'});
            this.GetQualifications(this.currentempID);
        }, (error) => {
            if (error._body != undefined && error._body != "")
            this.messageService.add({severity:'error', summary: 'Failure Message', detail:''});
            else
            this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed to save qualifications'});
        });

    }
    this.is10th = false;
}

onlyNumbers(event: any) {
    this._commonService.onlyNumbers(event);
}
formateDate(givenDate?: string): string {
    let formatedDate: string = "";
    if (givenDate != '' && typeof (givenDate) != 'undefined' && givenDate != null) {
        formatedDate = givenDate.split("T")[0];
    }
    return formatedDate;
}

taost(msg: string) {
    this.messageToaster.nativeElement.text = msg;
    this.messageToaster.nativeElement.open();
}
}

