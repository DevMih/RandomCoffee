import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private accountService: AccountService, private toastr: ToastrService, 
      private fb: FormBuilder, private router: Router) {}

  ngOnInit(): void {
   this.initializeForm();
  }

  initializeForm() {
    this.registerForm =new FormGroup({
      gender: new FormControl('male'),
      username: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.required),
      secondName: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')])
    })
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value 
      ? null : {isMatching: true}
    }
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(response => {
      this.router.navigateByUrl('/members');
      this.cancel();
    }, error => {
      this.validationErrors = error;
      console.log(error);
      this.toastr.error(error.error);
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

}