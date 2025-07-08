import { constants } from '../constants'

export class Associate_Allocation {
    constructor() {
        this.AssociateName            = 'input[formcontrolname="EmployeeId"]'
        this.ProjectName              = 'input[formcontrolname="ProjectId"]'
        this.Role                     = 'input[formcontrolname="RoleMasterId"]'
        this.AllocationPercentage     = 'input[formcontrolname="AllocationPercentage"]'
        //span[contains(text(),'Select Role')]
        //this.Client                   = 'input[formcontrolname="ClientId"]'
         this.EffectiveDate           = 'input[formcontrolname="EffectiveDate"]'
         this.ReportingManager        = 'input[formcontrolname="ReportingManagerId"]'
         this.ClientBillingRoleName   = 'input[formcontrolname="ClientBillingRoleName"]'
                                        //input[contains(@formcontrolname,'ClientBillingRoleName')]
    }


    navigateTAllocationPage() {
         cy.contains('h4', 'Program Manager').click()
         cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Talent Management").click()
         cy.contains("app-menu-list-item", "Associate Allocation").click()
         cy.url().should('contain', 'allocation', { timeout: 10000 }) 
    }

    associateAllocation(config) {
        const AssociateName = config.AssociateName
        const ProjectName = config.ProjectName
        const Role = config.Role
        const AllocationPercentage = config.AllocationPercentage
        const EffectiveDate = config.EffectiveDate
        const ReportingManager = config.ReportingManager
        const ClientBillingRoleName = config.ClientBillingRoleName
        
        if (AssociateName) {
            //cy.get(this.AssociateName).type(AssociateName)
            // cy.get(this.AssociateName).click()
            // cy.get(this.AssociateName).type(AssociateName)
            // //cy.contains('span.mat-option-text', CompetencyArea).click()

            //for non billable need to uncheck Billable check box
            //cy.contains('span', "Billable").click({force: true}).should('be.unchecked') //comment this line if associate allocated for billable project
            //cy.contains('span', "Primary").click({force: true}).should('be.checked') //comment this line if associate allocated for billable project

            cy.get(this.AssociateName).click()
            cy.contains('span.mat-option-text', AssociateName).click()  
        }
        
        if (ProjectName) {
            cy.get(this.ProjectName).click()
            cy.contains('span.mat-option-text', ProjectName).click()  
        }

        if (Role) {
            cy.get(this.Role).click()
            cy.contains('span.mat-option-text', Role).click() 
        }

        if (AllocationPercentage) {

            //span[contains(text(),'Select Role')]
            cy.contains('span', 'Select Role').click() 
            cy.contains('span.mat-option-text', AllocationPercentage).click() 

        }

         if (EffectiveDate) {
            cy.get(this.EffectiveDate).click()
            cy.wait(2000)
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(2).click()
            //cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
 
        if (ReportingManager) {
            // cy.get(this.ReportingManager).click()
            // cy.contains('span.mat-option-text', ReportingManager).click() 
            cy.contains('span', 'Select Manager').click() 
            cy.contains('span.mat-option-text', ReportingManager).click() 
        }

        if (ClientBillingRoleName) {
            cy.get(this.ClientBillingRoleName).click()
            //div[contains(@class,'mat-radio-outer-circle')]
            cy.contains('div','@class','mat-radio-outer-circle').click()
            //cy.contains("div",'mat-radio-outer-circle')
            cy.contains("span","Apply")
            cy.contains("span","Primary").click()
            //span[contains(text(),'Apply')]    
        }
        
        
    }
        }
    }