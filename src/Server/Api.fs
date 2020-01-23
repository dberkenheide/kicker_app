module Api

open System
open Shared.Dtos
open Shared.Apis
open System.Data
open SqlProvider
open FSharp.Data.Sql
open System.Collections.Generic
open Shared

let createNewTournament (ctx: Dbo) (newTournament : NewTournament) : Async<OpenTournament> = async {
  let addedTournament =
    ctx.Dbo.Tournament.``Create(StartDate, State, Title)``
      (newTournament.StartDate, TournamentState.Preparation |> int, newTournament.Title)

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
      select { Title = t.Title; Id= t.Id; State = enum t.State }
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

let getUserData (ctx: Dbo) login: Async<Result<UserData, string>> = async {
  match (Ldap.authenticateWithLdap login) with
  | Ok name ->
      let! userData = getOrAddUser ctx name

      return Ok { UserName = userData.Abbreviation; Token= "42" }
  | Error err ->
      return err |> Ldap.loginErrorText |> Error
}

let getTournamentPreparationById (ctx: Dbo) (id: int) : AsyncResult<OpenTournament, string> = async {
  let! tournament =
    query {
      for tour in ctx.Dbo.Tournament do
      where (tour.Id = id)
      select (tour)
    } |> List.executeQueryAsync

  let! teams =
    query {
      for team in ctx.Dbo.Teams do
      where (team.TournamentId = id)
      select team
    } |> List.executeQueryAsync

  if (tournament.IsEmpty) then
    return Error (sprintf "Turnier mit der Id %i wurde nicht gefunden!" id)
  else
    let t = tournament.Head
  
    return Ok {
      Id = t.Id
      Title = t.Title
      StartDate = t.StartDate
      Teams = teams |> List.map (fun t -> { Name = t.Name; PlayerOne = t.FirstPlayerId; PlayerTwo = t.SecondPlayerId })
    }
}

let createApi ctx: IApi = {
  login = getUserData ctx
  createNewTournament = createNewTournament ctx
  getAllTournaments = getAllTournaments ctx
  getAllPlayers = getAllPlayers ctx
  getTournamentPreparationById = getTournamentPreparationById ctx
}