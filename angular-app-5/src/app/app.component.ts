import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from '../environments/environment';
import { Numbers } from './numbers.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HttpClientModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'my-angular-app-2';
  sumResult: any;
  constructor(private httpClient: HttpClient) {}
  url = environment.apiUrl;

  ngOnInit() {
    this.getSum({ number1: 5, number2: 10 });
  }
  
  getSum(numbers: Numbers) {
    console.log('Sending numbers:', numbers);
    
    this.httpClient
      .post<{ sum: number }>(this.url + 'My/add-numbers', numbers)
      .subscribe({
        next: (response) => {
          this.sumResult = response.sum;
        },
        error: (error: any) => {
          console.error('Error occurred:', error);
          this.sumResult = 0;
        }
      });
  }
}
