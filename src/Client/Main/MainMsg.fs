module MainMsg
open Shared

open LoginModel
open Elmish
open LoginMsg
open MainModel

type MainMsg =
    | LoginMsg of Msg