import { Directive, ElementRef, HostListener, NgModule } from '@angular/core';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';

@Directive({
  selector: '[appValidations]'
})
export class ValidationsDirective {
  //Regex for following alphanumerics
  private regex: RegExp = /^([a-zA-Z0-9 ]*)$/;

  //giving snackbar horizontal and vertical positions for showing the messages to the user
  horizontalPosition: MatSnackBarHorizontalPosition = "right";
  verticalPosition: MatSnackBarVerticalPosition = "top";

  constructor(private _ele: ElementRef, private _snackBar: MatSnackBar,) {
  }

  // Used on keypress because it will allow the user to use shift and capitalize the alphabets where as its not enabling this feature in the keyup and keydown
  @HostListener('keypress', ['$event']) onKeyPress(event: any) {
    // stringfy the event
    var val = String(event.target.value || "");
    var valStr = event.target.value.toString();

    //if the input by the user is not following the regex or giving empty spaces
    if (!this.regex.test(event.key)) {
      // when the user is giving special characters in between the input
      this._snackBar.open(
        'Begin with Alphanumerics. Special Characters are not allowed',
        'x',
        {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      event.preventDefault();
    }

  }
}

@NgModule({
  declarations: [ValidationsDirective],
  exports: [ValidationsDirective]
})

export class ValidationsDirectiveModule { }
