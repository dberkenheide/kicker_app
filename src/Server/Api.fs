module Api

open System
open Shared.Dtos
open Shared.Apis
open System.Data
open SqlProvider
open FSharp.Data.Sql

let createNewTournament (ctx: Dbo) (newTournament : NewTournament) : Async<OpenTournament> = async {  
  let addedTournament = ctx.Dbo.Tournament.``Create(StartDate, Title)`` (newTournament.StartDate, newTournament.Title)

  do! ctx.SubmitUpdatesAsync()

  return {
    Id = addedTournament.Id
    Title = addedTournament.Title
    StartDate = addedTournament.StartDate
    Teams = []
  }
}

let getAllTournaments (ctx: Dbo) (): Async<TournamentForDropDown list> = async {  
  let! tournamentQuery =
    query {
      for t in ctx.Dbo.Tournament do
      sortBy t.StartDate
      select { Title = t.Title; Id= t.Id }
    } |> Seq.executeQueryAsync

  return List.ofSeq tournamentQuery
}

let getAllPlayers (ctx: Dbo) (): Async<Player list> = async {
  let! playerQuery =
    query {
      for p in ctx.Dbo.Player do
      sortBy p.Abbreviation
      select { PlayerId = p.Id; Abbreviation = p.Abbreviation }
    } |> Seq.executeQueryAsync
    
  return List.ofSeq playerQuery
}

let createApi ctx: IApi = {
  login = fun login -> async { return { UserName= "Test"; Token= "42" } }
  createNewTournament = createNewTournament ctx
  getAllTournaments = getAllTournaments ctx
  getAllPlayers = getAllPlayers ctx
}