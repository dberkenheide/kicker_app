namespace Shared.Dtos

open System

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

type NewTournament = {
  Title: string
  StartDate: DateTime
}

type Player = {
  PlayerId: string
  Abbreviation: string
}

type Team = {
  PlayerOne: Player
  PlayerTwo: Player
}

type OpenTournament = {
  Id: string
  Title: string
  StartDate: DateTime
  Teams: Team list
}