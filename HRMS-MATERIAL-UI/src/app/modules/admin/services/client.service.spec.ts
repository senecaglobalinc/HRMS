import { TestBed, inject } from '@angular/core/testing';

import { ClientService } from './client.service';

import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
// import * as servicePath from "../../../service-paths";
import * as servicePath from '../../../core/service-paths';
import { environment } from 'src/environments/environment';


describe('ClientService', () => {
  let service: ClientService;
  let httpMock: HttpTestingController;

  const mockClientData = [
    {
      "ClientId":5,
      "ClientName":"ALPS",
      "ClientRegisterName":"ALPS",
      "isActive" : "true"
    }
  ];

  const mockResponse = [
    {
      "ClientId":5,
      "ClientName":"ALPS",
      "ClientRegisterName":"ALPS"
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    httpMock = TestBed.get(HttpTestingController);
    service = TestBed.get(ClientService);
  });

  afterEach(inject(
    [HttpTestingController],
    (httpMock: HttpTestingController) => {
      httpMock.verify();
    }
  ));

  it('should be created', () => {
    const service: ClientService = TestBed.get(ClientService);
    expect(service).toBeTruthy();
  });

  
  // fdescribe('.getAllClients()', () => {
  //   fit('should get all the clients from clients service', () => {
  //     service.getAllClients().subscribe((response : any[]) => {
  //       expect(response).toBeTruthy();
  //       expect(response.length).toBe(mockResponse.length);
  //     });
  //     service.getAllClients()
  //     const resources = servicePath.API.Clients;
  //     const url = `${environment.ServerUrl + resources.list}`;
  //     const testRequest = httpMock.expectOne(url);

  //     expect(testRequest.request.method).toEqual('GET');
  //     testRequest.flush(mockResponse);
  //   });
  // });


  // fdescribe('.createClients()', () => {
  //   fit('should get all the clients from clients service', () => {
  //     service.createClients(mockClientData).subscribe((response : any[]) => {
  //       expect(response).toBeTruthy();
  //       expect(response.length).toBe(mockResponse.length);
  //     });
  //     service.createClients(mockClientData)
  //     const resources = servicePath.API.Clients;
  //     const url = `${environment.ServerUrl + resources.list}`;
  //     const testRequest = httpMock.expectOne(url);

  //     expect(testRequest.request.method).toEqual('GET');
  //     testRequest.flush(mockResponse);
  //   });
  // });

});


