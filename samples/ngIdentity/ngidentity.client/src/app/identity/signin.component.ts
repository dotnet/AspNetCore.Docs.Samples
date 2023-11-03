import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "./service";

@Component({
  selector: 'app-signin-component',
  templateUrl: './signin.component.html'
})
export class SignInComponent implements OnInit {
  loginForm!: FormGroup;
  authFailed: boolean = false;
  signedIn: boolean = false;

  constructor(private authService: AuthService,
    private formBuilder: FormBuilder,
    private router: Router) {
    this.authService.isSignedIn().forEach(
      isSignedIn => {
        this.signedIn = isSignedIn;
      });
  }

  ngOnInit(): void {
    this.authFailed = false;
    this.loginForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]]
      });
  }

  public signIn(_: any) {
    if (!this.loginForm.valid) {
      return;
    }
    const userName = this.loginForm.get('email')?.value;
    const password = this.loginForm.get('password')?.value;
    this.authService.signIn(userName, password).forEach(
      response => {
        if (response) {
          this.router.navigateByUrl("forecast");
        }
      }).catch(
        _ => {
          this.authFailed = true;
        });
  }
}
