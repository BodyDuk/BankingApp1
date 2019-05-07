import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Const {

    static readonly URL = "http://localhost:51969";
    static readonly UserIdentityController = "http://localhost:51969/UserIdentity";
    static readonly UserController = "http://localhost:51969/User";
    static readonly TransactionController = "http://localhost:51969/Transaction";
    static readonly BankingController = "http://localhost:51969/Banking";

    constructor() { }
}