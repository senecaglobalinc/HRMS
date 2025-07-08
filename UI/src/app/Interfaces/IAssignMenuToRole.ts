
import { Observable } from 'rxjs';
import { Menus, MenuRoles } from '../components/admin/Models/menu-roles.model';
export interface IAssignMenusToRole {
    getSourceMenus(RoleId: number)
    getTargetMenus(RoleId: number)
    addTargetMenuRoles(TargetMenuRoles: MenuRoles): Observable<boolean>
}