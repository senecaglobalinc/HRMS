export class BookParkingSlot {
    emailID: string;
    vehicleNumber: string;
    placeName: string;
}

export class parkingReportData {
    Email: string;
    VehicleNumber: string;
    BookedDate: string;
    BookedTime: string;
    Location: string;
}

export class parkingReportObj {
    startDate: string;
    enddate: string;
    location: string;
}

export class parkingReportResponse {
    Items :parkingReportData[];
}