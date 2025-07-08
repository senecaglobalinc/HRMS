export class Category {
    CategoryMasterId:number;
    CategoryName:string;
    IsActive:boolean;
 
}

// designation page uses all designation, grade, AppreciateDropDownType classes fields
export class CategoryData {
    CategoryMasterId: number;
    CategoryName: string;
    IsActive: boolean;
    ParentId: number;
    ParentCategory: CategoryData;
    ParentCategoryName: string;
}


