import { common } from '../../support/pageObjects/common'
import { Associate_Allocation_Zero_Allocation } from '../../support/pageObjects/Associate_Allocation_Zero_Allocation'
let commonObj = new common();
let Associate_Allocation_Zero_AllocationObj = new Associate_Allocation_Zero_Allocation()

const allocation_details = 
{ 
  AssociateName: "abc xyz",
  ProjectName: "Connections",
  Role: "Analyst",
  //AllocationPercentage: "50",
 // Client: "3 Step Solutions",//automatically generates
  EffectiveDate: " ",
  ReportingManager: "Kalyan P",
  Billable: " ",
  isPrimary: " "
}


describe('Validate Associate Allocation Non Billable scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 18 Apr 2023

  it('Verify associate allocation', () => {
    commonObj.login() //login common for all
    Associate_Allocation_Zero_AllocationObj.navigateTAllocationPage()
    Associate_Allocation_Zero_AllocationObj.Associate_Allocation_Zero_Allocation(allocation_details)
  })

})