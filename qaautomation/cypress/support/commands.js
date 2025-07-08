// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add('login', (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add('drag', { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add('dismiss', { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite('visit', (originalFn, url, options) => { ... })
Cypress.Commands.add("ifElementHasText", (locator, textToMatch, waitTime, callBack) => {
    // Use this function to do conditional testing where you want to wait for an element with text to exist before doing something, otherwise move on 
    // You do not need to use this function if element with text appears immediately - use this function only if it takes time for this element to exist, if at all
    // Eg. You click a button. As soon as X is true, do A, otherwise wait for maximum of 10 seconds for X to be true before moving on
    // Such a simple concept, yet Cypress developers refuse to give this to us because they hate conditional testing.
    // locator = the locator for the element that might contain the text
    // textToMatch = the text that might appear on the element
    // waitTime = the maximum wait time in milliseconds 
    // callback = the function that is called if text is found on the element  

    let matchFound = false;
    for (let i = 0; i <= waitTime; i += 500) {
        cy.get('body', { log: false }).then((body) => {
            if (!matchFound && body.find(locator).length > 0) {
                cy.get(locator, { log: false }).invoke({ log: false }, 'text').then((text) => {
                    if (text.includes(textToMatch)) {
                        matchFound = true;
                        callBack();
                    } else {
                        cy.wait(500, { log: false })
                    }
                })
            }
        })
    }
})

Cypress.Commands.add("ifElementExists", (locator, waitTime, callBack, container = 'body') => {
    // locator = the locator of the element that you are checking for
    // waitTime = the maximum wait time in milliseconds 
    // callback = the function that is called if element exists (note: use arrow function for callback or else the 'this' context might be lost) 
    // container = the container within which to find the element. Default value is 'body' to get entire document 

    let matchFound = false;
    for (let i = 0; i <= waitTime; i += 500) {
        cy.get(container, { log: false }).then((body) => {
            if (!matchFound && body.find(locator).length > 0) {
                matchFound = true;
                callBack();
            } else if (!matchFound) {
                cy.wait(500, { log: false })
            }
        })
    }
})

Cypress.Commands.add("containsExact", (locator, exact_text_to_match, config = {}) => {
    let timeout = config.timeout || 7000
    let escapedTextToMatch = exact_text_to_match.replace(/(\W)/g, '\\$1')
    let text_regex = new RegExp(`^${escapedTextToMatch}$`)
    cy.contains(locator, text_regex, { timeout: timeout })
})

Cypress.Commands.add("ifElementWithExactTextExists", (locator, exact_text_to_match, waitTime, callBack) => {
    let matchFound = false;
    for (let i = 0; i < waitTime; i += 500) {
        cy.get('div', { log: false }, { timeout: 20000 }).then(() => {
            let matches = Cypress.$(locator).filter(function () {
                return Cypress.$(this).text() == exact_text_to_match;
            })
            if (!matchFound && matches.length > 0) {
                matchFound = true
                callBack();
            } else if (!matchFound) {
                cy.wait(500, { log: false })
            }
        })
    }
})

Cypress.Commands.add("ifNoElementWithExactTextExists", (locator, exact_text_to_match, waitTime, callBack, container = 'body') => {
    let matchFound = true;
    for (let i = 0; i < waitTime; i += 500) {
        cy.get(container, { log: false }).then((body) => {
            let matches = Cypress.$(locator).filter(function () {
                return Cypress.$(this).text() == exact_text_to_match;
            })
            if (matches.length == 0) {
                matchFound = false
            } else if (matchFound) {
                cy.wait(500, { log: false })
            }
        })
    }

    cy.get(container, { log: false }).then(() => {
        if (!matchFound) {
            callBack()
        }
    })
})

Cypress.Commands.add("getIframeBody", (iframeLocator, index) => {
    // Highly advised that you use cy.waitFOrIframeToLoad before calling this function, unless you're certain iframe is fully loaded already 
    // If iframe is not fully loaded by the time this gets called, it will get a 'snapshot' of the half-loaded body, which will not update as more elements load 
    const i = index ? index : 0 // If iframeLocator expected to return more than 1 match, must specify which one you want 
    cy.get(iframeLocator).should('exist')
    cy.document().then((doc) => {
        let iframe = doc.querySelectorAll(iframeLocator)[i]                            // querySelector is vanilla javascript function to find elements 
        let body = iframe.contentWindow.document.querySelector('body')            // use javascript to find body within the iframe 
        let jElement = Cypress.$(body)                                            // convert js html element to jquery 
        cy.wrap(jElement)                                                         // wrap jquery element with cypress commands, allowing you to chain off this element using cypress
    })
})

Cypress.Commands.add("getIframeHead", (iframeLocator, index) => {
    // Highly advised that you use cy.waitFOrIframeToLoad before calling this function, unless you're certain iframe is fully loaded already 
    // If iframe is not fully loaded by the time this gets called, it will get a 'snapshot' of the half-loaded head, which will not update as more elements load 
    const i = index ? index : 0 // If iframeLocator expected to return more than 1 match, must specify which one you want 
    cy.get(iframeLocator).should('exist')
    cy.document().then((doc) => {
        let iframe = doc.querySelectorAll(iframeLocator)[i]                            // querySelector is vanilla javascript function to find elements 
        let body = iframe.contentWindow.document.querySelector('head')            // use javascript to find body within the iframe head
        let jElement = Cypress.$(body)                                            // convert js html element to jquery 
        cy.wrap(jElement)                                                         // wrap jquery element with cypress commands, allowing you to chain off this element using cypress
    })
})

