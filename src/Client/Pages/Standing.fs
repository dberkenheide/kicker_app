module Standing

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Shared.Dtos
open System
open Thoth.Json

type TournamentPreparation = {
  Id: int
  Name: string
  StartDate: DateTime
  Players: Player list
}

type TournamentQualifying = {
  Id: int
  Name: string
  StartDate: DateTime
}

type TournamentPlayOffs = {
  Id: int
  Name: string
  StartDate: DateTime
}

type Tournament =
  | NoTournament
  | Preparation of TournamentPreparation
  | Qualifying of TournamentQualifying
  | PlayOffs of TournamentPlayOffs

type Model = {
  Id: int
  Tournament: Tournament
}

type InternMsg =
  | NoMsg

type Msg =
  | Intern of InternMsg

let initModel id =
  {
    Id = id
    Tournament = NoTournament
  }, Cmd.none

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | NoMsg ->
      model, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) =
  strong [] [
    str ""
  ]