import { TestBed } from '@angular/core/testing';
import { of } from "rxjs/internal/observable/of";
import { CompetencyAreaService } from './competency-area.service';
import { CompetencyArea } from '../models/competencyarea.model';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import * as servicePath from "../../../service-paths";
import { environment } from 'src/environments/environment';

export class MockCompetencyAreaService{
   competencyAreaDummyData  = [
     { "CompetencyAreaCode" : "code1" , "CompetencyAreaDescription" : "description1"},
     { "CompetencyAreaCode" : "code2" , "CompetencyAreaDescription" : "description2"},
   ];
   editMode = false;
  
 CreateCompetencyArea(createObj:CompetencyArea){
   if(this.editMode ==false){
    return  of(true);
   } 
   else{ 
    return true;
   }
 }
 GetCompetencyAreaData(){
   return of();
 }
}

fdescribe('CompetencyAreaService', () => {
  let httpMock: HttpTestingController;
  let competencyAreaDummyData  = [
    { "CompetencyAreaCode" : "code1" , "CompetencyAreaDescription" : "description1"},
    { "CompetencyAreaCode" : "code2" , "CompetencyAreaDescription" : "description2"},
  ];
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
  }));

  it('should be created', () => {
    const service: CompetencyAreaService = TestBed.get(CompetencyAreaService);
    expect(service).toBeTruthy();
  });

  it('it should return true if valid data is given' , () => {
    httpMock = TestBed.get(HttpTestingController)
    let competencyService = TestBed.get(CompetencyAreaService);
    competencyService.CreateCompetencyArea(competencyAreaDummyData[0]).subscribe(res =>{
      expect(res).toBeTruthy();
    });
    competencyService.CreateCompetencyArea(competencyAreaDummyData[0]);
    const resources = servicePath.API.CompetencyArea.create;
    const url = `${environment.ServerUrl + resources}`;
    const testRequest = httpMock.expectOne(url);

    expect(testRequest.request.method).toEqual('POST');
    testRequest.flush(of(true));
  })


});