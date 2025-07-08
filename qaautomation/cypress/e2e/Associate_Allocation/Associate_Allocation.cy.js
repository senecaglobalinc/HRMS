import { common } from '../../support/pageObjects/common'
import { Associate_Allocation } from '../../support/pageObjects/Associate_Allocation'
let commonObj = new common();
let Associate_AllocationObj = new Associate_Allocation()

const allocation_details = 
{ 
  //need to variable name and 
  AssociateName: "Will smith",
  ProjectName: "Columbus",
  Role: "Analyst",
  AllocationPercentage: "50",
  Client: "3 Step Solutions",//automatically generates
  EffectiveDate: " ",
  ReportingManager: "Kalyan Penumutchu",
  ClientBillingRole: "Developer",
  ClientBillingRoleName:"True"
}


describe('Validate Project Creation scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 28 Mar 2023

  it('Verify associate allocation', () => {
    commonObj.login() //login common for all
    Associate_AllocationObj.navigateTAllocationPage()
    Associate_AllocationObj.associateAllocation(allocation_details)
  })

})