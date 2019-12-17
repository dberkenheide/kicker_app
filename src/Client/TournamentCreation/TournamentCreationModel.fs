module TournamentCreationModel

open Shared

type Tournament =
  {
    Name: string
    StartDate: System.DateTime
    Teams: Team list
  }

type Model =
  {
    SelectedTournament: Tournament option
    Tournaments: Tournament list
  }