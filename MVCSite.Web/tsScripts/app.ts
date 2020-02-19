/// <reference path='./jqwidgets-ver4.3.0/jqwidgets-ts/jqwidgets.d.ts' />
import { Component, ViewChild, ViewEncapsulation } from '@angular/core';

//import { jqxButtonComponent }   from '../Scripts/jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxbuttons';
//import { jqxNotificationComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxnotification';
//import { jqxDataAdapterComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxdataadapter';
//import { jqxGridComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxgrid';
//import { jqxCheckBoxComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxcheckbox';
//import { TitleComponent } from './components/title.component';

@Component({
    selector: 'my-app'
    , template: `
        <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
            <ul class="nav navbar-nav">
                <li><a routerLink="Admin/Console/User" routerLinkActive="active">User</a></li>
                <li><a routerLink="Admin/Console/Tour" routerLinkActive="active">Tour</a></li>

            </ul>
        </div>

        <div class="container">
            <div class="row">
            <!-- routing here -->
            <router-outlet></router-outlet>
            </div>
        </div>
    `
})
export class AppComponent { }
//                //<li><a routerLink="Admin/Console/Guide" routerLinkActive="active">Guide</a></li>