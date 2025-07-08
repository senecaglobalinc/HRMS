import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment";
import * as servicePath from '../../../core/service-paths';
import { TagAssociateList } from "../models/tag-associate.model";


@Injectable({
  providedIn: 'root'
})
export class TagAssociateService {

  serviceUrl = environment.ServerUrl;
  resources = servicePath.API.TagAssociate;

  constructor(private httpClient: HttpClient) { }

  public GetTagListDetailsByManagerId(managerId: number) {
      let url = this.serviceUrl + this.resources.GetTagListDetailsByManagerId + managerId;
      return this.httpClient.get<TagAssociateList[]>(url)
  }

  public GetTagListNamesByManagerId(managerId: number) {
      let url = this.serviceUrl + this.resources.GetTagListNamesByManagerId + managerId;
      return this.httpClient.get<TagAssociateList[]>(url);
  }

  public CreateTagList(wistLists: TagAssociateList[]) {
      let url = this.serviceUrl + this.resources.CreateTagList;
      return this.httpClient.post(url, wistLists);
  }

  public UpdateTagList(tagLists: TagAssociateList[]) {
      let url = this.serviceUrl + this.resources.UpdateTagList;
      return this.httpClient.post(url, tagLists);
  }

  public DeleteTagList(tagAssociateId: number) {
      let url = this.serviceUrl + this.resources.DeleteTagAssociate + tagAssociateId;
      return this.httpClient.post(url,null);
  }

  public DeleteWholeTagList(tagListName: string, managerId : number) {
      let url = this.serviceUrl + this.resources.DeleteTagList + tagListName + '&managerId=' + managerId ;
      return this.httpClient.post(url,null);
  }


}
