import { constants } from '../constants'

export class exit {
    constructor() {
        this.username                    = 'input[formcontrolname="email"]'
        this.reason                      = 'mat-select[formcontrolname="reasonId"]'
        this.resignationReason_Details   = 'textarea[formcontrolname="reasonDetail"]'
        this.additional_Remarks          = 'textarea[formcontrolname="associateRemarks"]'
        this.additional_Remarks          = 'textarea[formcontrolname="associateRemarks"]'
        this.accept                      = 'mat-icon[mattooltip="Accept Resignation"]'

    }

    exit()
    {
        //step-1 login as associate navigate to resignation menu
        cy.visit(constants.baseUrl)
        cy.get(this.username).type(constants.defaultUser)
        cy.contains('span', 'Log in').click()
        cy.wait(2000)
        
        

        //step-2 navigate to resignation menu
        cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
        cy.contains("app-menu-list-item", "Resignation").click()
        cy.url().should('contain', 'resignation', { timeout: 10000 })
        

        //step-3 provide resignation details and submit
        cy.get(this.reason).click()
        cy.contains('span.mat-option-text', 'Health issues').click()
        cy.get(this.resignationReason_Details).type('due to health reason')
        cy.contains("span", "Submit").click()
        

        //step-4 login as program manager
        cy.visit(constants.baseUrl)
        cy.get(this.username).type(constants.managerEmail)
        cy.contains('span', 'Log in').click()
        cy.wait(2000)
        cy.contains('h4', 'Program Manager').click()
        cy.url().should('contain', 'dashboard', { timeout: 10000 })
        

        //step-5 navigate to Exit actions
        cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
        cy.contains("app-menu-list-item", "Exit-Actions").click()
        cy.url().should('contain', 'exit-actions', { timeout: 10000 })
        

        //step-6 program manager Approve resignation
        cy.contains('span', 'Review').click()
        cy.contains('span', 'Submit').click()
        cy.contains('span', 'Yes').click()
              

        //step-7 login as HRM
        // cy.visit(constants.baseUrl)
        // cy.get(this.username).type(constants.managerEmail)
        // cy.contains('span', 'Log in').click()
        // cy.wait(2000)
        // cy.contains('h4', 'HRM').click()
        // cy.url().should('contain', 'dashboard', { timeout: 10000 })
        cy.contains("span", "more_horiz").click({force: true})
        cy.wait(2000)
        cy.contains("span", " Switch Role").click()
        cy.contains('h4', 'HRM').click()
        cy.url().should('contain', 'dashboard', { timeout: 10000 })
        

        //step-8 navigate to Exit actions
        cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
        cy.contains("app-menu-list-item", "Exit-Actions").click()
        cy.url().should('contain', 'exit-actions', { timeout: 10000 })
        

        //step-9 Approve resignation
        //cy.xpath("//mat-icon[normalize-space()='check']").click({force: true})
        cy.get(this.accept).click({force: true})
        
        //cy.get('mat-icon','check').click({force: true})
        cy.contains('span','Accept').click()

    }
}
