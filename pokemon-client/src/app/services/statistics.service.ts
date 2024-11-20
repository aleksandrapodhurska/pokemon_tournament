import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pokemon } from '../models/pokemon.model';
import { environment } from '../../environments/environment.development';


@Injectable({
  providedIn: 'root',
})
export class PokemonService {
  private apiUrl = environment.apiBaseUrl;

  constructor(private _http: HttpClient) {}


  getPokemonStatistics(
    sortBy: string,
    sortDirection: string = 'desc'
  ): Observable<Pokemon[]> {
      return this._http.get<Pokemon[]>(`${this.apiUrl}/pokemon/tournament/statistics?sortBy=${sortBy}&sortDirection=${sortDirection}`);
  }
}


