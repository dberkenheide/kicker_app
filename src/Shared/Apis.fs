namespace Shared.Apis

open System
open Shared
open Shared.Dtos
open System.Data

module Route =
  /// Defines how routes are generated on server and mapped from client
  let builder typeName methodName =
      sprintf "/api/%s/%s" typeName methodName

type IApi = {
  login: Login -> Async<Result<UserData, string>>
  createNewTournament: NewTournament -> Async<OpenTournament>
  getAllTournaments: unit -> Async<TournamentForDropDown list>
  getAllPlayers: unit -> Async<Player list>
  getTournamentPreparationById: int -> AsyncResult<OpenTournament, string>
}