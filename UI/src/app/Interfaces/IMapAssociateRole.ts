import { Observable } from 'rxjs/Observable';
import { GenericType } from '../models/dropdowntype.model';
import { AssociateRoleMappingData } from '../models/associaterolemappingdata.model';
export interface IMapAssociateRole {
 GetEmployeesByDepartmentIdAndProjectId(departmentId:number, projectId: number, isNew:boolean):Observable<AssociateRoleMappingData[]>;
 GetKraRolesByDepartmentId(DepartmentId:number):Observable<GenericType[]>;
  MapAssociateRole(associateRoleMappingData: AssociateRoleMappingData):Observable<number>;
}