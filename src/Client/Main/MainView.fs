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

let view (model : MainModel) (dispatch : MainMsg -> unit) =
    div 
        []
        [ 
            Navbar.navbar 
                [ Navbar.Color IsPrimary ]
                [ Navbar.Item.div [ ]
                    [ Heading.h2 [ ]
                        [ str "LMIS Kickerapp" ]
                    ] 
                ]

            Container.container 
                []
                [ 
                    Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                        [ div [ Style [MarginTop 20]] [ LoginView.view model.LoginModel (LoginMsg >> dispatch)] ]
                ]

            Footer.footer 
                []
                [ 
                    Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                        [ str "Nix!" ] 
                ]
        ]