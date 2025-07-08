import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';


@Injectable({
  providedIn: 'root'
})
export class ParkingServiceService {
 private employeeMicroService = environment.EmployeeMicroService;
 private serverUrl = servicePath.API.ParkingBookingSlot;

  constructor(private http : HttpClient) { }

  public bookSlot(saveObj){
    let url = this.employeeMicroService + this.serverUrl.BookParkingSlot;
    return this.http.post(url,saveObj);
  }

  public getSlots(selectedPlace:string){
    let url = this.employeeMicroService + this.serverUrl.GetSlotDetails + selectedPlace;
    return this.http.get(url);
  }

  public getAssociateBookingInfoByEmailId(email : string){
    let url = this.employeeMicroService + this.serverUrl.GetSlotDetailsByEmailID + email;
    return this.http.get(url);
  }

  public releaseParkingSlot(email : string){
    let url = this.employeeMicroService + this.serverUrl.ReleaseParkingSlot + email;
    return this.http.post(url,null);
  }

  public getParkingSlotsReport(parkingReportObj){
    let url = this.employeeMicroService + this.serverUrl.GetParkingSlotReport;
    return this.http.post(url,parkingReportObj);
  }
}
