import { Directive, Input, NgModule, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';

@Directive({
  selector:
    '[appRequirematch][ngModel],[appRequirematch][formControl],[appRequirematch][formControlName]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => RequirematchDirective),
      multi: true,
    },
  ],
})
export class RequirematchDirective implements Validator  
{

  @Input() InputValue:any;
  @Input() SearchList: any;
  valuesFound: any;
  

  constructor() {
   }

  validate(control: AbstractControl): { [key: string]: boolean } | null {
    if (control.value){
      const selection: any = typeof control.value === 'string'? control.value : control.value.label;
      if (this.SearchList && this.SearchList.findIndex(x => x.label === selection) < 0 && !(selection === '' || selection === null) ) {
        return { requireMatch: true };
      }
    }
    return null;
  }
  
}
@NgModule({
  declarations: [ RequirematchDirective ],
  exports: [ RequirematchDirective ]
})

export class RequirematchDirectiveModule {}
