import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-email-management',
  templateUrl: './email-management.component.html',
  styleUrls: ['./email-management.component.css']
})
export class EmailManagementComponent {
  baseUrl: string = environment.apiUrl;
  interval: number;

  constructor(private http: HttpClient, private toastr: ToastrService) {}

  startTimer() {
    this.http.get(this.baseUrl + 'email/timer/start').subscribe(() => {
      this.toastr.success("Started Mailing");
    });
  }

  updateTimer(interval: number) {
    this.http.put(this.baseUrl + 'email/timer/?startTime=' + interval + '&interval=' + interval, {}).subscribe(() => {
      this.toastr.success("Updated Mailing");
    });
  }

  stopTimer() {
    this.http.get(this.baseUrl + 'email/timer/stop').subscribe(() => {
      this.toastr.success("Stopped Mailing");
    });
  }
}
