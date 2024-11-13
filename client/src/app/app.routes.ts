import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginRegisterComponent } from './login-register/login-register.component';
import { FaqComponent } from './faq/faq.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login-register', component: LoginRegisterComponent},
    {path: 'faq', component: FaqComponent},
    {path: '**', component: HomeComponent, pathMatch: "full"}, //Eventual 404 page
];
