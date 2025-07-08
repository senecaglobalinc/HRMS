import { Directive, ElementRef, HostListener, NgModule } from '@angular/core';
import { MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition, MatSnackBar } from '@angular/material/snack-bar';

@Directive({
  selector: '[appEmptySpacesValidations]'
})
export class EmptySpacesValidationsDirective {

  //giving snackbar horizontal and vertical positions for showing the messages to the user
  horizontalPosition: MatSnackBarHorizontalPosition = "right";
  verticalPosition: MatSnackBarVerticalPosition = "top";

  constructor(private _ele: ElementRef, private _snackBar: MatSnackBar,) {
  }

  // Used on keypress because it will allow the user to use shift and capitalize the alphabets where as its not enabling this feature in the keyup and keydown
  @HostListener('keyup', ['$event']) onKeyPress(event: any) {
    // stringfy the event
    var val = String(event.target.value || "");

    //if the input by the user is not following the regex or giving empty spaces
    var valueEvent = event.target.value.toString();
    if (valueEvent.charCodeAt(0) == 32) {
      this._snackBar.open(
        'No Empty spaces allowed',
        'x',
        {
          duration: 3000,
          horizontalPosition: 'right',
          verticalPosition: 'top',
        }
      );
      event.preventDefault();
      event.target.value = valueEvent.substring(1);
    }

  }
}

@NgModule({
  declarations: [EmptySpacesValidationsDirective],
  exports: [EmptySpacesValidationsDirective]
})

export class EmptySpacesValidationsDirectiveModule { }
