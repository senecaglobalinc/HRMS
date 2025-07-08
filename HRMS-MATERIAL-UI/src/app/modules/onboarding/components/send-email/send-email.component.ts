import { Component, ContentChildren, ElementRef, Inject, OnInit, QueryList, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { title } from 'process';
import { themeconfig } from 'src/themeconfig';
import { SendMailModel } from '../../models/sendmail.model';
import { AssociateInformationService } from '../../services/associateInformation.service';
@Component({
  selector: 'app-send-email',
  templateUrl: './send-email.component.html',
  styleUrls: ['./send-email.component.scss']
})
export class SendEmailComponent implements OnInit {
  element:any;
  image:string;
  formData:FormData;
  uploadForm:any;
  imageView:any;
  isEditClicked:boolean = false;
  isEmpNameEdit:boolean = false;
  isDesignationEdit:boolean = false;
  isHighQualificationEdit:boolean=false;
  isInstitutionEdit:boolean=false;
  isExperienceEdit:boolean=false;
  isPrevEmpDesignationEdit:boolean=false;
  isSkillNameEdit:boolean=false;
  isDepartmentEdit:boolean=false;
  isPrevEmpDetailsEdit:boolean=false;
  isPronoun:string;
  isPossessivePronoun:string;
  isJoiningDateEdit:boolean=false;
  joiningDate:any;
  saluation: string;
  empfirstName: string;
  emplastName: string;
  skillLists:string[]=[];
  certificationList=[];
  bioContent:string;
  myTable:string;
  emptyValue:string=' ';
  isReportingEditClicked:boolean = false;
  reportingManagerName:string;
  @ViewChild('emailcontent') content;
  @ViewChild('emailsubject') subject;
  @ViewChild('sendmailcontent') sendmailcontent;
  @ViewChild('joinDate') joinDate;
  welcomeMessage:string='Welcome to Seneca Global';
  themeConfigInput = themeconfig.formfieldappearances;
  emailStr: string;
    fileToUpload: File;
    files: any;
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<SendEmailComponent>,
    private associateInformationService:AssociateInformationService,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { 
    if (data.element) {
      this.element = this.data.element;
      this.isPronoun = this.element.Gender==='Male'?'He':'She';
      this.saluation = this.element.Gender==='Male'?'Mr.':'Ms.';
      const nameSplit:number=this.element.EmpName.lastIndexOf(' ');
      this.empfirstName = this.element.EmpName.slice(0,nameSplit);
      this.emplastName = this.element.EmpName.slice(nameSplit+1,this.element.EmpName.length);
      const qualificationObj:any={'CertificationName':'','Specialization':''};
      qualificationObj.CertificationName=this.element.HighestQualification;
      qualificationObj.Specialization=this.element.Institution;
      this.certificationList.push(qualificationObj);
      this.element.SkillName.map((element)=>{
        this.skillLists.push(element.Skillname)
      })
      
      this.element.CertificationList.map((element)=>{
        this.certificationList.push(element);
      })
      this.myTable = "";
      for(var i=0;i<this.certificationList.length;i++){
        const td='<tr><td align="left"' +
        'style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:18px;text-align:left;color:#1a1a1a;font-family:Montserrat,Arial;line-height:22px">'+
        '<strong>'+this.certificationList[i].CertificationName+'</strong></span></p>'+
        '<p style="text-align:left;margin:10px 0;margin-top:10px"><span style="font-size:13px;text-align:left;color:#5c5c5c;font-family:Montserrat,Arial;line-height:5px">'+this.certificationList[i].Specialization+'</span></p></div></td></tr>';
        this.myTable=this.myTable+td;
      }

      this.isPossessivePronoun = this.element.Gender==='Male'?'him':'her';
      //this.joinDate = new Date(this.element.JoiningDate);
      const joiningDate = this.element.JoiningDate.split('-');
      this.joiningDate = joiningDate[1]+'/'+joiningDate[0]+'/'+joiningDate[2];

    }
  }


  ngOnInit(): void {
    this.uploadForm = this.fb.group({
      avatar: ['',Validators.required]
    })
  }

  onFileChange(event) {

    if (event.target.files.length > 0) {

      this.files = event.target.files[0];
       if(this.files.type==='image/png'||this.files.type==='image/jpg'
       ||this.files.type==='image/jpeg'){
        this.uploadForm.get('avatar').setValue(this.files);
        if (event.target.files && event.target.files[0]) {
          var reader = new FileReader();
  
          reader.readAsDataURL(event.target.files[0]); // read file as data url
  
          reader.onload = (event) => { // called once readAsDataURL is completed
            this.imageView = event.target.result;
          }
        }
       }else{
        this._snackBar.open('Please upload image(jpg/png/jpeg) files only.'), "x", {
          duration: 5000,
          horizontalPosition: "right",
          panelClass: ['error-alert'],
          verticalPosition: "top",
        }
       }
     

    }
  //   this._snackBar.open(error.error), "", {
  //     duration: 1000,
  //     panelClass:['error-alert'],
  //     horizontalPosition: "right",
  //     verticalPosition: "top",
  //   }
  // });

  }

  cancel(){
    this.dialogRef.close();
  }
  onUploadBanner(e) {
    this.fileToUpload = e.target.files[0];
}

onSelectFile(event) {
  this.fileToUpload = event.target.files[0];
  this.formData = new FormData();
  this.formData.append('fileName',this.fileToUpload.name);
  this.formData.append('fileType',this.fileToUpload.type);
  this.formData.forEach(function(index,value){
    console.log(index +' '+value);
});
  if (event.target.files && event.target.files[0]) {
    var reader = new FileReader();

    reader.readAsDataURL(event.target.files[0]); // read file as data url

    reader.onload = (event) => { // called once readAsDataURL is completed
      this.imageView = event.target.result;
    }
  }
}

isReportingEdit(e){
this.isReportingEditClicked = true;
}

reportingManagerEdit(e){
  this.isReportingEditClicked = false;
}

isQualificationEdit(e){
  this.isEditClicked = true;
  this.isEmpNameEdit=e.target.title==='Emp Name'?true:false;
  this.isDesignationEdit=e.target.title==='Designation'?true:false;
  this.isHighQualificationEdit=e.target.title==='HighestQualification'?true:false;
  this.isInstitutionEdit=e.target.title==='Institution'?true:false;
  this.isExperienceEdit=e.target.title==='Experience'?true:false;
  this.isPrevEmpDesignationEdit=e.target.title==='PrevEmploymentDesignation'?true:false;
  this.isSkillNameEdit=e.target.title==='SkillName'?true:false;
  this.isDepartmentEdit=e.target.title==='Department'?true:false;
  this.isPrevEmpDetailsEdit=e.target.title==='PrevEmployeeDetails'?true:false;
  this.isJoiningDateEdit=e.target.title==='JoiningDate'?true:false;
}
qualificationEdits(e){
  const targetValue:any=e.target.value;
  const targetName:string=e.target.name;
  this.element.EmpName=targetName==='Emp Name'?targetValue:this.element.EmpName;
  this.element.Designation=targetName==='Designation'?targetValue:this.element.Designation;
  this.element.HighestQualification=targetName==='HighestQualification'?targetValue:this.element.HighestQualification;
  this.element.Institution=targetName==='Institution'?targetValue:this.element.Institution;
  this.element.Experience=targetName==='Experience'?targetValue:this.element.Experience;
  this.element.PrevEmploymentDesignation=targetName==='PrevEmploymentDesignation'?targetValue:this.element.PrevEmploymentDesignation;
  this.element.SkillName=targetName==='SkillName'?targetValue:this.element.SkillName;
  this.element.Department=targetName==='Department'?targetValue:this.element.Department;
  this.element.PrevEmployeeDetails=targetName==='PrevEmployeeDetails'?targetValue:this.element.PrevEmployeeDetails;
  this.element.JoiningDate=targetName==='JoiningDate'?targetValue:this.element.JoiningDate;
  this.isEditClicked = false;
  if(targetName==='JoiningDate'){
    const joiningDate = targetValue.split('-');
    this.joiningDate = joiningDate[1]+'/'+joiningDate[0]+'/'+joiningDate[2];
  }
}
  send(){
  //   if(this.uploadForm.value.avatar===''){
  //  this._snackBar.open('Please upload associate image file.'), "x", {
  //     duration: 1000,
  //     panelClass:['error-alert'],
  //     horizontalPosition: "right",
  //     verticalPosition: "top",
  //   }
  //   }
   // else{
    const obj: SendMailModel = new SendMailModel();
    // this.emailStr=
    // '<html>'+
    // '<p>Dear Associates,</p><br/>'+
    // '<img src=cid:sample width="150" height="150"/><br/><br/>'+    
    //   '<span>'+this.element.EmpName +' has joined the organization as '+this.element.Designation+' on.'+this.isPronoun+' holds '+this.element.HighestQualification+' from '+this.element.Institution+'.'+'</span><br/><br/>'+
    //   '<span>'+this.isPronoun +' brings along with '+this.isPossessivePronoun+' '+this.element.Experience+'+ years of experience as '+this.element.PrevEmploymentDesignation+'.'+'</span>'+
    //   '<span>'+this.isPronoun +' has good exposure '+this.element.SkillName+'.</span>'+
    //   '<span> Prior to us,'+this.isPronoun +' worked as '+this.element.PrevEmploymentDesignation+' with '+
    //   this.element.PrevEmployeeDetails+'.</span>'+
    //   '<br/><br/><p>On behalf of all of us, I extend a hearty welcome to '+this.isPossessivePronoun+' and wish a long and successful career with Seneca Global.</p>'+
    //   '</html>'
    this.emailStr='<html>'+
    '<table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%" style="vertical-align:top"><tbody><tr><td align="center"><img src="cid:sample1" width="300" height="125"></td></tr></tbody></table><table align="center" border="0" cellpadding="0" cellspacing="0" role="presentation"><tbody><tr><td colspan="2"><img src="cid:sample" width="220" height="270"></td><td colspan="4"><div class="x_mj-column-per-50 x_mj-outlook-group-fix" style="font-size:0;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%"><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%" style="vertical-align:top"><tbody><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:20px;text-align:left;color:#ff6366;font-family:Montserrat,Arial;line-height:22px">'+this.saluation+' '+this.empfirstName+'</span><span style="font-size:20px;text-align:left;color:#575757;font-family:Montserrat,Arial;line-height:22px"><b>'+' '+this.emplastName+'</b></span></p></div></td></tr><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px"><span style="font-size:13px;text-align:left;color:#5c5c5c;font-family:Montserrat,Arial;line-height:5px">Joined As</span></p><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:20px;text-align:left;color:#ff6366;font-family:Montserrat,Arial;line-height:22px">'+this.element.Designation+'</span></p></div></td></tr><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px"><span style="font-size:13px;text-align:left;color:#5c5c5c;font-family:Montserrat,Arial;line-height:5px">Reporting to</span></p><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:18px;text-align:left;color:#5c5c5c;font-family:Montserrat,Arial;line-height:22px">'+this.reportingManagerName+'</span></p></div></td></tr></tbody></table></div></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%"><tbody><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#fa5c84;font-family:Montserrat,Arial;line-height:22px"><b>KEY SKILLS</b></span></p><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#1a1a1a;font-family:Montserrat,Arial;line-height:22px">'+this.skillLists+'</span></p></div></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%" style="vertical-align:top"><tbody><tr><td align="center" style="font-size:1px;padding:10px 15px 0 10px;word-break:break-word;border-top:solid 1px #fb8c00"><p style="font-size:1px;margin:0 auto;width:100%"></p></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%"><tbody><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#fa5c84;font-family:Montserrat,Arial;line-height:22px"><b>Education</b></span></p></div></td></tr>' +this.myTable+ '</tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%" style="vertical-align:top"><tbody><tr><td align="center" style="font-size:1px;padding:10px 15px 0 10px;word-break:break-word;border-top:solid 1px #fb8c00"><p style="font-size:1px;margin:0 auto;width:100%"></p></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%"><tbody><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#fa5c84;font-family:Montserrat,Arial;line-height:22px"><b>Bio</b></span></p><p style="text-align:left;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#1a1a1a;font-family:Montserrat,Arial;line-height:22px">'+this.bioContent+'</span></p></div></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%" style="vertical-align:top"><tbody><tr><td align="center" style="font-size:1px;padding:10px 15px 0 10px;word-break:break-word;border-top:solid 1px #fb8c00"><p style="font-size:1px;margin:0 auto;width:100%"></p></td></tr></tbody></table><table border="0" cellpadding="0" cellspacing="0" role="presentation" width="100%"><tbody><tr><td align="left" style="font-size:0;padding:0 25px 0 25px;padding-top:0;padding-right:25px;padding-bottom:0;padding-left:25px;word-break:break-word"><div style="font-family:Ubuntu,Helvetica,Arial,sans-serif;font-size:13px;line-height:1;text-align:left;color:#000"><p style="text-align:center;margin:10px 0;margin-top:10px;margin-bottom:10px"><span style="font-size:14px;text-align:left;color:#1a1a1a;font-family:Montserrat,Arial;line-height:22px">On behalf of all of us, I extend'+this.emptyValue+'<b><span style="color:#ff6366">hearty welcome</span></b>'+this.emptyValue+'to '+this.isPossessivePronoun+' and wish a long and<b><span style="color:#ff6366">'+this.emptyValue+'successful career</span></b>'+this.emptyValue+'with<b><span style="color:#ff6366">'+this.emptyValue+'SenecaGlobal</span></b>.</span></p></div></td></tr></tbody></table>'+
    '</html>'
   
    obj.FormMailContent =  this.emailStr;
    obj.FormMailEmpId = this.element.EmpId;
    obj.FormMailSubject = this.subject.nativeElement.innerText.trim();
    obj.FormMailCC = this.element.WorkEmail;
    let sendMailInfoDetails = JSON.stringify(obj);
    this.formData = new FormData();
    this.formData.append('file', this.uploadForm.get('avatar').value);
    this.formData.append('data', sendMailInfoDetails);
    console.log(this.formData);
    this.associateInformationService.sendWelcomeEmail(this.formData).subscribe((res: any) => {
      if (res) {
        this._snackBar.open('Email sent successfully.', "", {
          duration: 1000,
          panelClass: ['success-alert'],
          horizontalPosition: "right",
          verticalPosition: "top",
        });
        this.dialogRef.close();
      }
    }, (error) => {
      this._snackBar.open(error.error, 'x', {
        duration: 1000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top'
      });
      this.dialogRef.close();
    })
  }

   
  }
 

//}
