import { NgModule } from '@angular/core';
import { RouterModule, Routes, mapToCanActivate } from "@angular/router";
import { AuthGuard } from './identity/guard';
import { SignInComponent } from './identity/signin.component';
import { HomeComponent } from './home.component';
import { ForecastComponent } from './forecast.component';
import { RegisterComponent } from './identity/register.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent
  },
  {
    path: 'forecast',
    component: ForecastComponent,
    canActivate: mapToCanActivate([AuthGuard])
  },
  {
    path: 'signin',
    component: SignInComponent    
  },
  {
    path: 'new',
    component: RegisterComponent
  }];

@NgModule({
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
