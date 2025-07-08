import { constants } from '../constants'

export class Future_Allocation {
    constructor() {
        this.AssociateName           = 'input[formcontrolname="EmployeeId"]'
        this.ProjectName             = 'input[formcontrolname="ProjectId"]'
        this.Calender                = "//button[@aria-label='Open calendar']//span[@class='mat-button-wrapper']//*[name()='svg']"
        this.NextMonth               = "//button[@aria-label='Next month']"
        this.Date                    = "//div[normalize-space()='1']"
            }

        navigateToAllocationPage() {
         cy.contains('h4', 'Program Manager').click()
         cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Talent Management").click()
         cy.contains("app-menu-list-item", "Future Allocation").click()
         cy.url().should('contain', 'future-allocations', { timeout: 10000 }) 
    }

        addFutureAllocation(config) {
        const AssociateName = config.AssociateName
        const ProjectName = config.ProjectName
        const TentativeDate = config.TentativeDate
        
        if (AssociateName) {

            
            cy.get(this.AssociateName).click()
            cy.contains('span.mat-option-text', AssociateName).click()  
        }
        
        if (ProjectName) {
            cy.get(this.ProjectName).click()
            cy.contains('span.mat-option-text', ProjectName).click()  
        }

        if (TentativeDate) {
            cy.xpath(this.Calender).click()
            cy.xpath(this.NextMonth).click()
            cy.xpath(this.NextMonth).click()
            cy.xpath(this.NextMonth).click()
            cy.xpath(this.NextMonth).click()
            cy.xpath(this.Date).click()
            cy.contains('span', 'Submit').click()
        }     
    }
}
    