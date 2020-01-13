module TournamentPreparation

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open System
open Shared.Dtos
open Shared.ResultBuilder

type Model = {
    Id: int
  }

type InternMsg =
  | AddPlayer
  | SaveTournament

type Msg =
  | Intern of InternMsg

let initModel id =
  {
    Id = id
  }, Cmd.none

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | AddPlayer ->
      model, Cmd.none

  | SaveTournament ->
      model, Cmd.none
  
let view (model: Model) (dispatch: Msg -> unit) =
  strong [] [
    model.Id |> string |> str
  ]  