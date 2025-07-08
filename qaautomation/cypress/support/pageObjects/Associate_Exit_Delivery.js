import { constants} from '../constants'
export class Associate_Exit_Delivery
{
    constructor()
    {
        this.username                    = 'input[formcontrolname="email"]'
        this.ktprovidedto                = 'input[formcontrolname="transitionTo"]'
        this.ktStartCalender             = "//mat-datepicker-toggle[@class='mat-datepicker-toggle ng-tns-c89-30']//span[@class='mat-button-wrapper']//*[name()='svg']"
        this.ktStartNextMonth            = "//button[@aria-label='Next month']"
        this.ktStartDate                 = "//div[normalize-space()='1']"
        this.ktEndCalender               = "//mat-datepicker-toggle[@class='mat-datepicker-toggle ng-tns-c89-31']//span[@class='mat-button-wrapper']//*[name()='svg']"
        this.ktEndNextMonth              = "//button[@aria-label='Next month']"
        this.ktEndDate                   = "//div[normalize-space()='25']"
    }

    navigateToTeamLead()
        {
            cy.visit(constants.baseUrl)            
            cy.get(this.username).type(constants.managerEmail)            
            cy.contains('span', 'Log in').click()            
            cy.wait(2000)
            cy.contains('h4', 'Team Lead').click()            
            cy.url().should('contain', 'dashboard', { timeout: 10000 })         

        }
    navigateToTransitionPlan()
        {
            cy.contains('span','Transition Plan ').click()

        }
    defineKTPlan(config) 
    {
        const KTProvidedTo = config.KTProvidedTo
        const KTStartDate = config.KTStartDate
        const KTEndDate = config.KTEndDate


        if(KTProvidedTo)
        {
        cy.get(this.ktprovidedto).click()
        cy.contains('span.mat-option-text', 'Sowjnya P').click()
        }

        if(KTStartDate)
        {
            cy.xpath(this.ktStartCalender).click()
            cy.xpath(this.ktStartNextMonth).click()
            cy.xpath(this.ktStartDate).click()
        }

        if(KTEndDate)
        {
            cy.xpath(this.ktEndCalender).click()
            cy.xpath(this.ktEndNextMonth).click()
            cy.xpath(this.ktEndDate).click()
        }
    }
}