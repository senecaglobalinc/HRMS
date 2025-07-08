import { common } from '../../support/pageObjects/common'
import { Associate_Long_Leave } from '../../support/pageObjects/Associate_Long_Leave'
let commonObj = new common();
let Associate_Long_LeaveObj = new Associate_Long_Leave()

const longleave_details = 
{ 
  //need to variable name and 
  AssociateName: "Automation Dontdelete",
  StartDate: " ",
  JoinDate: " ",
  Reason:" ", 
  Reason:"Due to health issues"
}


describe('Validate long leave scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 10 Apr 2023

  it('Verify associate long leave', () => {
    commonObj.login() //login common for all
    Associate_Long_LeaveObj.navigateToLongLeavePage()
    Associate_Long_LeaveObj.longLeaveProvision(longleave_details)
  })
})