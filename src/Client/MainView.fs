module MainView

open Elmish
open Elmish.Navigation
open Fable.React
open Fable.React.Props
open Fulma

open Shared
open Routing

type PageModel =
  | NotFoundModel
  | HomeModel of string
  | LoginModel of Login.Model
  | StandingModel of Standing.Model

type Model = {
  User: UserData option
  PageModel: PageModel
}

type Msg =
  | LoginMsg of Login.Msg
  | StandingMsg of Standing.Msg

let update (msg: Msg) (model: Model) =
  let navigateTo p m =
    m, (p |> toPath |> Navigation.newUrl)

  match msg, model.PageModel with
  | LoginMsg msg, LoginModel loginModel ->
      match msg with
      | Login.Msg.LoginSuccess newUser ->
          navigateTo Page.Home { model with User = Some newUser }

      | Login.Msg.Intern internMsg ->
          let newLoginModel, loginCmd = Login.update internMsg loginModel
          { model with PageModel = LoginModel newLoginModel }, Cmd.map LoginMsg loginCmd

  | LoginMsg _, _ -> model, Cmd.none

  | StandingMsg msg, StandingModel standingModel -> model, Cmd.none

  | StandingMsg _, _ -> model, Cmd.none

let centerStyle direction =
  Style [ Display DisplayOptions.Flex
          FlexDirection direction
          AlignItems AlignItemsOptions.Center
          JustifyContent "center"
          Padding "20px 0"
  ]

let view (model: Model) (dispatch : Msg -> unit) =
  div [] [
    div [] [
      Menu.view model.User
    ]
    div [centerStyle "column" ] [
      match model.PageModel with
      | LoginModel loginModel ->
          yield Login.view loginModel (LoginMsg >> dispatch)

      | HomeModel name ->
          yield div [] [ str name ]

      | StandingModel standingModel ->
          yield Standing.view standingModel (StandingMsg >> dispatch)

      | NotFoundModel ->
          yield div [] [ str "The page is not available." ]
    ]
  ]

let urlUpdate (result : Page option) (model:Model) =
  match result with
  | None ->
      model, Cmd.none

  | Some Page.NotFound ->
      { model with PageModel = NotFoundModel }, Cmd.none

  | Some Page.Login ->
      let m, c = Login.initModel ()
      { model with PageModel = LoginModel m }, c |> Cmd.map LoginMsg

  | Some Page.Home ->
      { model with PageModel = HomeModel "Can GET/Home" }, Cmd.none //Cmd.map HomePageMsg cmd

  | Some (Page.Standing s) ->
      let m, c = Standing.initModel s
      { model with PageModel = StandingModel m }, c |> Cmd.map LoginMsg

let init (page: Page option) : Model * Cmd<Msg> =
  let initialModel = {
    User = None
    PageModel = NotFoundModel
  }
  //let loadCountCmd =
  //  Cmd.OfAsync.perform initialCounter () InitialCountLoaded
  urlUpdate page initialModel
