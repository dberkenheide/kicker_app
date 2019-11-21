module MainView

open Elmish
open Elmish.React
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open MainModel
open MainMsg

let getNavbar title =
    Navbar.navbar
            [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str title ]
                ]
            ]

let getLogin (model : LoginModel.Model) (dispatch : MainMsg -> unit) =
    Container.container
        []
        [
            Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ div [ Style [MarginTop 20]] [ LoginView.view model (LoginMsg >> dispatch)] ]
        ]

let getFooter () =
    Footer.footer
        []
        [
            Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                [ str "Nix!" ]
        ]

let view (model : MainModel) (dispatch : MainMsg -> unit) =
    seq {
        if (model.IsLoggedIn) then
            yield getNavbar "LMIS Kicker-App"
            // Content!
            yield getFooter ()
        else
            yield getLogin model.LoginModel dispatch
    }
    |> div []
