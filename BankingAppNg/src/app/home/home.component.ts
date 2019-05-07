import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { User } from '../User';
import { BankOperation } from '../BankOperation';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],

})
export class HomeComponent implements OnInit {

  user: User = new User('0','Name',0);   
  users: User[];               
  tableMode: boolean = true;  

  transferAmount: number[];
  depositAmount:  number;
  withdrawAmount: number;
 
  constructor( private dataService: DataService) {}

  ngOnInit() {
    this.loadUsers();
    this.loadCurrentUser();
  }

  loadCurrentUser(){
    this.dataService.getUserProfile().subscribe(currentUser =>this.saveUser(currentUser));
  }

  loadUsers() {
      this.dataService.getUsers().subscribe(data=> this.dataSave(data));
  }

  transfer(user: User, index: number ) {
    if (this.user.UserId != null || this.user.UserId != "" ) {
      this.dataService.transfer(new BankOperation(this.transferAmount[index], user.UserId ))
      .subscribe(data => this.printOperationDetails(data));
    } 
    this.transferAmount[index] = undefined; 
  }

  withdraw() {
    if(this.withdrawAmount >=1){
      this.dataService.withdraw(this.withdrawAmount)
      .subscribe(data => this.printOperationDetails(data));
    }
    this.withdrawAmount = undefined; 
  } 

  deposit() {
    if(this.depositAmount >=1){
      this.dataService.deposit(this.depositAmount)
      .subscribe(data => this.printOperationDetails(data));
    }
    this.depositAmount = undefined; 
  } 

  private printOperationDetails(pod){
   if(pod.succeeded == true){
      this.ngOnInit()
    }
  }

  private dataSave(dataSave1) {
    this.users = dataSave1.map(x => new User(x.userId, x.name, x.amount));
    this.transferAmount = new Array<number>(dataSave1.length);
  }

  private saveUser(dataU) {
    this.user = new User(dataU.userId, dataU.name, dataU.amount);
  }
}