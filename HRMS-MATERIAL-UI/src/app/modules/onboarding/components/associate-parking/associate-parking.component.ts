import { Component, Inject, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ParkingServiceService } from '../../services/parking-service.service';
import { BookParkingSlot } from '../../models/bookParking.model'
import { themeconfig } from 'src/themeconfig';
import { FormControl, Validators } from '@angular/forms';

interface slotsData {
  TotalSlotCount: number;
  AvailableSlotCount: number;
}


@Component({
  selector: 'app-associate-parking',
  templateUrl: './associate-parking.component.html',
  styleUrls: ['./associate-parking.component.scss']
})
export class AssociateParkingComponent implements OnInit {
  TotalSlots: number = 0;
  AvailableSlots: number = 0;
  BookedSlots: number = 0;
  message: string = '';
  status: string = '';
  today = new Date();
  slotsInfo: slotsData;
  dataRefresher: any;
  slotsAvailbilityMsg: string = '';
  bookedTime: string = '';
  themeConfigInput = themeconfig.formfieldappearances;
  vehicleNo = new FormControl('', [Validators.required]);
  disableVehiceNumber:boolean = false;
  selectedPlace : string='GALAXY';
  //parkingLocations = ['GALAXY', 'SHILPARAMAM'];
  parkingLocations = ['GALAXY'];


  constructor(private service: ParkingServiceService, private _snackBar: MatSnackBar) {

  }

  ngOnInit(): void {
    this.getSlotsData();
    this.refreshData();
    this.getAssociateBookingInfo();
  }

  bookSlot() {
    if (this.vehicleNo.valid) {
      let saveObj: BookParkingSlot = new BookParkingSlot();
      let email = this.getLoggedInEmail();
      saveObj.emailID = email;
      saveObj.vehicleNumber = this.vehicleNo.value;
      saveObj.placeName = this.selectedPlace;
      this.service.bookSlot(saveObj).subscribe((res: any) => {
        if (res.IsSuccessful == true) {
          this.getSlotsData();
          this.getAssociateBookingInfo();
          this._snackBar.open('Slot Booked Successfully', 'x', {
            duration: 5000,
            panelClass: ['success-alert'],
            horizontalPosition: 'right',
            verticalPosition: 'top',
          });

        }
      }, (error) => {
        this._snackBar.open('Error occurred while booking. Please try again!', 'x', {
          duration: 5000,
          panelClass: ['error-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        });
      })
    }

    else{
      this._snackBar.open('Please Enter Vehicle Number', 'x', {
        duration: 5000,
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    }
  }

  getSlotsData() {
    if(this.selectedPlace){
      this.service.getSlots(this.selectedPlace).subscribe((res: any) => {
        this.slotsInfo = res.Item;
        this.AvailableSlots = this.slotsInfo.AvailableSlotCount;
        this.TotalSlots = this.slotsInfo.TotalSlotCount;
        this.BookedSlots = (this.TotalSlots - this.AvailableSlots);
        if (this.slotsInfo.AvailableSlotCount
          <= 0) {
          this.slotsAvailbilityMsg = " Slots are not available for today ";
        }
      }, (error) => {
  
      })
    }
  }

  refreshData() {
    this.dataRefresher = setInterval(() => {
      this.getSlotsData();
    }, 3000);
  }

  ngOnDestroy() {
    clearInterval(this.dataRefresher);
  }

  getAssociateBookingInfo() {
    let email = this.getLoggedInEmail();
    this.service.getAssociateBookingInfoByEmailId(email).subscribe((res: any) => {
      this.vehicleNo.setValue(res.Item.VehicleNumber);
      // this.selectedPlace = res.Item.PlaceName;
      if (res.Item.IsSlotBooked == true) {
        this.bookedTime = res.Item.BookingTime;
        this.message = 'Slot booked ';
        this.status = 'booked';
        this.disableVehiceNumber = true;
        
      }
      else {
        this.status = '';
        this.disableVehiceNumber = false;
      }

    }, (error) => {
      this.status = '';
      this._snackBar.open(error.Message, 'x', {
        duration: 5000,
        panelClass: ['error-alert'],
        horizontalPosition: 'right',
        verticalPosition: 'top',
      });
    })
  }

  onlyAlphaNumeric(event) {
    let regex: RegExp = /^([a-zA-Z0-9 ]*)$/;
    if (!regex.test(event.key)) {
      this._snackBar.open(
        'Special Characters are not allowed',
        'x',
        {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      event.preventDefault();
    }
  }

  releaseSlot(){
    let email = this.getLoggedInEmail();
   this.service.releaseParkingSlot(email).subscribe((res:any)=>{
    if(res.IsSuccessful == true){
      this.getAssociateBookingInfo();
      this.getSlotsData();
      this.status = '';
      this._snackBar.open(
        'Parking Slot Released Successfully',
        'x',
        {
          duration: 3000,
          panelClass: ['success-alert'],
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
    }
   })
  }

  getLoggedInEmail() {
    return JSON.parse(
      sessionStorage.AssociatePortal_UserInformation
    ).email;
  }

  onPlaceChange(e){
    this.selectedPlace = e.value;
    this.getSlotsData();
  }

}