Cypress.Commands.add("waitForIframeToLoad", (iframeLocator, elementIndicatorLocator, maxWait, index) => {
    // This command waits for the iframe to load a specific element. Does not fail test if condition never becomes true 
    // elementIndicatorLocator is the element whose presence in the iframe would indirectly indicate that the iframe has fully loaded its contents
    // maxWait is the maximum wait time. If element exists before that time, then there will be no more waiting 
    const j = index ? index : 0 // If iframeLocator expected to return more than 1 match, must specify which one you want
    cy.get(iframeLocator, { timeout: 10000 }).should('exist')
    let indicatorLoaded = false
    for (let i = 500; i < maxWait; i += 500) {
        cy.document({ log: false }).then((doc) => {
            let iframe = doc.querySelectorAll(iframeLocator)[j]
            let element = iframe.contentWindow.document.querySelector(elementIndicatorLocator)
            if (!indicatorLoaded && element) {
                indicatorLoaded = true
            } else if (!indicatorLoaded) {
                cy.wait(500, { log: false })
            }
        })
    }
})

Cypress.Commands.add("invokeWithinFrame", (iframeLocator, elementLocator, method, maxWait = 3000, state = {}) => {
    // If you want to invoke an element's methods from within an iframe, use this command 
    // state is an object (optional) that you pass in. Results returned from method will be passed in as state.result. 
    // Note: you cannot directly 'return' any values from a cy command due to async - hence, take advantage of javascript object mutation  

    cy.waitForIframeToLoad(iframeLocator, elementLocator, maxWait)
    cy.document().then((doc) => {
        let iframe = doc.querySelector(iframeLocator)
        let element = iframe.contentWindow.document.querySelector(elementLocator)
        state.result = eval(`element.${method}`)
    })
})

Cypress.Commands.add("waitForFrameElementCondition", (iframeLocator, elementLocator, maxWait, checkCondition) => {
    // This command waits for an iframed element condition to be true before moving to the next cy command
    // Does not fail the test if condition never becomes true. Useful if waiting for iframed video player to play before calling pause on it 
    // checkCondition is a callback that will be passed an html element. checkCondition must return true/false.
    // The element passed into checkCondition is an html element, so in order to get its properties, you have to use vanilla javascript 

    let conditionMet = false
    cy.waitForIframeToLoad(iframeLocator, elementLocator, maxWait)
    for (let i = 500; i < maxWait; i += 500) {
        cy.document({ log: false }).then((doc) => {
            let iframe = doc.querySelector(iframeLocator)
            let element = iframe.contentWindow.document.querySelector(elementLocator)
            if (!conditionMet && checkCondition(element)) {
                conditionMet = true
            } else if (!conditionMet) {
                cy.wait(500, { log: false })
            }
        })
    }
})

Cypress.Commands.add("invokeJS", (elementLocator, method, state = {}) => {
    cy.document().then((doc) => {
        let element = doc.querySelector(elementLocator)
        state.result = eval(`element.${method}`)
    })
})

Cypress.Commands.add("scrollWithin", (config) => {
    const direction = config.direction || 'y' // y for vertical scroll top to bottom, x for horizontal scroll left to right
    const scroller = config.scroller // The scrollable element 
    const find = config.find // The element you are looking for within the scrollable element
    const increment = config.increment || 10 // The % of the total scroll you want to scroll by with each loop 

    // Do not put anything in here - internal use by function only
    const position = isNaN(config.position) ? 0 : config.position
    const positionX = direction == 'x' ? position : 0
    const positionY = direction == 'y' ? position : 0

    if (position <= 100) {
        cy.wait(100, { log: false })
        cy.get(scroller, { log: false, timeout: 20000 }).scrollTo(`${positionX}%`, `${positionY}%`, { log: false })
        config.position = position + increment
        if (find && Cypress.$(find).length > 0) { cy.log("FOUND IT!!!"); return } // Break out of loop if find what you're look for 
        cy.scrollWithin(config)
    }

})

Cypress.Commands.add("mouseWheel", (config) => {
    // Use this instead of scrollWithin when dealing with react virtual scrollers. These are usually found in VEX or website tools.  
    // These elements don't actually scroll - they create and destroy the list as you "scroll", and update the viewable area
    // Calling scrollTo will do nothing. Must trigger a mousewheel event. 
    const scroller = config.scroller
    const find = config.find
    const increment = config.increment || 20
    const maxScroll = config.maxScroll || 1000
    const position = isNaN(config.position) ? 0 : config.position // Internal function use only 

    if ((find && Cypress.$(find).length > 0) || position > maxScroll) { cy.log("Scroll ended"); return }
    cy.get(scroller, { log: false, timeout: 10000 }).trigger('wheel', {
        deltaX: 0,
        deltaY: position
    })
    cy.wait(100) // Need to slow it down or else will miss finding the "find" element
    config.position = position + increment
    cy.mouseWheel(config)
})

Cypress.Commands.add("angryClick", (config) => {
    // Sometimes, our application requires multiple clicks to get something to work, but not when doing this manually 
    const clickElement = config.clickElement // The element you want clicked
    const repeat = config.repeat || 10 // How many times to try again before giving up 
    const checkElement = config.checkElement // The element that should appear when clickElement is clicked
    const interval = config.interval || config.interval == 0 ? config.interval : 1000 // Interval between clicks

    cy.wait(3000)
    let checkElementFound = Cypress.$(checkElement).length
    cy.log(checkElementFound)
    if (checkElementFound < 1) {
        let remainingRepeats = repeat - 1
        cy.get(clickElement).click()
        cy.do(() => {
            if (remainingRepeats == 0) {
                return
            } else {
                if (interval) {
                    cy.wait(interval)
                }
                cy.angryClick({ clickElement: clickElement, checkElement: checkElement, repeat: remainingRepeats, interval: interval })
            }
        })
    }
})