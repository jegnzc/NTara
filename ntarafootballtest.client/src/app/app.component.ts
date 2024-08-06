import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

export interface FootballTeam {
  id: number;
  team: string;
  mascot: string;
  dateOfLastWin: Date;
  winningPercentage: number;
  wins: number;
  losses: number;
  ties: number;
  games: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public teams: FootballTeam[] = [];
  public query: string = '';
  public column: string = 'All';
  public columns: string[] = ['All', 'Team', 'Mascot'];
  public error: string = '';

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  searchTeams() {
    const params = new HttpParams()
      .set('query', this.query)
      .set('column', this.column);

    this.http.get<{ teams: FootballTeam[] }>('/api/teams/search', { params }).subscribe(
      (result) => {
        this.teams = result.teams;
        this.error = '';
      },
      (error: HttpErrorResponse) => {
        console.error(error);
        this.teams = [];
        if (error.status === 400 && error.error.errors) {
          this.error = this.formatErrorMessages(error.error.errors);
        } else {
          this.error = 'An error occurred while fetching data.';
        }
      }
    );
  }

  private formatErrorMessages(errors: { [key: string]: string[] }): string {
    let errorMessages: string[] = [];
    for (const field in errors) {
      if (errors.hasOwnProperty(field)) {
        errorMessages = errorMessages.concat(errors[field]);
      }
    }
    return errorMessages.join(' ');
  }
}
