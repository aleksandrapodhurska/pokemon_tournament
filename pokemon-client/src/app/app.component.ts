import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StatisticsComponent } from "./components/statistics/statistics.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, StatisticsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'pokemon-client';
}
