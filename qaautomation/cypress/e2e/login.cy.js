import { common } from '../support/pageObjects/common'
let commonObj = new common();

describe('Login functionality', () => {
  it('Verify successfull loign to the apllication', () => {
    commonObj.visitUrl()
  })
})