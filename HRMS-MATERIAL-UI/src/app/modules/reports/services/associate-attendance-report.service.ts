import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import * as servicePath from '../../../core/service-paths';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { GenericType } from '../../../modules/master-layout/models/dropdowntype.model';
import { AttendanceReportFilter, AttendanceReport, AttendanceDetailReport, AttendanceDetailReportData } from '../models/associate-attendance-report.model';


@Injectable({
  providedIn: 'root'
})
export class AssociateAttendanceReportService {

  reportUrl = environment.EmployeeMicroService;    
  resources = servicePath.API.Reports;
  public attendanceData = new BehaviorSubject<AttendanceDetailReportData>(new AttendanceDetailReportData());
  public attendanceDataObservable = this.attendanceData.asObservable();

  constructor(private httpClient: HttpClient) { }

  public GetAttendanceSummaryReport(filter: AttendanceReportFilter) {
    var url = this.reportUrl + this.resources.GetAttendanceSummaryReport;    
    return this.httpClient.post<AttendanceReport[]>(url, filter);
  }

  public GetBiometricAttendanceSummaryReport(filter: AttendanceReportFilter){
    var url = this.reportUrl + this.resources.GetBiometricAttendanceSummaryReport;    
    return this.httpClient.post<AttendanceReport[]>(url, filter);
  }

  public GetAttendanceDetailReport(filter: AttendanceReportFilter) {
    var url = this.reportUrl + this.resources.GetAttendanceDetailReport;    
    return this.httpClient.post<AttendanceDetailReport[]>(url, filter);
  }
  public GetAdvanceAttendanceReport(filter: AttendanceReportFilter) {
    var url = this.reportUrl + this.resources.GetAdvanceAttendanceReport;    
    return this.httpClient.post<Blob>(url, filter);
  }

  public GetBiometricAttendanceDetailReport(filter: AttendanceReportFilter) {
    var url = this.reportUrl + this.resources.GetBiometricAttendanceDetailReport;    
    return this.httpClient.post<AttendanceDetailReport[]>(url, filter);
  }

  public GetAssociates(employeeId:number, roleName: string, projectId:number, isLeadership: boolean) {
    var url = this.reportUrl + this.resources.GetAssociates + "/" +  employeeId + "/" +  roleName + "/" +  projectId + '?isLeadership=' + isLeadership;    
    return this.httpClient.get<GenericType[]>(url);
  }

  public GetAssociatesForBiometricAttendance(employeeId:number, roleName: string, projectId:number, isLeadership: boolean) {
    var url = this.reportUrl + this.resources.GetAssociatesForBiometricAttendance + "/" +  employeeId + "/" +  roleName + "/" +  projectId + '?isLeadership=' + isLeadership;    
    return this.httpClient.get<GenericType[]>(url);
  }

  public GetProjects(employeeId:number, roleName: string) {
    var url = this.reportUrl + this.resources.GetProjects + "/" +  employeeId + "/" +  roleName ;    
    return this.httpClient.get<GenericType[]>(url);
  }

  public GetProjectsForBiometricAttendance(employeeId:number, roleName: string) {
    var url = this.reportUrl + this.resources.GetProjectsForBiometricAttendance + "/" +  employeeId + "/" +  roleName ;    
    return this.httpClient.get<GenericType[]>(url);
  }

  public GetAttendanceMaxDate() {
    var url = this.reportUrl + this.resources.GetAttendanceMaxDate;    
    return this.httpClient.get<Date>(url);
  }


  public GetBiometricAttendanceMaxDate() {
    var url = this.reportUrl + this.resources.GetBiometricAttendanceMaxDate;    
    return this.httpClient.get<Date>(url);
  }

  public IsDeliveryDepartment(employeeId:number) {
    var url = this.reportUrl + this.resources.IsDeliveryDepartment + "/" +  employeeId;    
    return this.httpClient.get<boolean>(url);
  }

  public IsDeliveryDepartmentforBiometric(employeeId:number) {
    var url = this.reportUrl + this.resources.IsDeliveryDepartmentforBiometric + "/" +  employeeId;    
    return this.httpClient.get<boolean>(url);
  }

  public getMusterReport(year:number,month:number){

    var url = environment.EmployeeMicroService + this.resources.GetMusterReport+year+"/"+month;

    return this.httpClient.get(url,{ headers: {
      'Content-type': 'application/json'
   } });

  }
  
}
