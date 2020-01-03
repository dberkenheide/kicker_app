module Routing

open System
open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page =
  | Home
  | NotFound
  | Login
  | Standing of String

let toPath =
  function
  | Page.Home -> "#home"
  | Page.Login -> "#login"
  | Page.NotFound -> "#not-found"
  | Page.Standing uid -> "#standing/" + (uid)

/// The URL is turned into a Result.
let pageParser: Parser<Page -> Page, Page> =
  oneOf [
    map Page.Home (s "")
    map Page.Home (s "home")
    map Page.Login (s "login")
    map Page.NotFound (s "notfound")
    map Page.Standing (s "standing" </> str)
  ]

let urlParser location =
  parseHash pageParser location