import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import * as servicePath from '../../../core/service-paths';
import { BehaviorSubject } from 'rxjs';
import { PracticeArea } from '../models/practicearea.model';


@Injectable({
  providedIn: 'root'
})
export class PracticeAreaService {
  public editMode = false;
  private _serviceUrl = environment.AdminMicroService;
  private resources = servicePath.API.PracticeArea;
  public practiceAreaEdit = new BehaviorSubject<PracticeArea>(new PracticeArea());
  public practiceAreaList = new BehaviorSubject<PracticeArea[]>([]);
  constructor(private httpClient : HttpClient ) { }
  
  public getPracticeAreas(IsActive: boolean = true){
    this.httpClient.get(this._serviceUrl + this.resources.list+'?IsActive='+IsActive)
    .subscribe((res : PracticeArea[]) =>{ this.practiceAreaList.next(res);});
  }

  public createPracticeAreas(practicearea : PracticeArea){
    if(this.editMode ==false)
      return this.httpClient.post(this._serviceUrl + this.resources.create , practicearea);
    else
      return this.httpClient.post(this._serviceUrl + this.resources.update , practicearea);
  }
}