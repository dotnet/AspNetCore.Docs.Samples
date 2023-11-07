import { Component, OnInit } from '@angular/core';
import { AuthService } from './identity/service';
@Component({
  selector: 'home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  public isSignedIn: boolean = false;

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.authService.onStateChanged().forEach((state: boolean) => {
      this.isSignedIn = state;      
    });
    this.authService.isSignedIn().forEach((signedIn: boolean) => {
      this.isSignedIn = signedIn;
    });
  }

}
