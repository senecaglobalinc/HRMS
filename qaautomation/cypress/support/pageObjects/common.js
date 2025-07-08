import { constants } from '../constants'

export class common {
    constructor() {
        this.username                    = 'input[formcontrolname="email"]'
        
    }

    login() {
        cy.visit(constants.baseUrl)
        cy.get(this.username).type(constants.defaultUser)
        cy.contains('span', 'Log in').click()
        cy.wait(2000)
        cy.contains('h2', 'Status', { timeout: 10000 }).should('be.visible')
        cy.wait(4000)
        cy.url().should('contain', 'roles', { timeout: 10000 })
    }

    associateLogin() {
        cy.visit(constants.baseUrl)
        cy.get(this.username).type(constants.defaultUser)
        cy.contains('span', 'Log in').click()
        cy.wait(2000)
    }
    randomString() {
        //define a variable consisting alphabets in small and capital letter  
        var characters = "ABCDEFGHIJKLMNOPQRSTUVWXTZabcdefghiklmnopqrstuvwxyz";

        //specify the length for the new string  
        var lenString = 7;
        var randomstring = '';

        //loop to select a new character in each iteration  
        for (var i = 0; i < lenString; i++) {
            var rnum = Math.floor(Math.random() * characters.length);
            randomstring += characters.substring(rnum, rnum + 1);
        }
        return randomstring;
    }

}
