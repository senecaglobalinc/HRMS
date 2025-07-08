export interface Relation {
    Name: string;
    RelationShip: string;
    DateOfBirth?: Date;
    BirthDate: string;
    Occupation: string;
}

export class EmergencyContactDetail {
    // isPrimary: boolean;
    // empID: number;
    // contactName: string;
    // contactType: string;
    // birthDate: Date;
    // occupation: string;
    // relationship: string;
    // addressLine1: string;
    // addressLine2: string;
    // city: string;
    // country: string;
    // state: string;
    // zip: number;
    // telephoneNo: number;
    // MobileNo: number;
    // emailAddress:string;
    //above needs to be removed
         Id: number;
         EmployeeId:number;
         ContactType:string;
         ContactName:string;
         Relationship:string;
         AddressLine1:string;
         AddressLine2:string;
         TelePhoneNo:string;
         MobileNo:number;
         EmailAddress:string;
         City:string;
         Country:string;
         PostalCode:string;
         State:string;
         IsPrimary:boolean;
}