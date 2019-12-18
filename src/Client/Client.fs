module Client

open Elmish
open Elmish.React
open Fable.FontAwesome
open Fable.FontAwesome.Free
open Fable.React
open Fable.React.Props
open Fulma
open Thoth.Json

open Shared

type PageModel =
  | NotFoundModel
  | LoginModel of Login.Model

type Model = {
  PageModel : PageModel
}

type Msg =
  | LoginMsg of Login.Msg

//let initialCounter = ServerApi.api

let init () : Model * Cmd<Msg> =
  let initialModel = { PageModel = LoginModel (Login.initModel()) }
  //let loadCountCmd =
  //  Cmd.OfAsync.perform initialCounter () InitialCountLoaded
  initialModel, Cmd.none

let update (msg: Msg) (model: Model) =
  match msg, model.PageModel with
  | _, _ ->
      model, Cmd.none

let centerStyle direction =
    Style [ Display DisplayOptions.Flex
            FlexDirection direction
            AlignItems AlignItemsOptions.Center
            JustifyContent "center"
            Padding "20px 0"
    ]

let view (model : Model) (dispatch : Msg -> unit) =
  div [] [
    div [] [
      Menu.view ()
    ]
    div [centerStyle "column" ] [
      match model.PageModel with
      | LoginModel loginModel ->
          yield Login.view loginModel (LoginMsg >> dispatch)
      | NotFoundModel ->
          yield div [] [ str "The page is not available." ]
    ]
  ]


#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
