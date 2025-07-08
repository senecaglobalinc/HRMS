//import "common" file from support/pageObjects/  
//in this we create a class with name "common" and creates a constructor for common locaters
// like username, pwd etc. In this we create method login.
import { common } from '../../support/pageObjects/common'
import { exit } from '../../support/pageObjects/Exit'

//import "Associate_Revoke" file from support/pageObjects/
// it will contains locators, calling methods, it is .js file
import { Associate_Revoke } from '../../support/pageObjects/Associate_Revoke'

//Create an object for "common" class
let commonObj = new common();
let exitObj = new exit()

//create an object for "Associate_Revoke" class
let Associate_Revoke_Obj = new Associate_Revoke();

//For associate revoke, we need an associate email who will resigns and revokes, needs Program Manager and HRM email details
//Define all the above detsils as const

const revokeDetails =
{
    
    revoke_Details: "recovered, revoking resignation"

}

//Define test cases here
describe('verify associate revoke after resignation',() => 
{
    it('verify associate revoke after resignation', () =>{
        //step-1 exit class has code for associte login,PM approval and HRM approval  
        exitObj.exit()     

        //step-2 login into application as associate
        commonObj.associateLogin()

        //step-3 associate revokes resignation
        Associate_Revoke_Obj.associateRevoke()

        Associate_Revoke_Obj.associateRevokeReason()


        //step-4 login as HRM
        Associate_Revoke_Obj.loginAsHRM(revokeDetails)

        //step-5 navigate to exit-actions
        Associate_Revoke_Obj.navigateToExitActions(revokeDetails)
        
        //step-6 HRM accept revoke
        Associate_Revoke_Obj.acceptRevoke()




    })
})

