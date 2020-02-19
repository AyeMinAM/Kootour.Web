///<reference path="./../typings/globals/core-js/index.d.ts"/>
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app';
import { routing }  from './app.routing';
import { HttpModule } from '@angular/http';

//import { jqxButtonComponent }   from '../Scripts/jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxbuttons';
//import { ActivatedRoute } from '@angular/router';

import { UserComponent } from './components/user/user.component';
import { GuideComponent } from './components/guide/guide.component';
import { TourComponent } from './components/tour/tour.component';
//import { jqxDataAdapterComponent } from 'jqxcomponents/angular_jqxdataadapter';
//import { jqxGridComponent } from 'jqxcomponents/angular_jqxgrid';
import { jqxNotificationComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxnotification';
import { jqxDataAdapterComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxdataadapter';
import { jqxGridComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxgrid';
//import { jqxCheckBoxComponent } from './jqwidgets-ver4.3.0/jqwidgets-ts/angular_jqxcheckbox';
@NgModule({
    imports: [BrowserModule, routing, HttpModule],//other modules the app depends on
    declarations: [// declare all derectives and components
        AppComponent
        //, ActivatedRoute

        , UserComponent
        //, GuideComponent
        , TourComponent
        //, jqxButtonComponent
        , jqxNotificationComponent
        , jqxDataAdapterComponent
        , jqxGridComponent
        //, jqxCheckBoxComponent
        
    ],
    bootstrap: [AppComponent]// root component to bootstarp
})
export class AppModule { }