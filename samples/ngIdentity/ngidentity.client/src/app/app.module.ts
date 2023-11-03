import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { SignInComponent } from './identity/signin.component';
import { Router } from '@angular/router';
import { AuthInterceptor } from './identity/interceptor';
import { AuthGuard } from './identity/guard';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './identity/service';
import { AppRoutingModule } from './app.routing.module';
import { RegisterComponent } from './identity/register.component';
import { ForecastComponent } from './forecast.component';
import { HomeComponent } from './home.component';

@NgModule({
  declarations: [
    SignInComponent,
    RegisterComponent,
    ForecastComponent,
    HomeComponent,
    AppComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useFactory: (router: Router) => {
      return new AuthInterceptor(router);
    },
    multi: true,
    deps: [Router]
  },
    AuthGuard,
    AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
