
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { JobsComponent } from './Jobs/jobs.component';
import { JobEditComponent } from './Jobs/jobs-edit.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularMaterialModule } from './angular-material.module';
import { MatInputModule } from '@angular/material/input';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

import { LoginComponent } from './login/login.component';
import { UserHomeComponent } from './user-home/user-home.component';
import { AuthGuard } from './guards/auth.guard';
import { HttpInterceptorService } from './services/http-interceptor.service';
import { ErrorInterceptorService } from './services/error-interceptor.service';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AdminGuard } from './guards/admin.guard';






@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    JobsComponent,
    JobEditComponent,
    LoginComponent,
    UserHomeComponent,
    AdminHomeComponent



  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'jobs', component: JobsComponent },
      { path: 'job/:id', component: JobEditComponent },
      { path: 'job', component: JobEditComponent },
      { path: 'login', component: LoginComponent },
      { path: 'user-home', component: UserHomeComponent, canActivate: [AuthGuard] },
      { path: 'admin-home', component: AdminHomeComponent, canActivate: [AdminGuard] }

    ], ),
    BrowserAnimationsModule,
    AngularMaterialModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  exports: [
    MatInputModule,

  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: HttpInterceptorService, multi: true },
  { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
