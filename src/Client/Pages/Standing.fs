module Standing

open Elmish
open Fable.React
open Fable.React.Props
open Fulma
open Shared
open System
open Thoth.Json

type Model = {
    ModelId: string
  }

type InternMsg =
  | NoMsg

type Msg =
  | Intern of InternMsg

let initModel s =
  {
    ModelId = s
  }, Cmd.none

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | NoMsg ->
      model, Cmd.none

let view (model: Model) (dispatch: Msg -> unit) =
  strong [] [
    str model.ModelId
  ]