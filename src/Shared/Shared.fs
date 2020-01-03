namespace Shared

open System

type Counter = { Value : int }

// Json web token type.
type JWT = string

// Login credentials.
type Login = {
  UserName: string
  Password: string
  PasswordId: Guid
}

type TournamentId = {
  Name: string
  Id: string
}

type UserData = {
  UserName : string
  Token : JWT
}

type ApiError =
  | LoginFailed of string

module Route =
  /// Defines how routes are generated on server and mapped from client
  let builder typeName methodName =
      sprintf "/api/%s/%s" typeName methodName

type IAuthApi = {
  login : Login -> Async<UserData>
}