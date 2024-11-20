import { NgModule } from '@angular/core';
import { provideHttpClient  } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { StatisticsComponent } from './components/statistics/statistics.component';


@NgModule({
  declarations: [
  ],
  imports: [
    AppComponent,
    StatisticsComponent,
    CommonModule,
    FormsModule
  ],
  providers: [
    provideHttpClient()
  ]
})
export class AppModule { }
