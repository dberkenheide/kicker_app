module MainMsg
open Shared

open LoginModel
open Elmish
open LoginMsg
open MainModel

type MainMsg =
    | LoginMsg of LoginMsg.Msg
    | GroupPhaseMsg of GroupPhaseMsg.Msg
    //TournamentsMsg
    | NavigateToGroupPhase
    | NavigateToTournamentCreation