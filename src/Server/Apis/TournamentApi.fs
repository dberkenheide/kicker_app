module TournamentApi

open System
open Shared.Dtos
open Shared.Apis
open Dapper
open System.Data
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

let createTournamentApi (connection: IDbConnection) : ITournamentApi = {
  createNewTournament = createNewTournament connection
  getAllTournaments = getAllTournaments connection
}