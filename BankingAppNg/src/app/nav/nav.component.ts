import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {

  appTitle: string = "BankingApp";
  userDetails;

  constructor(private appComponent: AppComponent, private router: Router, private service: DataService) { }

  ngOnInit() {
    this.appComponent.isLogged = (localStorage.getItem('token') != null)
  }

  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
    this.appComponent.isLogged = false;
  }
}