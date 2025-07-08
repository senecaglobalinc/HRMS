describe("Testing HRMS APIs", ()=>{
        it("Get AllocationPercentage ById",()=>{
            cy.request({
                method:"GET",
                url   :"http://192.168.11.50:3023/project/api/v1/AllocationPercentage/GetById/4",
            }).then ((response)=>
        {
            expect(response.status).to.eq(200)
           // expect(response.body).to.have.property('AllocationPercentageId:5')
        })
    })
})

