import { Component, OnInit, ViewChild } from '@angular/core';
import * as servicePath from '../../../service-paths';
import { MessageService } from 'primeng/api';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MasterDataService } from '../../../services/masterdata.service';
import { AdrConfigurationService } from '../Services/adr-configuration.service';
import { ADRConfiguration } from '../Models/adr-configuration.model';
import { GenericType, DropDownType } from '../../../models/dropdowntype.model';
import { ADRSections } from '../Models/adr-sections.model';
import { AdrSectionsService } from '../Services/adr-sections.service';

@Component({
  selector: 'app-adr-configuration',
  templateUrl: './adr-configuration.component.html',
  styleUrls: ['./adr-configuration.component.scss'],
  providers: [MessageService, AdrConfigurationService ]
})
export class AdrConfigurationComponent implements OnInit {
    public adrConfiguration : FormGroup;
    public financialYearsList: DropDownType[] = [];
    public adrSectionsList: DropDownType[] = [];
    public FormSubmitted: boolean = false;
    financialYearId: number = 0;
    constructor(private masterService : MasterDataService, 
        private messageService : MessageService,
        private adrConfig : AdrConfigurationService,
        private adrSectionsService : AdrSectionsService){
    }

    ngOnInit(){
        this.adrConfiguration = new FormGroup({
            adrConfigurationId: new FormControl(null),
            financialYearId: new FormControl(null, [
              Validators.required
            ]),
            adrSectionId: new FormControl(null, [
                Validators.required
              ]),
          });
        this.getFinancialYears();
        this.getADRSectionsData();
    }

    public getFinancialYears() {
        this.masterService.GetFinancialYears().subscribe((yearsdata: GenericType[]) => {
            this.financialYearsList = [];
            this.financialYearsList.push({ label: 'Select Financial Year', value: null });
            yearsdata.forEach((element: GenericType) => {
                this.financialYearsList.push({ label: element.Name, value: element.Id });
            });
        }, (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Failed to get financial years.', detail: '' });
        });
    }

    public getADRSectionsData() {
        this.adrSectionsService.GetADRSections().subscribe((res: ADRSections[]) => {
            this.adrSectionsList = [];
            res.forEach((element: ADRSections) => {
                this.adrSectionsList.push({ label: element.ADRSectionName, value: element.ADRSectionId });
            });
        }, (error: any) => {
            this.messageService.add({ severity: 'error', summary: 'Failed to get ADR Sections.', detail: '' });
        });
    }

    public configureADR() {
    }
}
