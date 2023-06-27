import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService) {}

  ngOnInit(): void {
    
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

  login(){
    this.accountService.login(this.model).subscribe(response => {
        this.router.navigateByUrl('/members');
      }, error => {
        console.log(error);
        this.toastr.error(error.error);
      });
  }
}