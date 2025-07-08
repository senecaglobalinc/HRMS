import { common } from '../../support/pageObjects/common'
import { Associate_Abscond } from '../../support/pageObjects/Associate_Abscond'
let commonObj = new common();
let Associate_AbscondObj = new Associate_Abscond()

const associate_details = 
{ 
  //need to variable name and 
  Leademail: "ramesh.avudurthi@senecaglobal.com",
  HRemail:"kalyan.penumutchu@senecaglobal.com",
  ITemail:"manisha.siram@senecaglobal.com",
  Adminemail:"swathi.guguloth@senecaglobal.com",
  Financeemail:"saubhagya.pattanaik@senecaglobal.com",
  Trainingemail:"anitha.muvva@senecaglobal.com", 
  AssociateName:"Kevin currier",
  // Date:  true,
  AbsentFrom:" ",
  AbsentTo:" ",
  RemarksByTL: "TL marked as Absconded",
  RemarksByHRA: "HRA marked as Absconded",
  RemarksByHRM: "HRM marked as Absconded"
  
}


describe('Validate Associate Abscond scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 29 Mar 2023

  it('Verify Associate Abscond', () => {
     commonObj.login() //login common for all
     Associate_AbscondObj.navigateToTeamLead()
     Associate_AbscondObj.associateAbscondReqByTL(associate_details) 
     Associate_AbscondObj.associateAbscondMarkedByHRA(associate_details)
     Associate_AbscondObj.associateAbscondMarkedByHRM(associate_details)
     Associate_AbscondObj.associateAbscondApproveChecklistHRM(associate_details)
     Associate_AbscondObj.associateAbscondApproveChecklistIT(associate_details)
     Associate_AbscondObj.associateAbscondApproveChecklistAdmin(associate_details)
     Associate_AbscondObj.associateAbscondApproveChecklistFinance(associate_details)
     Associate_AbscondObj.associateAbscondApproveChecklistTraining(associate_details)
     Associate_AbscondObj.associateAbscondClearance(associate_details)

    
  })
})