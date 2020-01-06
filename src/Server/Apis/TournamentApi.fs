module TournamentApi

open System
open Shared.Dtos
open Shared.Apis

let createNewTournament (newTournament : NewTournament) : Async<OpenTournament>  = async{
  return {
    Id = ""
    Title = ""
    StartDate = DateTime.Now
    Teams = []
  }
}

let (tournamentApi: ITournamentApi) = {
  createNewTournament = createNewTournament
}