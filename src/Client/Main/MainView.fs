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

let getNavbar (model : MainModel) =
    
    if model.IsLoggedIn then
        Navbar.navbar 
                [ Navbar.Color IsPrimary ]
                [ Navbar.Item.div [ ]
                    [ Heading.h2 [ ]
                        [ str "LMIS Kickerapp" ]
                    ] 
                ]
        |> Some            
    else
        None        

let getLogin (model : MainModel) (dispatch : MainMsg -> unit) =
    if not model.IsLoggedIn then
        Container.container 
            []
            [ 
                Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ div [ Style [MarginTop 20]] [ LoginView.view model.LoginModel (LoginMsg >> dispatch)] ]
            ]
        |> Some    
    else 
        None    

let getFooter (model : MainModel) =
    if model.IsLoggedIn then
        Footer.footer 
            []
            [ 
                Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ str "Nix!" ] 
            ]    
        |> Some
    else
        None    

let view (model : MainModel) (dispatch : MainMsg -> unit) =
    div 
        []
        ([ 
            getNavbar model

            getLogin model dispatch

            getFooter model
        ] 
        |> List.filter (fun o -> o.IsSome)
        |> List.map (fun o -> o.Value))
