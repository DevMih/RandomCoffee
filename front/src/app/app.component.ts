import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  users: any;
  newUser: any = {};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers();
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
