import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
    MatAutocompleteModule, MatButtonModule, MatButtonToggleModule, MatCardModule,
    MatCheckboxModule, MatChipsModule, MatDatepickerModule, MatDialogModule,
    MatExpansionModule, MatGridListModule, MatIconModule, MatInputModule,
    MatListModule, MatMenuModule, MatNativeDateModule, MatPaginatorModule,
    MatProgressBarModule, MatProgressSpinnerModule, MatRadioModule, MatRippleModule,
    MatSelectModule, MatSidenavModule, MatSliderModule, MatSlideToggleModule,
    MatSnackBarModule, MatSortModule, MatTableModule, MatTabsModule,
    MatToolbarModule, MatTooltipModule, MatStepperModule
} from '@angular/material';


const PrimeNgModules = [
    CommonModule,
    MatAutocompleteModule, MatButtonModule, MatButtonToggleModule, MatCardModule,
    MatCheckboxModule, MatChipsModule, MatDatepickerModule, MatDialogModule,
    MatExpansionModule, MatGridListModule, MatIconModule, MatInputModule,
    MatListModule, MatMenuModule, MatNativeDateModule, MatPaginatorModule,
    MatProgressBarModule, MatProgressSpinnerModule, MatRadioModule, MatRippleModule,
    MatSelectModule, MatSidenavModule, MatSliderModule, MatSlideToggleModule,
    MatSnackBarModule, MatSortModule, MatTableModule, MatTabsModule,
    MatToolbarModule, MatTooltipModule, MatStepperModule
];

@NgModule({
    imports: PrimeNgModules,
    exports: PrimeNgModules,
    declarations: []
})

export class AppMaterialModule {}
