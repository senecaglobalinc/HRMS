import { Directive, Input, NgModule, forwardRef } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator } from '@angular/forms';

@Directive({
  selector:
    '[appDropDownSuggestion][ngModel],[appDropDownSuggestion][formControl],[appDropDownSuggestion][formControlName]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => DropDownSuggestionDirective),
      multi: true,
    },
  ],
})
export class DropDownSuggestionDirective implements Validator {

  @Input() InputValue: any;
  @Input() SearchList: any;
  valuesFound: any;


  constructor() {
  }

  validate(ctrl: AbstractControl): { [key: string]: boolean } | null {
    var i, filteredlist = [];
    this.valuesFound = [];
    if(this.SearchList){
      for(i=0; i<this.SearchList.length; i++){
        var list =this.SearchList[i]
        filteredlist.push(list.label)
      }
    }
    if (ctrl.value) {
      this.InputValue = ctrl.value
      if (typeof this.InputValue != 'string') {
        this.InputValue = this.InputValue.label;
      }
      this.valuesFound = filteredlist.filter((option) =>
        option.toLowerCase().includes((this.InputValue).toLowerCase())
      );
      return (this.valuesFound.length == 0) ? { 'invalidValue': true } : null
    }
    else {
      return null;
    }
  }

}
@NgModule({
  declarations: [DropDownSuggestionDirective],
  exports: [DropDownSuggestionDirective]
})

export class DropDownSuggestionDirectiveModule { }
