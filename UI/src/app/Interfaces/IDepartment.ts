import { Observable } from 'rxjs';
import { Department } from "../components/admin/models/department.model";
export interface IDepartment {
    getDepartmentDetails(): Observable<Department[]> ;
    CreateDepartment(department: Department): Observable<number>;
    UpdateDepartment(department: Department): Observable<number>;
}