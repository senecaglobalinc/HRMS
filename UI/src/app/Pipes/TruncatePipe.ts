import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'truncate'
})
export class TruncatePipe {
  transform(value: string, args: string) : string {
    let limit = args != undefined ? parseInt(args) : 10;

    return (value != null && value != undefined && value.length > limit) ? value.substring(0, limit)  : value;
  }
}