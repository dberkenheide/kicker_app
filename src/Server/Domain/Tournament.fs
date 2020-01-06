module Tournament
open System


type TournamentId = TournamentId of Guid
type String50 = String50 of String
let createString50 (inputString : string) =
  if inputString.Length > 50 then
    None
  else
    Some inputString

type NewTournament = {
  TournamentId: TournamentId
  Name: String50
  StartDate : DateTime
}

type PlayerId = PlayerId of Guid

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

type ParseNewTournamentDto = Shared.Dtos.NewTournament -> Result<NewTournament, TeamCreationError>

type CreateTournament = NewTournament -> Result<TorunamentInCreationPhase, TeamCreationError>

type AddTeam = TorunamentInCreationPhase -> Team -> Result<TorunamentInCreationPhase, TeamCreationError>

