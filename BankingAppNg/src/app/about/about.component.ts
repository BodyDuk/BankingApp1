import { Component, OnInit } from '@angular/core';
import { DataService } from '../data.service';
import { TransactionResult } from '../TransactionResult';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  constructor(private dataService: DataService) { }

  transactions: TransactionResult[];  

  ngOnInit() {
    this.dataService.getUserTransactions().subscribe(data=> this.dataSave(data));
  }

  private dataSave(data) {
    this.transactions = data.map(x => new TransactionResult(x.timeStemp, x.operationName, x.amount, x.senderName, x.recipientName));
  }
}