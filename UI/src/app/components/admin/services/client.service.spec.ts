import { TestBed, inject } from '@angular/core/testing';

import { ClientService } from './client.service';

import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import * as servicePath from "../../../service-paths";
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
});


