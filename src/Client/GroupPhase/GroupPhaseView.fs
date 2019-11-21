module GroupPhaseView

open Fable.React
open Fable.React.Props

open Fulma
open GroupPhaseModel
open GroupPhaseMsg
open GroupPhaseUpdate

let view (model : Model) (dispatch : Msg -> unit) =
    Hero.hero
        [ Hero.IsFullHeight ]
        [ Hero.body [ ]
            [ Container.container
                [ Container.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ div [][ str "Hallo!!" ] ] ] ]