import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MenuItem } from 'primeng/api';

@Component({
    selector: 'app-prospective-to-associate',
    templateUrl: './prospective-to-associate.component.html',
    styleUrls: ['./prospective-to-associate.component.scss']
})
export class ProspectiveToAssociateComponent implements OnInit {
    items: MenuItem[];
    private subType: string;
    activeIndex: number = 0;
    private Type: string;

    constructor(private _router: Router, private actRoute: ActivatedRoute) { }
    ngOnInit() {
        this.items = [{
            label: 'Personal',
        },
        {
            label: 'Family',

        },
        {
            label: 'Education',

        },
        {
            label: 'Employment',

        },
        {
            label: 'Professional',

        },
        {
            label: 'Projects',

        },
        {
            label: 'Skills',

        },
        {
            label: 'Upload Documents',

        }
        ];

        this.actRoute.params.subscribe(params => { this.Type = params['type']; });
        this.actRoute.params.subscribe(params => { this.subType = params['subtype']; });
    }



    onSelect(event) {
        if (this.Type == 'new') {
            this.activeIndex = 0;
            return;
        }
        this.activeIndex = event;
    }

    onAssociateSave(data: any) {
        this.Type = 'edit';
    }
    onBack() {
        if (this.subType == "profile") this._router.navigate(['/associates/prospectiveassociate']);
        else if (this.subType == "profileupdate") this._router.navigate(['/associates/associatejoining']);
        else if (this.subType == "list") this._router.navigate(['/associates/associateinformation']);
        else if (this.subType == "EPU") this._router.navigate(["/shared/dashboard"]);
        else this._router.navigate(["/associates/prospectiveassociate/list"]);
    }

}
