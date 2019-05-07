import { DataService } from '../../data.service';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AppComponent } from 'src/app/app.component';
 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit {
  formModel = {
    UserName: '',
    Password: ''
  }

  errorMessage: string = '';

  constructor(private appCommp: AppComponent, private service: DataService, private router: Router) { }

  ngOnInit() {
    if (localStorage.getItem('token') != null){
      this.router.navigateByUrl('/home');
      this.appCommp.isLogged = true;
    }else{
      this.appCommp.isLogged = false;
    }
  }

  onSubmit(form: NgForm) {
    this.errorMessage = '';
    this.service.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        this.router.navigateByUrl('/home');
        this.appCommp.isLogged = true;
      },
      err => {
        this.errorMessage = 'Incorrect username or password';
      }
    );
  }
}
