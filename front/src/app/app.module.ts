import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { SharedModule } from './_modules/shared/shared.module';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { HomeComponent } from './home/home.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { RegisterComponent } from './register/register.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { EmailManagementComponent } from './admin/email-management/email-management.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { MessageModalComponent } from './modals/message-modal/message-modal.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    MemberListComponent,
    MemberCardComponent,
    MemberDetailComponent,
    MemberEditComponent,
    HomeComponent,
    RegisterComponent,
    PhotoEditorComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    EmailManagementComponent,
    RolesModalComponent,
    MessageModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    SharedModule,
    ReactiveFormsModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
