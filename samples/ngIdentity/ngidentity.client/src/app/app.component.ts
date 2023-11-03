import { Component, OnInit } from '@angular/core';
import { AuthService } from './identity/service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public isSignedIn: boolean = false;

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit() {
    this.auth.isSignedIn().forEach((signedIn: boolean) => this.isSignedIn = signedIn);
  }

  signOut() {
    if (this.isSignedIn) {
      this.auth.signOut().forEach(response => {
        if (response) {
          window.location.reload();
        }
      });
    }
  }

  title = 'ngidentity.client';
}
