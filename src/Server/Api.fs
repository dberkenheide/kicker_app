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

let getOrAddUser (ctx: Dbo) (abbreviation: string) = async {
  let abbreviation = abbreviation.ToUpper()

  let user =
    query {
      for p in ctx.Dbo.Player do
      where (p.Abbreviation = abbreviation)
      select p
    } |> Seq.tryHead

  match user with
  | Some u ->
      return u

  | None ->
      let u = ctx.Dbo.Player.``Create(Abbreviation)`` abbreviation
      do! ctx.SubmitUpdatesAsync()
      return u
}

let getUserData (ctx: Dbo) login: Async<UserData> = async {
  match (Ldap.authenticateWithLdap login) with
  | Ok name ->
      let! userData = getOrAddUser ctx name

      return { UserName = userData.Abbreviation; Token= "42" }
  | Error err ->
      return { UserName = err |> Ldap.loginErrorText; Token= "-" }
}

let createApi ctx: IApi = {
  login = getUserData ctx
  createNewTournament = createNewTournament ctx
  getAllTournaments = getAllTournaments ctx
  getAllPlayers = getAllPlayers ctx
}