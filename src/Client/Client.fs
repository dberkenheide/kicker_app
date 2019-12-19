module Client

open Elmish
open Elmish.React
open Elmish.UrlParser
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

[<RequireQualifiedAccess>]
type Page =
  | Home
  | NotFound
  | Login

let toPath =
  function
  | Page.Home -> "/"
  | Page.Login -> "/login"
  | Page.NotFound -> "/notfound"

  /// The URL is turned into a Result.
let pageParser : Parser<Page -> Page, Page> =
    oneOf
        [ UrlParser.map Page.Home (UrlParser.s "")
          UrlParser.map Page.Login (UrlParser.s "login")
          UrlParser.map Page.NotFound (UrlParser.s "notfound") ]

let urlParser location = parsePath pageParser location

type Model = {
  PageModel : PageModel
}

type Msg =
  | LoginMsg of Login.Msg

//let initialCounter = ServerApi.api

let init (page: Page option) : Model * Cmd<Msg> =
  let initialModel = { PageModel = NotFoundModel }
  //let loadCountCmd =
  //  Cmd.OfAsync.perform initialCounter () InitialCountLoaded
  initialModel, Cmd.none

let update (msg: Msg) (model: Model) =
  match msg, model.PageModel with
  | LoginMsg msg, LoginModel loginModel ->
      match msg with
      | Login.Msg.LoginSuccess newUser ->
          { model with PageModel = NotFoundModel }, Cmd.none

      | _ ->
        let newLoginModel, loginCmd = Login.update msg loginModel
        { model with PageModel = LoginModel newLoginModel }, Cmd.map LoginMsg loginCmd

  | LoginMsg _, _ -> model, Cmd.none

let urlUpdate (result:Page option) (model:Model) =
    match result with
    | None ->
        model, Cmd.none

    | Some Page.NotFound ->
        { model with PageModel = NotFoundModel }, Cmd.none

    | Some Page.Login ->
        let m = Login.initModel()// model.MenuModel.User
        { model with PageModel = LoginModel m }, Cmd.none

    | Some Page.Home ->
        { model with PageModel = NotFoundModel }, Cmd.none //Cmd.map HomePageMsg cmd

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

open Elmish.Navigation

Program.mkProgram init update view
|> Program.toNavigable urlParser urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
