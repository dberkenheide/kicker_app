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
    Players: Player list
  }

type InternMsg =
  | PlayersLoaded of Player list 
  | AddPlayer
  | SaveTournament

type Msg =
  | Intern of InternMsg

let initModel id =
  let loadAllPlayers () = async {
    return! ServerApi.api.getAllPlayers ()
  }

  let loadTournamentsCmd = Cmd.OfAsync.perform loadAllPlayers () (PlayersLoaded >> Intern)
    
  { Id = id
    Players = []
  }, loadTournamentsCmd

let update (msg: InternMsg) model : Model * Cmd<Msg> =
  match msg with
  | AddPlayer ->
      model, Cmd.none

  | SaveTournament ->
      model, Cmd.none

  | PlayersLoaded players ->
      { model with Players = players }, Cmd.none
  
let view (model: Model) (dispatch: Msg -> unit) =
  strong [] [
    model.Id |> string |> str
  ]  