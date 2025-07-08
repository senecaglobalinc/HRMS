import { common } from '../../support/pageObjects/common'
import { Associate_Abscond_Not_Confirm } from '../../support/pageObjects/Associate_Abscond_Not_Confirm'
let commonObj = new common();
let Associate_Abscond_Not_ConfirmObj = new Associate_Abscond_Not_Confirm()

const associate_details = 
{ 
  //need to variable name and 
  HRemail:"ramesh.avudurthi@senecaglobal.com",
  AssociateName:"Rajeev K",
  // Date:  true,
  AbsentFrom:" ",
  AbsentTo:" ",
  RemarksByTL: "TL marked as Absconded",
  RemarksByHRA: "HRA marked as Absconded",
  RemarksByHRM: "HRM marked as Absconded"
  
}

describe('Validate Associate Abscond scenarios - Not Confirmed', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 29 Mar 2023

  it('Verify Associate Abscond', () => {
     commonObj.login() //login common for all
     Associate_Abscond_Not_ConfirmObj.navigateToTeamLead()
     Associate_Abscond_Not_ConfirmObj.associateAbscondReqByTL(associate_details)  
     Associate_Abscond_Not_ConfirmObj.associateAbscondMarkedByHRA(associate_details)
     Associate_Abscond_Not_ConfirmObj.associateAbscondMarkedByHRM(associate_details)
    //  Associate_AbscondObj.associateAbscondApproveChecklistHRM(associate_details)
    //  Associate_AbscondObj.associateAbscondApproveChecklistIT(associate_details)
    //  Associate_AbscondObj.associateAbscondApproveChecklistAdmin(associate_details)
    //  Associate_AbscondObj.associateAbscondApproveChecklistFinance(associate_details)
    //  Associate_AbscondObj.associateAbscondApproveChecklistTraining(associate_details)
    //  Associate_AbscondObj.associateAbscondClearance(associate_details)

    
  })
})