import { constants } from '../constants'

export class Associate_Long_Leave {
    constructor() {
        this.AssociateName           = 'input[formcontrolname="AssociateName"]'
        this.Reason                  = 'textarea[formcontrolname="Reason"]'
        this.StartDate               = 'input[formcontrolname="LongLeaveStartDate"]'
        //this.JoinDate                = 'button[aria-label="Open calendar"]'
        this.JoinDate                = 'input[formcontrolname="TentativeJoinDate"]'
        this.NextMonth               = 'button[aria-label="Next month"]'
        this.RadioButton             = "//div[@class='mat-radio-outer-circle']"
        //this.Search                  = 'button[type="submit"]'
        //this.Search                  = 'span[contains(text(),"Search")]'

        //span[contains(text(),'Search')]
            }

    navigateToLongLeavePage() {
         cy.contains('h4', 'HRA').click()
         cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
         cy.contains("app-menu-list-item", "Associate Long Leave").click()
         cy.url().should('contain', 'longleave', { timeout: 10000 }) 
    }

    longLeaveProvision(config) {
        const AssociateName = config.AssociateName
        const SelectAssociate = config.SelectAssociate
        const StartDate = config.StartDate
        const JoinDate = config.JoinDate
        const NextMonth = config.NextMonth
        const Reason = config.Reason
        
        
        if (AssociateName) {
            cy.get(this.AssociateName).click()
            cy.contains('span.mat-option-text', AssociateName).click()
            //cy.contains('span', 'Search').click({force: true})
            cy.xpath("//span[contains(text(),'Search')]").click()
            cy.xpath("//div[@class='mat-radio-outer-circle']").click() 
            //cy.get('.mat-radio-outer-circle').click()
            
            //cy.pause()
        }
        
       
        if (StartDate) {
            cy.get(this.StartDate).click()
            cy.wait(2000)
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
    
        }


        if (JoinDate) {
            
            cy.wait(2000)
            cy.get(this.JoinDate).click()
            cy.wait(2000)
            //cy.get(this.NextMonth).click()
            cy.pause()
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(11).click()
            cy.get(this.Reason).type(Reason)
            cy.contains("span", "Save").click()

            // cy.xpath("//mat-datepicker-toggle[@class='mat-datepicker-toggle ng-tns-c89-542']//span[@class='mat-button-wrapper']//*[name()='svg']//*[name()='path' and contains(@d,'M19 3h-1V1')]").click()
            // cy.xpath("//button[@aria-label='Next month']").click()
            // cy.xpath("//button[@aria-label='Next month']").click()
            // cy.xpath("//button[@aria-label='Next month']").click()
            // cy.xpath("//button[@aria-label='Next month']").click()
            // cy.xpath("//div[normalize-space()='1']").click()

        }     
         
    }
}
    