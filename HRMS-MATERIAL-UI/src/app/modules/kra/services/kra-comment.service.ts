import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import * as servicePath from 'src/app/core/service-paths';
import { CommentModel } from "src/app/modules/kra/models/comment.model";
import { Observable } from 'rxjs/Observable';

@Injectable({
  providedIn: 'root'
})
export class KraCommentService {
  private _resources: any;
  private _serverUrl: string;
  constructor(private _http: HttpClient) {
    this._serverUrl = environment.KRAMicroService;
    this._resources = servicePath.API.KRAComment;
  }

  public CreateComment(commentModel: CommentModel) {
    let url = this._serverUrl + this._resources.createComment;
    return this._http.post(url, commentModel); 
}

  public GetComment(financialYearId: number, departmentId: number, gradeId: number, roleTypeId: number, isCEO: boolean): Observable<Array<CommentModel>> {
    let url = this._serverUrl + this._resources.getComment + financialYearId + "&departmentId=" + departmentId
        + "&gradeId=" + gradeId + "&roleTypeId=" + roleTypeId + "&isCEO=" + isCEO;
    return this._http.get<Array<CommentModel>>(url);
}
}
