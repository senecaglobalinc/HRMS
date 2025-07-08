import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'booleanToString' })
export class BooleanToStringPipe implements PipeTransform {
    transform(value: boolean): string {
        if(value == null)
        {
            return null;
        }
        return ( value == true || value.toString().toLowerCase() == "true" ) ? 'Yes' : 'No'
    }; 
}
