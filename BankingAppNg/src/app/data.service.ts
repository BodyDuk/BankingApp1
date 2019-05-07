import { Injectable } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http'; 
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { BankOperation } from './BankOperation';
import { Const } from './const'

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  formModel = this.fb.group({
    UserName: ['', Validators.required],
    Passwords: this.fb.group({
      Password: ['', [Validators.required, Validators.minLength(4)]],
      ConfirmPassword: ['', Validators.required]
    }, { validator: this.comparePasswords })
  });

  comparePasswords(fb: FormGroup) {
    let confirmPswrdCtrl = fb.get('ConfirmPassword');
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors) {
      if (fb.get('Password').value != confirmPswrdCtrl.value)
        confirmPswrdCtrl.setErrors({ passwordMismatch: true });
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  register() {
    var body = {
      Name: this.formModel.value.UserName,
      Password: this.formModel.value.Passwords.Password
    };
    return this.http.post(Const.UserIdentityController + '/registerUser', body);
  }

  login(formData) {
    return this.http.post(Const.UserIdentityController + '/authenticate', formData);
  }

  getUserProfile() {
    return this.http.get(Const.UserController + '/userProfile');
  }

  getUsers() {
    return this.http.get(Const.UserController + '/get')
  }

  getUserTransactions() {
    return this.http.get(Const.TransactionController + '/userTransactions')
  }

  transfer(user: BankOperation) {
    return this.http.post(Const.BankingController +'/transfer', user);
  }

  deposit(Amount) {
    return this.http.post(Const.BankingController +'/deposit', Amount);
  }

  withdraw(Amount) {
    return this.http.post(Const.BankingController +'/withdraw', Amount);
  }
}