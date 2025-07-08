import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { IDomainReport } from '../../Reports/Interfaces/IDomainReport';
import { DomainDataCount } from '../../reports/models/domainreport.model';
import { EmployeeReportData } from '../../reports/models/employee.model';
import * as servicePath from '../../../service-paths';
import { environment } from '../../../../environments/environment';
import { HttpClient} from '@angular/common/http';

@Injectable({
    providedIn: 'root'
  })
 

export class DomainReportService implements IDomainReport {

    private _resources: any;
    private _reportURL: string;

    constructor(private httpClient: HttpClient) {
        this._reportURL = environment.ReportMicroService;
        // this._serverURL = environment.ServerUrl;
        this._resources = servicePath.API.Reports;
    }

    public GetDomainDataCount(): Observable<Array<DomainDataCount>> {
        var _url = this._reportURL + this._resources.GetDomainCountReport;
        return this.httpClient.get<DomainDataCount[]>(_url);
    }
    public GetEmployeesByDomainId(domainId: number): Observable<Array<EmployeeReportData>> {
        let _url = this._reportURL + this._resources.getEmployeesByDomainId + domainId;
        return this.httpClient.get<EmployeeReportData[]>(_url)
    }
}