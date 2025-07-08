import { constants } from '../constants'

export class Associate_Abscond_Not_Confirm {
    constructor() {
        this.AssociateName           = 'input[formcontrolname="associateId"]'
        this.AbsentFrom              = 'input[formcontrolname="absentFromDate"]'
        this.AbsentTo                = 'input[formcontrolname="absentToDate"]'
        this.NextMonth               = '//button[@aria-label="Next month"]'
        this.RemarksByTL             = 'input[formcontrolname="remarksByTL"]'
        this.RemarksByHRA            = 'input[formcontrolname="remarksByHRA"]'
        this.RemarksByHRM            = 'input[formcontrolname="remarksByHRM"]'
        this.HRemail                 = 'input[formcontrolname="email"]'
    }

    navigateToTeamLead() {
            cy.contains('h4', 'Team Lead').click()
            cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
            cy.contains("app-menu-list-item", "Abscond Request").click()
            cy.url().should('contain', 'abscond-request', { timeout: 10000 })          
            }

        associateAbscondReqByTL(config) {
        const AssociateName = config.AssociateName
        const AbsentFrom    = config.AbsentFrom
        const AbsentTo      = config.AbsentTo
        const RemarksByTL   = config.RemarksByTL
        
        if (AssociateName) {
            cy.get(this.AssociateName).click()
            cy.contains('span.mat-option-text', AssociateName).click()  
        }
        
        if (AbsentFrom) {
            cy.get(this.AbsentFrom).click()
            cy.get('input[formcontrolname="absentFromDate"]').type('4/01/2023')
        }

        if (AbsentTo) {
            cy.get(this.AbsentTo).click()
            cy.get('input[formcontrolname="absentToDate"]').type('4/10/2023')
        } 

        if (RemarksByTL) {
            //cy.get(this.RemarksByTL).click()//type(RemarksByTL)
            cy.xpath("//textarea[@formcontrolname='remarksByTL']").click().type('TL marked as Absconded')
            cy.contains("span", "Submit").click()
            // cy.get('.mat-simple-snackbar-action').invoke('text').then((resp)=>{
            // expect(resp).to.equal('Marked as Abscond Successfully.')})
            }
        }

        associateAbscondMarkedByHRA(config) {
            const HRemail       = config.HRemail
            const RemarksByHRA  = config.RemarksByHRA

            cy.visit(constants.baseUrl)
            cy.get(this.HRemail).type(HRemail)
            cy.contains('span', 'Log in').click()
            // cy.contains('h2', 'Status', { timeout: 10000 }).should('be.visible')
            // cy.url().should('contain', 'roles', { timeout: 10000 })
            cy.contains('h4', 'HRA').click()
            cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
            cy.wait(2000)
            cy.contains("app-menu-list-item", "Abscond-Actions").click()
            cy.url().should('contain', 'abscond-dashboard', { timeout: 10000 })
            cy.pause()
            cy.contains('mat-icon','check').click()
            cy.pause()
            cy.xpath("//textarea[@formcontrolname='remarksByHRA']").click().type('HRA marked as Absconded')
            cy.pause()
            cy.contains("span", "Submit").click()   
                
            }

        associateAbscondMarkedByHRM(config) {
           const HRemail      = config.HRemail
           const RemarksByHRM = config.RemarksByHRM
    
            cy.visit(constants.baseUrl)
            cy.get(this.HRemail).type(HRemail)
            cy.contains('span', 'Log in').click()
            // cy.contains('h2', 'Status', { timeout: 10000 }).should('be.visible')
            // cy.url().should('contain ', 'roles', { timeout: 10000 })
            cy.contains('h4', 'HRM').click()
            cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associate Exit").click()
            cy.wait(2000)
            cy.contains("app-menu-list-item", "Abscond-Actions").click()
            cy.url().should('contain', 'abscond-dashboard', { timeout: 10000 })
            cy.xpath("//textarea[@formcontrolname='remarksByHRM']").click().type('HRM not condirmed as Absconded')
            cy.wait(2000)
            cy.pause()
            cy.contains("span", "Submit").click()
            }
        

    }


    