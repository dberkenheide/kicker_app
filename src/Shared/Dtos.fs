namespace Shared.Dtos

open System

type Counter = {
  Value : int
}

type Login = {
  UserName: string
  Password: string
  PasswordId: Guid
}

type TournamentId = {
  Name: string
  Id: string
}

type JWT = string

type UserData = {
  UserName : string
  Token : JWT
}