import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = "RandomCoffe";
  users: any;
  newUser: any = {};

  constructor(private http: HttpClient, private accountService: AccountService) {}

  ngOnInit(): void {
    //this.getUsers();
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    this.accountService.setCurrentUser(user);
  }
  
  getUsers() {
    this.http.get("https://localhost:7100/api/users").subscribe(responce => {
      this.users = responce;
    })
  }

  deleteUser(id: any) {
    console.log(id.id);
    this.http.delete("https://localhost:7100/api/users/" + id).subscribe(responce => {
      console.log(responce);
      this.getUsers();
    }, error => {
      console.log(error);
    })
  }

  createUser() {
    this.http.post("https://localhost:7100/api/users/", this.newUser).subscribe(responce => {
      console.log(responce);
      this.getUsers();
    });
  }
}
