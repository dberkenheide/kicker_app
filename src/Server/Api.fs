module Api

open System
open Shared.Dtos
open Shared.Apis
open System.Data
open Dapper
open WriteModels

let createNewTournament (connection: IDbConnection) (newTournament : NewTournament) : Async<OpenTournament> = async {
  let (tournament: Tournament) = {
      Title = newTournament.Title
      StartDate = newTournament.StartDate
  }

  let sql = "insert into Tournament (Title, StartDate) values (@Title, @StartDate); SELECT LAST_INSERT_ID();";

  let! newId = Async.AwaitTask(connection.ExecuteScalarAsync(sql, tournament))
  let id = (newId :?> UInt64 |> int)

  return {
    Id = id
    Title = tournament.Title
    StartDate = tournament.StartDate
    Teams = []
  }
}

let getAllTournaments (connection: IDbConnection) (): Async<TournamentForDropDown list> = async {
  let select = "select Title, Id from Tournament";

  let! ids = Async.AwaitTask(connection.QueryAsync<TournamentForDropDown>(select))

  return List.ofSeq ids
}

let createApi (connection: IDbConnection): IApi = {
  login = fun login -> async { return { UserName= "Test"; Token= "42" } }
  createNewTournament = createNewTournament connection
  getAllTournaments = getAllTournaments connection
  getAllPlayers = (fun () -> async { return [ { PlayerId = 1; Abbreviation = "ABC" }] })
}