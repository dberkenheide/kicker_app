namespace Shared.Apis

open System
open Shared.Dtos

module Route =
  /// Defines how routes are generated on server and mapped from client
  let builder typeName methodName =
      sprintf "/api/%s/%s" typeName methodName

type IAuthApi = {
  login: Login -> Async<UserData>
}

type ITournamentApi = {
  createNewTournament: NewTournament -> Async<OpenTournament>
}