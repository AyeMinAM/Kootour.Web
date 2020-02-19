
import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { UserComponent } from './components/user/user.component'; //import home components
import { TourComponent } from './components/tour/tour.component'; //import about component

const appRoutes: Routes = [
    { path: 'Admin/Console/User', component: UserComponent },
    { path: 'Admin/Console/Tour', component: TourComponent },
    { path: 'Admin/Console/Tour/:userID', component: TourComponent },
    { path: 'Admin/Console', component: UserComponent, pathMatch: 'full' } // redirect to home page on load
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);


