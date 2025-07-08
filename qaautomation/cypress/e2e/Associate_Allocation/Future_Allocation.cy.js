import { common } from '../../support/pageObjects/common'
import { Future_Allocation } from '../../support/pageObjects/Future_Allocation'
let commonObj = new common();
let Future_AllocationObj = new Future_Allocation()

const allocation_details = 
{ 
  //need to variable name and 
  AssociateName: "Anitha Muvva",
  ProjectName: "Columbus",
  TentativeDate: " ",
}


describe('Validate Future Allocation scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 29 Mar 2023

  it('Verify Future Allocation', () => {
    commonObj.login() //login common for all
    Future_AllocationObj.navigateToAllocationPage()
    Future_AllocationObj.addFutureAllocation(allocation_details)
  })
})