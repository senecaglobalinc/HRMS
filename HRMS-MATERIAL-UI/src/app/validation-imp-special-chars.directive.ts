import { Directive, ElementRef, HostListener, NgModule } from '@angular/core';
import { MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition, MatSnackBar } from '@angular/material/snack-bar';

@Directive({
  selector: '[appValidationImpSpecialChars]'
})
export class ValidationImpSpecialCharsDirective {
  //Regex for following alphanumerics
  private regex: RegExp = /^([A-Za-z0-9 &_]+)$/;

  //giving snackbar horizontal and vertical positions for showing the messages to the user
  horizontalPosition: MatSnackBarHorizontalPosition = "right";
  verticalPosition: MatSnackBarVerticalPosition = "top";

  constructor(private _ele: ElementRef, private _snackBar: MatSnackBar) {
  }

  // Used on keypress because it will allow the user to use shift and capitalize the alphabets where as its not enabling this feature in the keyup and keydown
  @HostListener('keyup', ['$event']) onKeyPress(event: any) {
    // stringfy the event
    var val = String(event.target.value || "");

    //if the input by the user is not following the regex or giving empty spaces
    var valStr = event.target.value.toString();
    if (!this.regex.test(event.key) && valStr.length > 1) {
      // when the user is giving special characters in between the input
      // if(valStr.charCodeAt(0) == 16 && ((valStr.charCodeAt(0) < 47 && valStr.charCodeAt(0) > 122) ||(valStr.charCodeAt(0) < 65 && valStr.charCodeAt(0) > 90))){

      // }
      this._snackBar.open(
        'Begin with Alphanumerics. Special Characters except _ and & are not allowed',
        'x',
        {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      event.preventDefault();
      event.target.value = valStr.substring(0, valStr.length - 1);
    }

    else if (valStr.length == 1) {
      if (event.shiftKey) {
        if ((valStr.charCodeAt(0) < 47 || valStr.charCodeAt(0) > 122) || (valStr.charCodeAt(0) < 65 || valStr.charCodeAt(0) > 90)) {
          this._snackBar.open(
            'Begin with Alphanumerics. Special Characters except _ and & are not allowed',
            'x',
            {
              duration: 3000,
              horizontalPosition: 'right',
              verticalPosition: 'top',
            }
          );
          event.preventDefault();
          event.target.value = null;
        }
      }
    }

  }

}

@NgModule({
  declarations: [ValidationImpSpecialCharsDirective],
  exports: [ValidationImpSpecialCharsDirective]
})

export class ValidationImpSpecialCharsDirectiveModule { }
