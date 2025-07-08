import { common } from '../../support/pageObjects/common'
import { Associate_Allocation_NonBillable } from '../../support/pageObjects/Associate_Allocation_NonBillable'
let commonObj = new common();
let Associate_Allocation_NonBillableObj = new Associate_Allocation_NonBillable()

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
  Billable: " ",
  isPrimary: " "
}


describe('Validate Associate Allocation Non Billable scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 29 Mar 2023

  it('Verify associate allocation', () => {
    commonObj.login() //login common for all
    Associate_Allocation_NonBillableObj.navigateTAllocationPage()
    Associate_Allocation_NonBillableObj.AssociateAllocationNonBillable(allocation_details)
  })

})