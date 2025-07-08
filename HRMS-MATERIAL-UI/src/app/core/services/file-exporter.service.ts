import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import { type } from 'os';
import * as XLSX from 'xlsx';
const EXCEL_TYPE='application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=UTF-8';
const EXCEL_EXT = '.xlsx'


@Injectable({
  providedIn: 'root'
})
export class FileExporterService {

  constructor() { }

  exportToExcel(json:any[],excelFileName: string): void{
    const workSheet : XLSX.WorkSheet = XLSX.utils.json_to_sheet(json, { cellDates: true, dateNF: 'MM/DD/YYYY' })
    const workBook: XLSX.WorkBook = {Sheets:{'data':workSheet},
    SheetNames:['data']};
    const excelBuffer:any = XLSX.write(workBook, {bookType:'xlsx',type:'array'})
    this.saveAsExcel(excelBuffer,excelFileName)
  }

  private saveAsExcel(buffer:any, fileName : string): void{
    const data : Blob = new Blob([buffer],{type:EXCEL_TYPE})
    FileSaver.saveAs(data,fileName)
  }
}
