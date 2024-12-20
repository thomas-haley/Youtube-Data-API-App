import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginRegisterComponent } from './login-register/login-register.component';
import { FaqComponent } from './faq/faq.component';
import { SessionHomeComponent } from './home/session-home/session-home.component';
import { authGuard } from './_guards/auth.guard';
import { TasksViewComponent } from './home/session-home/tasks-view/tasks-view.component';
import { TestComponent } from './test/test.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'login-register', component: LoginRegisterComponent},
    {path: 'faq', component: FaqComponent},
    {path: 'test', component: TestComponent},

    //Logged in routes
    {path: 'dash', component: SessionHomeComponent, canActivate: [authGuard]},
    {path: 'tasks', component: TasksViewComponent, canActivate: [authGuard]},



    {path: '**', component: HomeComponent, pathMatch: "full"}, //Eventual 404 page
];
