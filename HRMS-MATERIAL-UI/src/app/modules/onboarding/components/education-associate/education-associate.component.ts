import { Component, OnInit, Injector, ViewChild, Inject, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { Qualification } from '../../models/education.model';
import * as moment from 'moment';
import { Router, ActivatedRoute, } from '@angular/router';
import { EducationService } from '../../services/education.service';
import { CommonService } from '../../../../core/services/common.service';
import { HttpClient } from '@angular/common/http';
import { AppInjector } from '../../../shared/injector';
import { Associate } from '../../models/associate.model';
import { DropDownType } from '../../../master-layout/models/dropdowntype.model';
import { MatTableDataSource } from '@angular/material/table';
import { themeconfig } from '../../../../../themeconfig';
import { FormGroup, FormControl, Validators, FormArray, FormBuilder, FormGroupDirective } from '@angular/forms';
import { ProgramType } from '../../../master-layout/utility/enums';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { getMultipleValuesInSingleSelectionError } from '@angular/cdk/collections';
import { MatTable } from '@angular/material/table';
import {
    MatSnackBar,
    MatSnackBarHorizontalPosition,
    MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import { CommonDialogComponent } from '../../../shared/components/common-dialog/common-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MessageDialogComponent } from 'src/app/modules/project-life-cycle/components/message-dialog/message-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
    selector: 'app-education-associate',
    templateUrl: './education-associate.component.html',
    styleUrls: ['./education-associate.component.scss']
})
export class EducationAssociateComponent implements OnInit {
    added = false;
    dialogResponse: boolean;
    id: number;
    qualifications: Array<Qualification>;
    educationAssociate: FormGroup;
    EducationalQualification = 'hello';
    themeAppearence = themeconfig.formfieldappearances;
    _Associate = new Associate();
    currentempID: number;
    // @ViewChild('messageToaster') messageToaster: any;
    @ViewChild('educationDialog') educationDialog: any;
    @ViewChild(MatTable) table: MatTable<any>;
    horizontalPosition: MatSnackBarHorizontalPosition = 'right';
    verticalPosition: MatSnackBarVerticalPosition = 'top';
    _resources: any;
    _dataService: Array<Qualification>;
    index: number;
    newCreateForm: FormGroup;
    buttonType: string;
    type: string;
    ddlQualifications: any[];
    ddlGrades: any[];
    valueKey: string = 'Qualification';
    valueKey1: string = 'GradeType';
    programType = new Array<DropDownType>();
    // programType: any[];
    yearRange: string;
    is10th: boolean = false;
    dataSource: MatTableDataSource<Qualification>;
    arr = [];
    EducationArray: FormArray;
    EducationForm: FormGroup;
    @ViewChild('FormGroupDirective') formGroupDirective: FormGroupDirective;
    // tslint:disable-next-line:max-line-length
    displayedColumns: string[] = ['EducationalQualification', 'ProgramTypeId', 'Specialization', 'AcademicCompletedYear', 'Institution', 'Grade', 'Marks', 'Action', 'Delete'];
    isEduFormSubmitted: boolean = false;
    // tslint:disable-next-line:variable-name
    // tslint:disable-next-line:max-line-length
    constructor(public dialog: MatDialog, public fb: FormBuilder, private snackBar: MatSnackBar, @Inject(HttpClient) private _http: HttpClient, private _injector: Injector = AppInjector(),
        // tslint:disable-next-line:variable-name
        // tslint:disable-next-line:max-line-length
        @Inject(EducationService) private _service: EducationService, private _commonService: CommonService,
        @Inject(Router) private _router: Router, private actRoute: ActivatedRoute, private changeDetectorRefs: ChangeDetectorRef,private spinner: NgxSpinnerService) {
        this.qualifications = new Array<Qualification>();

    }

