import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { Speciality } from '../models/speciality.model';
import * as servicePath from '../../../service-paths';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SpecialityService {
  public editMode = false;
  private resources = servicePath.API.Speciality;
  public specialityEdit = new BehaviorSubject<Speciality>(new Speciality());
  public specialityList = new BehaviorSubject<Speciality[]>([]);
  _serviceUrl = environment.AdminMicroService;
  constructor(private httpClient : HttpClient ) { }
  
  public getSpecialities(){
    this.httpClient.get(this._serviceUrl + this.resources.list)
    .subscribe((res : Speciality[]) => {this.specialityList.next(res);});
  }

  public createSpeciality(speciality : Speciality){
    if(this.editMode == false)
      return this.httpClient.post(this._serviceUrl + this.resources.create, speciality)
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update, speciality)
    }
}
