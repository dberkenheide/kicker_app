module TournamentApi

open System
open Shared.Dtos
open Shared.Apis

let createNewTournament (newTournament : NewTournament) : Async<OpenTournament> = async{
  return {
    Id = "Test"
    Title = "Test"
    StartDate = DateTime.Now
    Teams = []
  }
}

let (tournamentApi: ITournamentApi) = {
  createNewTournament = createNewTournament
}