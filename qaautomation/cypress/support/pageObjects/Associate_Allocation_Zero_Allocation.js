import { constants } from '../constants'

export class Associate_Allocation_Zero_Allocation {
    constructor() {
        this.AssociateName            = 'input[formcontrolname="EmployeeId"]'
        this.ProjectName              = 'input[formcontrolname="ProjectId"]'
        this.Role                     = 'input[formcontrolname="RoleMasterId"]'
        this.EffectiveDate            = 'input[formcontrolname="EffectiveDate"]'
        this.ReportingManager         = 'input[formcontrolname="ReportingManagerId"]'
    }

    navigateTAllocationPage() {
         cy.contains('h4', 'Program Manager').click()
         cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Talent Management").click()
         cy.contains("app-menu-list-item", "Associate Allocation").click()
         cy.url().should('contain', 'allocation', { timeout: 10000 }) 
    }

    Associate_Allocation_Zero_Allocation(config) {
        const AssociateName = config.AssociateName
        const ProjectName = config.ProjectName
        const Role = config.Role
        const EffectiveDate = config.EffectiveDate
        const ReportingManager = config.ReportingManager
                
        if (AssociateName) {
            cy.wait(5000)
            cy.contains('span', "0% allocation").click({force: true})

            // cy.contains('span', "0% allocation").click({force: true})
            // cy.contains('span', "0% allocation").click({force: true})
            cy.wait(2000)
            cy.get(this.AssociateName).click()
            cy.contains('span.mat-option-text', AssociateName).click()  
            cy.wait(2000)
        }
        
        if (ProjectName) {

            cy.get(this.ProjectName).click()
            cy.contains('span.mat-option-text', ProjectName).click()  
            cy.wait(2000)
        }

        if (Role) {
            cy.get(this.Role).click()
            cy.contains('span.mat-option-text', Role).click() 
            cy.wait(2000)
        }

         if (EffectiveDate) {
            cy.get(this.EffectiveDate).click()
            cy.wait(2000)
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
            cy.wait(2000)
            //cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
 
        if (ReportingManager) {
            cy.contains('span', 'Select Manager').click() 
            cy.wait(2000)
            cy.contains('span.mat-option-text', ReportingManager).click() 
            cy.wait(2000)
            cy.contains('span', "Allocate").click()
            cy.contains('The Associate has already allocated to the selected project').should('exist')
            cy.wait(5000)
        }
    }
}
}