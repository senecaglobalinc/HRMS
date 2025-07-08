import { constants } from '../constants'

export class hra {
    constructor() {
        this.firstName = 'input[formcontrolname="firstName"]'
        this.middleName = 'input[formcontrolname="middleName"]'
        this.lastName = 'input[formcontrolname="lastName"]'
        this.personalEmailAddress = 'input[formcontrolname="PersonalEmailAddress"]'
        this.mobileNo = 'input[formcontrolname="MobileNo"]'
        this.dateOfJoining = 'input[formcontrolname="dateOfJoining"]'
        this.todayDate = "div[class*='mat-calendar-body-today']"
        this.designation = 'input[formcontrolname="ddldesignationID"]'
        this.gradeName = 'input[formcontrolname="ddlgradeName"]'
        this.employmentType = 'mat-select[formcontrolname="ddlemploymentType"]'
        this.departmentId = 'mat-select[formcontrolname="ddlDepartmentId"]'
        this.technology = 'mat-select[formcontrolname="ddltechnology"]'
        this.managerId = 'mat-select[formcontrolname="ddlmanagerId"]'
        this.advisorName = 'mat-select[formcontrolname="ddlHRAdvisorName"]'
        this.recruitedBy = 'mat-select[formcontrolname="recruitedBy"]'
        this.saveButton = 'button[name="btnSubmit"]'
        this.typeAnythingToSearch = "input[placeholder='Type anything to search']"
    }


    navigateToProspectiveAssociates() {
        cy.contains('h4', 'HRA').click()
        cy.contains("mat-sidenav mat-nav-list app-menu-list-item", "Associates").click()
        cy.contains("app-menu-list-item", "Prospective Associates").click()
        cy.url().should('contain', 'prospectiveassociate', { timeout: 10000 })
    }

    dropProspectiveAssociate(name) {
        cy.wait(2000)
        cy.get(`[class="topNavGlobalSearch"] input:visible`).type(name)
        cy.wait(2000)
        cy.get('body').then(body => {
            if (body.find(`td:contains('${name}')`).length > 0) {
                cy.get('body').find(`td:contains('${name}')`).parent('tr').find("td[class*='Edit'] mat-icon").click()
                cy.get(`mat-checkbox[formcontrolname="dropped"] label`).click()
                cy.get(`textarea[formcontrolname="DropOutReason"]`).type("Exit")
                cy.get(`button[name="btnSubmit"]`).click()
            }
        })

    }

    addProspectiveAssociate(config) {
        const firstName = config.firstName
        const lastName = config.lastName
        const middleName = config.middleName
        const mobileNo = config.mobileNo
        const joiningDate = config.date
        const designation = config.designation
        const employeeType = config.employeeType
        const department = config.department
        const technology = config.technology
        const manager = config.manager
        const hrAdvisor = config.hrAdvisor
        const email = config.email
        cy.contains('app-prospective-associate button', 'Add').click()
        if (firstName) {
            cy.get(this.firstName).type(firstName)
        }
        if (middleName) {
            cy.get(this.middleName).type(middleName)
        }
        if (lastName) {
            cy.get(this.lastName).type(lastName)
        }
        if (email) {
            cy.get(this.personalEmailAddress).type(email)
        }
        if (mobileNo) {
            cy.get(this.mobileNo).type(mobileNo)
        }
        if (joiningDate) {
            cy.get(this.dateOfJoining).click()
            cy.wait(2000)
            cy.get(`mat-month-view tbody tr td[aria-label*='1']`).eq(0).click()
        }
        if (designation) {
            cy.get(this.designation).type(designation)
            cy.contains('span.mat-option-text', designation).click()
        }
        if (employeeType) {
            cy.get(this.employmentType).click()
            cy.contains('span.mat-option-text', employeeType).click()
        }
        if (department) {
            cy.get(this.departmentId).click()
            cy.contains('span.mat-option-text', department).eq(0).click()
        }
        if (technology) {
            cy.wait(200)
            cy.get(this.technology).click()
            cy.contains('span.mat-option-text', technology).click()
        }
        if (manager) {
            cy.get(this.managerId).click()
            cy.contains('span.mat-option-text', manager).click()
        }
        if (hrAdvisor) {
            cy.get(this.advisorName).click()
            cy.contains('span.mat-option-text', hrAdvisor).click()
        }

        cy.wait(1000)
        cy.get(this.saveButton).click()
        cy.wait(1000)
    }

}
