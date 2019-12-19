module MainView

open Elmish
open Fable.React
open Fable.React.Props
open Fulma

open Routing

type PageModel =
  | NotFoundModel
  | HomeModel of string
  | LoginModel of Login.Model

type Model = {
  PageModel : PageModel
}

type Msg =
  | LoginMsg of Login.Msg

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

      | HomeModel name ->
          yield div [] [ str name ]

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
        let m = Login.initModel()// model.MenuModel.User
        { model with PageModel = LoginModel m }, Cmd.none

    | Some Page.Home ->
        { model with PageModel = HomeModel "Can GET/Home" }, Cmd.none //Cmd.map HomePageMsg cmd

let init (page: Page option) : Model * Cmd<Msg> =
  let initialModel = { PageModel = NotFoundModel }
  //let loadCountCmd =
  //  Cmd.OfAsync.perform initialCounter () InitialCountLoaded
  urlUpdate page initialModel
