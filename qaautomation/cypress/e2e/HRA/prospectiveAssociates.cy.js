import { common } from '../../support/pageObjects/common'
import { hra } from '../../support/pageObjects/hra'
let commonObj = new common();
let hraObj = new hra()

const emailValue = `testmail${Math.floor((Math.random() * 1000000000) + 1)}@sg.com`
let randomString = commonObj.randomString();
const firstNameValue = `firstName${randomString}`
const lastNameValue = `lastName${randomString}`
const middleNameValue = `middleName${randomString}`
const mobileNumber = Math.floor(Math.random() * 10000000000);

const prospectiveAssociateDetails = {
  firstName: firstNameValue,
  lastName: lastNameValue,
  middleName: middleNameValue,
  email: emailValue,
  mobileNo: mobileNumber,
  date: true,
  designation: "Senior Architect",
  employeeType: "Contractors",
  department: "Administration",
  manager: "Adityanand Pasumarthi",
  hrAdvisor: "Akhila Dasari"
}

describe('Validate Prospective Associates scenarios', () => {
  //  * Author By : Ananth
  //  * Created On : 20 Dec 2022

  it('Verify User is able to add prospective associates details', () => {
    commonObj.login()
    hraObj.navigateToProspectiveAssociates()
    hraObj.dropProspectiveAssociate(prospectiveAssociateDetails.firstName)
    hraObj.addProspectiveAssociate(prospectiveAssociateDetails)
    hraObj.dropProspectiveAssociate(prospectiveAssociateDetails.firstName)
  })

})