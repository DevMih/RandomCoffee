import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-message-modal',
  templateUrl: './message-modal.component.html',
  styleUrls: ['./message-modal.component.css']
})
export class MessageModalComponent {
  user: User;
  member: Member;
  message: any = {
    subject: "New user thinks you are a perfect coffee-match!"
  };
  baseUrl = environment.apiUrl;

  constructor(public bsModalRef: BsModalRef, private accountService: AccountService,
     private http: HttpClient, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      this.user = user;
      this.message.body = `Hi!
Hope you're doing well! I stumbled upon your profile on RandomCoffee and found it really
interesting. I thought it'd be cool to meet up for a coffee and chat about our common interests.

What do you say? We can hit up a local café or any place you prefer. I'm pretty flexible with my schedule, so let me know what dates/times work for you.
Looking forward to hearing from you and hopefully grabbing that coffee soon!

Check my profile at <a>http://localhost:4200/members/${this.user.username}</a>

Best regards,
RandomCoffee Team`;
    })
  }

  sendMessage() {
    this.message.to = this.member.email;
    this.message.body = `Hi!\
    Hope you're doing well! I stumbled upon your profile on RandomCoffee and found it really\
     interesting. Your posts and comments caught my attention, and I thought it'd be cool to\
      meet up for a coffee and chat about our common interests.<br><br> \
      What do you say? We can hit up a local café or any place you prefer. \
      I'm pretty flexible with my schedule, so let me know what dates/times work for you.\
      Looking forward to hearing from you and hopefully grabbing that coffee soon!<br><br>\
      Check my profile at <a>http://localhost:4200/members/${this.user.username}<br><br>
      Best regards,<br>\
      RandomCoffee Team`;

    this.http.post(this.baseUrl + 'users/message', this.message).subscribe(responce => {
      this.toastr.success("Email sent!");
    })
    this.bsModalRef.hide();
  }
}
