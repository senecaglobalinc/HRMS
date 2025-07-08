import { constants } from '../constants'

export class Project_Creation {
    constructor() {
        this.projectcode          = 'input[formcontrolname="ProjectCode"]'
        this.projectname          = 'input[formcontrolname="ProjectName"]'
        this.projecttype          = 'input[formcontrolname="ProjectTypeId"]'
        this.technology           = 'input[formcontrolname="PracticeAreaId"]'
        this.client               = 'input[formcontrolname="ClientId"]'
        this.domain               = 'input[formcontrolname="DomainId"]'
        //this.dept                 = 'input[formcontrolname="DepartmentId"]'
        this.startdate            = 'input[formcontrolname="ActualStartDate"]'
        this.email                = 'input[formcontrolname="email"]'
        //this.saveproj             = '//span[normalize-space()="Save"]'
       //######Skipping SOW and Client Billing Roles######//
        // this.sowid                = 'input[formcontrolname="SOWId"]'
        // this.sowfilename          = 'input[formcontrolname="SOWFileName"]'
        // this.sowstartdate         = 'input[formcontrolname="SOWSignedDate"]'
        // this.clientbillingrolename= 'input[formcontrolname="ClientBillingRoleName"]'
        // this.positions            = 'input[formcontrolname="NoOfPositions"]'
        // this.billingpercentage    = 'input[formcontrolname="ClientBillingPercentage"]'
        // this.billingstartdate     = 'input[formcontrolname="StartDate"]'
        //this.clientbillingrole    = "//div[text()='Client Billing Role']"
        this.visibility             = "//mat-icon[contains(.,'visibility')]"
   
        //here we need to write locators
    }


    navigateToprojectdetails() {
         cy.contains('h4', 'Program Manager').click()
         cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Project").click()
         cy.contains("app-menu-list-item", "Project Information").click()
         cy.url().should('contain', 'dashboard', { timeout: 10000 }) 
         cy.contains("span", "Add Project").click()
    }

    addprojectdetails(config) {
        const projectcode = config.projectcode
        const projectname = config.projectname
        const projecttype = config.projecttype
        const technology  = config.technology
        const client      = config.client
        const domain      = config.domain
        //const dept        = config.dept
        const startdate      = config.startdate
       
        if (projectcode) {
            cy.get(this.projectcode).type(projectcode)
        }
        
        if (projectname) {
            cy.get(this.projectname).type(projectname)
        }
        
        if (projecttype) {
            cy.get(this.projecttype).click()
            cy.contains('span.mat-option-text', projecttype).click()     
        }
        
        if (technology) {
            cy.get(this.technology).click()
            cy.contains('span.mat-option-text', technology).click()     
        }
        
        if (client) {
            cy.get(this.client).click()
            cy.contains('span.mat-option-text', client).click()     
        }
        
        if (domain) {
            cy.get(this.domain).click()
            cy.contains('span.mat-option-text', domain).click()     
        }
        // if (dept) {
        //     cy.get(this.domain).click()
        //     cy.contains('span.mat-option-text', dept).click()     
        // }
        if (startdate) {
            cy.get(this.startdate).click()
            cy.wait(2000)
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
            cy.contains("span", "Save").click()
            // cy.contains("span", "yes").click() 
            
            cy.contains("span", "Cancel").click() 
            cy.contains("span", "Cancel").click() 
            cy.contains("span", "Submit For Approval").click() 
            
        }
        
        
    }
   

    // addsowdetails(config) {
    //     const sowid = config.sowid
    //     const sowfilename = config.sowfilename
    //     const sowstartdate      = config.sowstartdate
        
               
    //     if (sowid) {
    //         cy.get(this.sowid).type(sowid)
    //     }
        
    //     if (sowfilename) {
    //         cy.get(this.sowfilename).type(sowfilename)
    //     }
        
    //     if (sowstartdate) {
    //         cy.get(this.sowstartdate).click()
    //         cy.wait(2000)
    //         cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
    //         cy.contains("span", "Add").click()
    //         //cy.contains("span", "Yes").click() 
            
    //     }
    // }
    //     //cy.screenshot()

    //     //Project_CreationObj.addclientbilling(clientbilling)

    //     addclientbilling(config) {
    //         const clientbillingrole = config.clientbillingrole
    //         const clientbillingrolename = config.clientbillingrolename
    //         const positions = config.positions
    //         const billingpercentage = config.billingpercentage
    //         const billingstartdate = config.billingstartdate
          

    //         if (clientbillingrole) {
    //             //cy.xpath(this.clientbillingrole).click()
    //             //div[text()='Client Billing Role']
    //             cy.contains("div", "Client Billing Role").click()
                
    //         }
    //         if (clientbillingrolename) {
    //             cy.get(this.clientbillingrolename).type(clientbillingrolename)
    //         }
            
    //         if (positions) {
    //             cy.get(this.positions).type(positions)
    //         }

    //         if (billingpercentage) {
    //             cy.get(this.billingpercentage).type(billingpercentage)
    //         }
            
            
    //         if (billingstartdate) {
    //             cy.get(this.billingstartdate).click()
    //             cy.wait(2000)
    //             cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
    //             cy.contains("span", "Add").click()
                
    //         }
    navigateToapproveproject(config) {
        const email = config.email
        cy.visit(constants.baseUrl)
        cy.get(this.email).type(email)
        cy.contains('span', 'Log in').click()
        // cy.contains('h2', 'Status', { timeout: 10000 }).should('be.visible')
        // cy.url().should('contain', 'roles', { timeout: 10000 })
         cy.contains('h4', 'Department Head').click()
         //cy.contains("//mat-icon[contains(.,'visibility')]").click()
         cy.contains('mat-icon','visibility').click()
         cy.contains("span", "Approve").click()
    }
        }

      

