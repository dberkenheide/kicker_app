namespace Shared.Dtos

open System

type Login = {
  UserName: string
  Password: string
}

type TournamentState =
  | Preparation = 1
  | Qualifying = 2
  | PlayOffs = 3

type TournamentForDropDown = {
  Title: string
  Id: int
  State: TournamentState
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
  Name: string
  PlayerOne: int option
  PlayerTwo: int option
}

type OpenTournament = {
  Id: int
  Title: string
  StartDate: DateTime
  Teams: Team list
}