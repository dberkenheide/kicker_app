namespace Shared.Dtos

open System

type Login = {
  UserName: string
  Password: string
}

[<CLIMutable>]
type TournamentForDropDown = {
  Title: string
  Id: int
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
  PlayerId: int
  Abbreviation: string
}

type Team = {
  PlayerOne: Player
  PlayerTwo: Player
}

type OpenTournament = {
  Id: int
  Title: string
  StartDate: DateTime
  Teams: Team list
}