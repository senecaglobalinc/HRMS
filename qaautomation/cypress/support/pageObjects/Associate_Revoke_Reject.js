//importing "constants" file which has application URL and login credentials
import { constants } from '../constants'

export class Associate_Revoke_Reject
{
    constructor()
    {
        this.rejectReason                = 'textarea[formcontrolname="RevokeReason"]'
        this.username                    = 'input[formcontrolname="email"]'
        this.revoke_Reject               = 'mat-icon[mattooltip="Reject Revoke"]'
        //this.reject                      = 'textarea[formcontrolname="RevokeReason"]'
    }

        associateRevoke()
        {
                       
            if(true)
            {
            cy.contains('span','Revoke ').click()          
            }
        }

        associateRevokeReason()
        {
                       
            if(true)
            {
            cy.get('textarea[formcontrolname="RevokeReason"]').type("recovered, revoking resignation")            
            cy.contains('span','Submit').click()
            }


        }
        loginAsHRM()
        {
            cy.visit(constants.baseUrl)            
            cy.get(this.username).type(constants.managerEmail)            
            cy.contains('span', 'Log in').click()            
            cy.wait(2000)
            cy.contains('h4', 'HRM').click()            
            cy.url().should('contain', 'dashboard', { timeout: 10000 })         

        }

        navigateToExitActions()
        {
            cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()            
            cy.contains("app-menu-list-item", "Exit-Actions").click()            
            cy.url().should('contain', 'exit-actions', { timeout: 10000 })
            
            
        }
        rejectRevoke(config)
        {
            //const rejectReason = config.rejectReason
        if(true)
            {
            cy.wait(5000)
            cy.get(this.revoke_Reject).click({force: true})                     
            cy.get(this.rejectReason).type("Revoke rejected")                       
            cy.contains('span','Submit').click()
            }
        }
    }