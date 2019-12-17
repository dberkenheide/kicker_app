module Shared

type Credentials = {UserName: string; Password: string}
type LoginResult = {UserName: string; Token: string}


type Player = Player of string

type Team =
  {
    Name: string
    FirstPlayer: Player
    SecoundPlayer: Player
  }

let noneWhenEmpty str =
    if System.String.IsNullOrWhiteSpace str then None else Some str