import {Component, HostBinding, Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {animate, state, style, transition, trigger} from '@angular/animations';
import { NavItem } from '../../models/nav-item';
import { NavService } from '../../services/nav.service';
import { Pipe, PipeTransform } from "@angular/core";
import { orderBy } from 'lodash';

@Pipe({
 name: "orderBy"
})
export class OrderByPipe  implements PipeTransform {
 transform(array: any, sortBy: string, order ): any[] {
 const sortOrder = order ? order : 'asc'; // setting default ascending order

  return orderBy(array, [sortBy], [sortOrder]);
  }
}

@Component({
  selector: 'app-menu-list-item',
  templateUrl: './menu-list-item.component.html',
  styleUrls: ['./menu-list-item.component.scss'],
  animations: [
    trigger('indicatorRotate', [
      state('collapsed', style({transform: 'rotate(0deg)'})),
      state('expanded', style({transform: 'rotate(180deg)'})),
      transition('expanded <=> collapsed',
        animate('225ms cubic-bezier(0.4,0.0,0.2,1)')
      ),
    ])
  ]
})
export class MenuListItemComponent implements OnInit {
  expanded: boolean=false;
  @HostBinding('attr.aria-expanded') ariaExpanded = this.expanded;
  @Input() item: any;
  @Input() depth: number;

  constructor(public navService: NavService,
              public router: Router) {

          

    if (this.depth === undefined) {
      this.depth = 0;
    }
  }

  ngOnInit() {

    this.navService.currentUrl.subscribe((url: string) => {
      if (this.item.route && url) {
        this.expanded = url.indexOf(`/${this.item.route}`) === 0;
        this.ariaExpanded = this.expanded;
      }
    });
  }

  onItemSelected(item: any) {
    if (!item.Categories || !item.Categories.length) {
      this.router.navigate([item.Path]);
    }
    if (item.Categories && item.Categories.length) {
      this.expanded = !this.expanded;
    }
  }
}