    ngOnInit() {
        this.spinner.show()
        this.actRoute.params.subscribe(params => { this.id = params.id; });
        // this.id = 221;
        this.currentempID = this.id;
        this.getBusinessValues(this.valueKey);
        this.getGrades(this.valueKey1);
        this.GetQualifications(this.currentempID);
        this.programType.push({ label: 'Full Time', value: 0 });
        this.programType.push({ label: 'Part Time', value: 1 });
        this.programType.push({ label: 'Distance Education', value: 2 });


    }
    creatForm() {
        this.educationAssociate = new FormGroup({
            EducationArray: new FormArray([])
        });
    }
    createFormGroup() {
        this.EducationArray = new FormArray([]);
        this.qualifications.forEach(element => {
            this.EducationArray = this.educationAssociate.get('EducationArray') as FormArray;
            this.EducationArray.push(this.BuildForm(element));
        });
    }
    BuildForm(element): any {
        if (element.EducationalQualification == "10th") {
            this.is10th = true;
            return new FormGroup({
                EducationalQualification: new FormControl(element.EducationalQualification, [Validators.required]),
                ProgramTypeId: new FormControl(element.ProgramTypeId, [Validators.required]),
                Specialization: new FormControl(element.Specialization, [Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
                AcademicCompletedYear: new FormControl(element.AcademicCompletedYear, [Validators.required]),
                Institution: new FormControl(element.Institution, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
                Grade: new FormControl(element.Grade, [Validators.required]),
                Marks: new FormControl(element.Marks, [Validators.required, Validators.pattern('^[0-9.]*$')])
            });
        }
        else {
            this.is10th = false;
            return new FormGroup({
                EducationalQualification: new FormControl(element.EducationalQualification, [Validators.required]),
                ProgramTypeId: new FormControl(element.ProgramTypeId, [Validators.required]),
                Specialization: new FormControl(element.Specialization, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
                AcademicCompletedYear: new FormControl(element.AcademicCompletedYear, [Validators.required]),
                Institution: new FormControl(element.Institution, [Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)]),
                Grade: new FormControl(element.Grade, [Validators.required]),
                Marks: new FormControl(element.Marks, [Validators.required, Validators.pattern('^[0-9.]*$')])
            });
        }
    }
    get testArray(): FormArray {
        return this.educationAssociate.get('EducationArray') as FormArray;
    }
    onlyStrings(event: any) {
        let k: any;
        k = event.target.value;
        return k;
    }


    setValueQualification(i: any) {
        this.educationAssociate.get('EducationalQualification').patchValue(this.qualifications[i].EducationalQualification);

    }
    setValueSpecialization(i: any) {
        console.log(this.is10th)
    }
    setValueProgramType(i: any) {
        this.educationAssociate.get('ProgramTypeId').patchValue(this.qualifications[i].ProgramTypeId);
    }
    setValueCompletedYear(i: any) {
        this.educationAssociate.get('AcademicCompletedYear').patchValue(this.qualifications[i].AcademicCompletedYear);
    }
    setValueInstitute(i: any) {
        this.educationAssociate.get('Institution').patchValue(this.qualifications[i].Institution);
    }
    setValueGrade(i: any) {
        this.educationAssociate.get('Grade').patchValue(this.qualifications[i].Grade);
    }
    setValueMarks(i: any) {
        this.educationAssociate.get('Marks').patchValue(this.qualifications[i].Marks);
    }
    OnDateChange(event: any) {
    }
    GetQualifications = function (empId: number): any {
        this._service.GetQualifications(empId).subscribe((res: any) => {
            this.spinner.hide()
            for (let i = 0; i < res.length; i++) {
                res[i].AcademicCompletedYear = moment(res[i].AcademicCompletedYear).format('YYYY-MM-DD');
            }

            this.qualifications = res;
            if (this.qualifications.length !== 0) {
                this.type = 'edit';
            } else {
                this.type = 'new';
            }

            if (this.qualifications.length === 0) {
                // tslint:disable-next-line:max-line-length
                this.qualifications.push({ AcademicCompletedYear: null, EducationalQualification: '', Grade: '', ProgramTypeId: null, Institution: '', Specialization: '', Marks: null });
            }
            this.creatForm();
            this.createFormGroup();

        },(error)=>{
            this.spinner.hide()
        });
    };
    getBusinessValues(valueKey: string) {

        this._commonService.GetBusinessValues(valueKey).subscribe((res: any) => { this.ddlQualifications = res });
    }

    getGrades(valueKey1: string) {

        this._commonService.GetBusinessValues(valueKey1).subscribe((res: any) => {
            this.ddlGrades = res;
        });
    }

    onNewQualification() {
        this.added = true;
        if (this.educationAssociate.controls.EducationArray.valid) {
            // tslint:disable-next-line:max-line-length
            this.qualifications.push({ AcademicCompletedYear: null, EducationalQualification: null, Grade: null, ProgramTypeId: null, Institution: null, Specialization: null, Marks: null });
            this.EducationArray = this.educationAssociate.get('EducationArray') as FormArray;
            // tslint:disable-next-line:max-line-length
            this.EducationArray.push(this.newCreateForm = this.BuildForm({ AcademicCompletedYear: null, EducationalQualification: null, Grade: null, ProgramTypeId: null, Institution: null, Specialization: null, Marks: null }));
            this.dynamicEducationValidations('AcademicCompletedYear', this.newCreateForm);
            this.dynamicEducationValidations('EducationalQualification', this.newCreateForm);
            this.dynamicEducationValidations('Grade', this.newCreateForm);
            this.dynamicEducationValidations('ProgramTypeId', this.newCreateForm);
            this.dynamicEducationValidations('Institution', this.newCreateForm);
            this.dynamicEducationValidations('Specialization', this.newCreateForm);
            this.dynamicEducationValidations('Marks', this.newCreateForm);
            this.table.renderRows();
            return false;
        }
        else {
            return false;
        }

    }

    dynamicEducationValidations(eduControl: string, newFormEduValidate: FormGroup) {
        newFormEduValidate.controls[eduControl].setErrors(null);
        newFormEduValidate.controls[eduControl].clearValidators();
        newFormEduValidate.controls[eduControl].updateValueAndValidity();
    }

    OpenConfirmationDialog() {   // method to open dialog

        this.Delete();
    }

    checkqualification(event, i) {
        if (event === '10th')
            this.setSpecializationValidators('10th', i);
        else
            this.setSpecializationValidators(event, i);
    }
    setSpecializationValidators(qual: any, i: number) {
        this.is10th = false;
        // tslint:disable-next-line:max-line-length
        this.EducationArray.controls[i].get('Specialization').setValidators([Validators.required, Validators.pattern('^[a-zA-Z ]*$')]);
        if (qual === '10th') {
            this.is10th = true;
            this.EducationArray.controls[i].get('Specialization').setValidators([Validators.pattern('^[a-zA-Z ]*$')]);
        }
        this.EducationArray.controls[i].get('Specialization').updateValueAndValidity();
    }
    onDelete(index: number) {
        this.index = index;
        this.openDialog('Confirmation', 'Do you want to Delete ?', index);
        this.index = index;
    }

    clearInput(evt: any, fieldName, i): void {
        if (fieldName == 'AcademicCompletedYear') {
            evt.stopPropagation();
            this.testArray.controls[i].get('AcademicCompletedYear').reset();
        }
    }

    Delete() {
        this.qualifications.splice(this.index, 1);
        // tslint:disable-next-line:no-var-keyword
        // tslint:disable-next-line:prefer-for-of
        for (let i = 0; i < this.qualifications.length; i++) {
            if (this.qualifications.length === 1) {
                if (this.qualifications[i].EducationalQualification === '10th') {
                    this.is10th = true;
                }
            }
        }
        this.EducationArray.removeAt(this.index);
        this.table.renderRows();
    }

    openDialog(Heading, Message, index): void {
        const dialogRef = this.dialog.open(CommonDialogComponent, {
            disableClose: true,
            hasBackdrop: true,
            width: '300px',
            data: { heading: Heading, message: Message }
        });

        dialogRef.afterClosed().subscribe(result => {
            this.dialogResponse = result;
            if (this.dialogResponse == true) {
                this.Delete();
                const dialogConf = this.dialog.open(MessageDialogComponent, {
                    disableClose: true,
                    hasBackdrop: true,
                    width: '300px',
                    data: { heading: 'Confirmation', message: 'Education Details deleted successfully' }
                })
            }
        });
    }


    onCancel() {
        this.educationDialog.nativeElement.close();
    }

    // tslint:disable-next-line:only-arrow-functions
    IsValidDate = function (fromDate: any, toDate: any) {
        if (Date.parse(fromDate) <= Date.parse(toDate)) {
            return true;
        }
        return false;
    }
    OnSubmit(qualification: Qualification[]) {
        if (this.type === 'new') {
            this.buttonType = 'Save';
        }
        else {
            this.buttonType = 'Update';
        }
        if (this.buttonType === 'Save' || this.buttonType === 'Update') {
            return this.onSaveorUpdate(this.qualifications);
        }

    }
    OnUpdate() {
        this.buttonType = 'Update';
    }

    OnSave() {
        this.buttonType = 'Save';
    }
    setonNewQualification() {
        this.buttonType = 'NewQualification';
    }

    setEducationRequiredValidations(eduFormControl: string, newEduDetails: FormGroup) {
        newEduDetails.controls[eduFormControl].setValidators([Validators.required])
        newEduDetails.controls[eduFormControl].updateValueAndValidity();
        if (eduFormControl === 'Marks')
            this.isEduFormSubmitted = false
        else if (eduFormControl === 'Institution') {
            newEduDetails.controls[eduFormControl].setValidators([Validators.required, Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100)])
            newEduDetails.controls[eduFormControl].updateValueAndValidity();
        }
        else if (eduFormControl === 'Marks') {
            newEduDetails.controls[eduFormControl].setValidators([Validators.required, Validators.pattern('^[0-9.]*$')])
            newEduDetails.controls[eduFormControl].updateValueAndValidity();
        }
        else if (eduFormControl == ''){
            newEduDetails.controls[eduFormControl].setValidators([Validators.pattern('^[a-zA-Z ]*$'), Validators.maxLength(100), Validators.required])
            newEduDetails.controls[eduFormControl].updateValueAndValidity();
        }
    }

    onSaveorUpdate(qual: Array<Qualification>) {
        if (this.type === 'new') {
            this.buttonType = 'Save';
        }
        else {
            this.buttonType = 'Update';
        }
        if (this.newCreateForm !== undefined) {
            this.isEduFormSubmitted = true;
            this.setEducationRequiredValidations('AcademicCompletedYear', this.newCreateForm);
            this.setEducationRequiredValidations('EducationalQualification', this.newCreateForm);
            this.setEducationRequiredValidations('Grade', this.newCreateForm);
            this.setEducationRequiredValidations('Specialization', this.newCreateForm);
            this.setEducationRequiredValidations('ProgramTypeId', this.newCreateForm);
            this.setEducationRequiredValidations('Institution', this.newCreateForm);
            this.setEducationRequiredValidations('Marks', this.newCreateForm);
        }
        let today: any = new Date();
        // tslint:disable-next-line:prefer-for-of
        for (let i = 0; i < qual.length; i++) {

            if (qual[i].EducationalQualification === '10th') {
                // tslint:disable-next-line:max-line-length
                if ((qual[i].EducationalQualification.length === 0 || !qual[i].EducationalQualification) || !qual[i].Grade || !qual[i].Institution || !qual[i].Marks || !qual[i].AcademicCompletedYear) {
                    return true;
                }

            }

            if (qual[i].EducationalQualification !== '10th') {
                // tslint:disable-next-line:max-line-length
                if ((!qual[i].EducationalQualification) || !qual[i].Grade || !qual[i].Institution || !qual[i].Specialization || !qual[i].Marks || !qual[i].AcademicCompletedYear) {
                    // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Please complete education details'});
                    return true;
                }
            }

            if (qual[i].Grade === 'Percentage') {
                // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Percentage should not be greater than 100'});
                if (qual[i].Marks > 100) {
                    this.snackBar.open('Percentage should not be greater than 100', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                    return true;
                }
                else if (qual[i].Marks == 0) {
                    this.snackBar.open('Percentage should not be zero', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                    return true;
                }
            }
            if ((qual[i].Grade == 'CPI' || qual[i].Grade == 'GPI')) {
                //  this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'CPI/GPA should not be greater than 10'});
                if (qual[i].Marks > 10) {
                    this.snackBar.open('CPI/GPA should not be greater than 10', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                    return true;
                }
                else if (qual[i].Marks == 0) {
                    this.snackBar.open('CPI/GPA should not be zero', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                    return true;
                }
            }

            if (this.IsValidDate(today, qual[i].AcademicCompletedYear)) {
                // tslint:disable-next-line:max-line-length
                // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Completed Year should not be greater than or equal to todays date'});
                this.snackBar.open('Completed Year should not be greater than or equal to todays date', 'x', {
                    duration: 1000,
                    horizontalPosition: this.horizontalPosition,
                    verticalPosition: this.verticalPosition,
                });
                return true;
            }

            qual[i].AcademicCompletedYear = qual[i].AcademicCompletedYear;
        }

        let IsDuplicate = 0;
        let duplicate = false;
        // tslint:disable-next-line:prefer-for-of
        for (let i = 0; i < qual.length; i++) {
            if (!duplicate) {
                IsDuplicate = 0;
                // tslint:disable-next-line:prefer-for-of
                for (let q = 0; q < qual.length; q++) {
                    if (qual[i].EducationalQualification === qual[q].EducationalQualification && qual[i].EducationalQualification != 'Post Graduation' && qual[i].EducationalQualification != 'Graduation') {
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
            // this.messageService.add({severity:'warn', summary: 'Warning Message', detail:'Same qualification is selected multiple times'});
            this.snackBar.open('Same qualification is selected multiple times', 'x', {
                duration: 1000,
                horizontalPosition: this.horizontalPosition,
                verticalPosition: this.verticalPosition,
            });
            return true;
        }
        else {
            this._Associate.Qualifications = [];
            // tslint:disable-next-line:prefer-for-of
            for (let i = 0; i < qual.length; i++) {
                this._Associate.Qualifications.push(qual[i]);
            }
            this._Associate.EmpId = this.currentempID;
            this._service.SaveEducationDetails(this._Associate).subscribe((data) => {
                // this.messageService.add({severity:'success', summary: 'Success Message', detail:'Qualifications saved successfully'});
                if (this.buttonType == 'Save') {
                    this.snackBar.open('Qualifications saved successfully', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                }
                else {
                    this.snackBar.open('Qualifications updated successfully', 'x', {
                        duration: 1000,
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                }
                // this.EducationArray.reset();
                this.GetQualifications(this.currentempID);
            }, (error) => {
                if (error._body !== undefined && error._body !== '') {
                    //  this.messageService.add({severity:'error', summary: 'Failure Message', detail:''});
                }
                else {
                    this.snackBar.open('Failed to save qualifications', 'x', {
                        duration: 1000,
                        panelClass:['error-alert'],
                        horizontalPosition: this.horizontalPosition,
                        verticalPosition: this.verticalPosition,
                    });
                    //  this.messageService.add({severity:'error', summary: 'Failure Message', detail:'Failed to save qualifications'});
                }
            });
            this.is10th = false;
            return false;
        }
    }

    onlyNumbers(event: any) {
        this._commonService.onlyNumbers(event);
    }
    formateDate(givenDate?: string): string {
        let formatedDate = '';
        if (givenDate !== '' && typeof (givenDate) !== 'undefined' && givenDate != null) {
            formatedDate = givenDate.split('T')[0];
        }
        return formatedDate;
    }

}
