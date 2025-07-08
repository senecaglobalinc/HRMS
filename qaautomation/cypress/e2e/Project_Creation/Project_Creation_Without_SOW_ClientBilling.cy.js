import { common } from '../../support/pageObjects/common'
import { Project_Creation } from '../../support/pageObjects/Project_Creation'
let commonObj = new common();
let Project_CreationObj = new Project_Creation()

const projectdetails = { //need to variable name and 
  projectcode: "Q004",
  projectname: "Redis",
  projecttype: "Cloud",
  technology: "Microsoft Technologies (MSFT)",
  client: "3 Step Solutions",
  domain: "HealthCare",
  //dept: 
  startdate: " ",
  email: "kalyan.penumutchu@senecaglobal.com"
  //saveproj: " "
  //need to write other text boxes
  //######Skipping SOW and Client Billing Roles######//
  // sowid: "001",
  // sowfilename: "mmts",
  // sowstartdate: " ",
  // clientbillingrolename:"Developer",
  // positions:"10",
  // billingpercentage:"100",
  // billingstartdate:" "

}
// const sowdetails = { //need to variable name and 
//   sowid: "001",
//   sowfilename: "mmts",
//   sowstartdate: " "
// }
// const clientbilling = { //need to variable name and 
//   clientbillingrolename:"Developer",
//   positions:"10",
//   billingpercentage:"100",
//   billingstartdate:" "
// }



describe('Validate Project Creation scenarios', () => {
  //  * Author By : Ramesh Avudurthi
  //  * Created On : 24 Feb 2023

  it('Verify User is able create new project', () => {
    commonObj.login() //login common for all
    Project_CreationObj.navigateToprojectdetails()
    Project_CreationObj.addprojectdetails(projectdetails)
    //Project_CreationObj.addsowdetails(sowdetails)
    //Project_CreationObj.addclientbilling(clientbilling)
    Project_CreationObj.navigateToapproveproject(projectdetails)
    

  })

})