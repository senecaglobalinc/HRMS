import { common } from '../../support/pageObjects/common'
import { Associate_Exit_Delivery } from '../../support/pageObjects/Associate_Exit_Delivery'
import { exit } from '../../support/pageObjects/Exit'

let commonObj = new common()
let Associate_Exit_DeliveryObj = new Associate_Exit_Delivery()
let exitObj = new exit()

 const exitDetails=
 {
    KTProvidedTo: 'Sowjnya P',
    KTStartDate: " ",
    KTEndDate: " "
 }

describe('Verify Associate Exit',()=>
{
    it('Verify Associate Exit', ()=>{
        exitObj.exit() 
        Associate_Exit_DeliveryObj.navigateToTeamLead()
        Associate_Exit_DeliveryObj.navigateToTransitionPlan()
        Associate_Exit_DeliveryObj.defineKTPlan(exitDetails)
        // commonObj.associateLogin()
        // Associate_Exit_DeliveryObj.completeTransitionPlan()
        // Associate_Exit_DeliveryObj.navigateToTeamLead()
        // Associate_Exit_DeliveryObj.verifyTransitionPlan()
        // Associate_Exit_DeliveryObj.corporateLogin()
        // Associate_Exit_DeliveryObj.completeExitInterview()
        // Associate_Exit_DeliveryObj.HRAinitiateactivities()
        // Associate_Exit_DeliveryObj.HRMapprovesChecklist()
        // Associate_Exit_DeliveryObj.ITDeptApproveChecklist()
        // Associate_Exit_DeliveryObj.AdminapproveChecklist()
        // Associate_Exit_DeliveryObj.FinanceDeptApproveChecklist()
        // Associate_Exit_DeliveryObj.TrainingDeptApproveChecklist()
        // Associate_Exit_DeliveryObj.HRMDeactivatesAssociate()

        


    })
})
