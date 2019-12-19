module Routing

open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page =
  | Home
  | NotFound
  | Login

let toPath =
  function
  | Page.Home -> "#home"
  | Page.Login -> "#login"
  | Page.NotFound -> "#not-found"

  /// The URL is turned into a Result.
let pageParser : Parser<Page -> Page, Page> =
    oneOf
        [ map Page.Home (s "")
          map Page.Home (s "home")
          map Page.Login (s "login")
          map Page.NotFound (s "notfound") ]

let urlParser location =
  parseHash pageParser location