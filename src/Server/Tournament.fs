module Tournament

open System
open FSharp
open Shared.ResultBuilder
open Shared

type TournamentId = TournamentId of int

type String50 = String50 of String

let createString50 (inputString : string) =
  if inputString.Length > 50 then
    None
  else
    Some inputString

type NewTournament = {
  TournamentId: TournamentId
  Title: String50
  StartDate : DateTime
}

type PlayerId = PlayerId of int

type Player = {
  PlayerId: PlayerId
}

type Team = {
  TeamName: String50
}

type TorunamentInCreationPhase = {
  TournamentId: TournamentId
  Name: String50
  Players: Player list
}

type TeamCreationError =
| PlayerAlreadyExists of Player
| WrongTitleFormat of string
| WrongStartDate of string

type ParseNewTournamentDto = Shared.Dtos.NewTournament -> Result<NewTournament, TeamCreationError>

type CreateTournament = NewTournament -> Result<TorunamentInCreationPhase, TeamCreationError>

type AddTeam = TorunamentInCreationPhase -> Team -> Result<TorunamentInCreationPhase, TeamCreationError>

let validatedTitle title =
  match title with
  | title when String.IsNullOrWhiteSpace title ->
      "Der Titel darf nicht leer sein." |> WrongTitleFormat |> Error

  | title when title.Length > 50 ->
      "Der Titel dar maximal 50 Zeichen lang sein." |> WrongTitleFormat |> Error

  | title -> Ok title

let validatedStartDate startDate =
  match startDate with
  | date when date < DateTime.Today ->
      "Das Datum darf nicht in der Vergangenheit liegen" |> WrongStartDate |> Error

  | date -> Ok date


let parseNewTournamentDto (newTournamentDto: Shared.Dtos.NewTournament) =

  let r = asyncResult {
    let! title = newTournamentDto.Title |> validatedTitle |> AsyncResult.ofResult
    let! startDate = newTournamentDto.StartDate |> validatedStartDate  |> AsyncResult.ofResult

    return 1
  }

  ()