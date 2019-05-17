import { Component, OnInit } from '@angular/core';
import { IdentityService } from '../identity.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-loginbutton',
  templateUrl: './loginbutton.component.html',
  styleUrls: ['./loginbutton.component.css']
})
export class LoginbuttonComponent implements OnInit {

  login() {
    this.identitySvc.login('qweqwe1@qwe.com', '123123', true).subscribe(res => { this.router.navigateByUrl('Dashboard'); });
  }

  logOff() {
    this.identitySvc.logOff().subscribe(res => { this.router.navigateByUrl('Login'); });
  }

  constructor(private identitySvc: IdentityService, private router: Router) { }

  ngOnInit() {
  }

}
