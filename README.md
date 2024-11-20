Pokémon Tournament Task
------------------------------
This project implements a Pokemon battle emulation, where each Pokémon is 'fighting' against another in a series of battles.
The fight results are determined based on Pokemon's types and base experience points.
The logic includes a predefined set of type advantages (e.g., Water beats Fire), and if no winner is determined by type, the Pokémon with the higher base experience wins.
In case of a tie in both type and experience, the result is marked as a tie.
------------------------------
Features
------------------------------
Pokemon Type-based Battle Rules: Types such as Water, Fire, Grass, Electric, etc., determine the winner.
Fallback to Base Experience: If types don’t decide the winner, the Pokemon with higher base experience wins.
Tie Handling: If both type and experience are the same, it results in a tie.
Statistical Tracking: Each Pokémon’s wins, losses, and ties are tracked during the tournament simulation.
-------------------------------

Requirements
.NET 6 or higher
C# 10 or higher
A modern web browser (for API testing)

Setup
git clone https://github.com/aleksandrapodhurska/pokemon_tournament

Inside the API directory run:
dotnet restore
dotnet build
dotnet run

Inside the Client directory run:
npm install
ng serve

