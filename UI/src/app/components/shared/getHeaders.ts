import { HttpHeaders } from '@angular/common/http';
export class Header {
    static getHeaders() {
        const headers = new HttpHeaders({ 'Authorization': 'bearer ' + sessionStorage['token'] });
        return headers;
    }

  }
