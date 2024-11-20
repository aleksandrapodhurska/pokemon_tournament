import { Component, OnInit } from '@angular/core';
import { Pokemon } from '../../models/pokemon.model';
import { PokemonService } from '../../services/statistics.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-statistics',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './statistics.component.html',
  styleUrl: './statistics.component.css'
})
export class StatisticsComponent implements OnInit{
  pokemons: Pokemon[] = [];
  errorMessage = '';
  selectedSortBy: string = 'wins';
  selectedSortDirection: string = 'asc';

  sortByOptions = ['wins', 'losses', 'ties', 'name', 'id'];
  sortDirectionOptions = ['desc', 'asc'];

  constructor(private _pokemonService: PokemonService) {}

  ngOnInit(): void {
    this.fetchData();
  }

  fetchData(): void {
    this._pokemonService.getPokemonStatistics(this.selectedSortBy, this.selectedSortDirection).subscribe(data => {
      this.pokemons = data;
    });
  }
}
