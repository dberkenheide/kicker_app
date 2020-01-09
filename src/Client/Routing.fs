module Routing

open System
open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page =
  | Home
  | NotFound
  | Login
  | CreateNew
  | Standing of String

let toPath =
  function
  | Page.Home -> "#home"
  | Page.Login -> "#login"
  | Page.NotFound -> "#not-found"
  | Page.CreateNew -> "#create-new"
  | Page.Standing uid -> "#standing/" + (uid)

/// The URL is turned into a Result.
let pageParser: Parser<Page -> Page, Page> =
  oneOf [
    map Page.Home (s "")
    map Page.Home (s "home")
    map Page.Login (s "login")
    map Page.CreateNew (s "create-new")
    map Page.NotFound (s "notfound")
    map Page.Standing (s "standing" </> str)
  ]

let urlParser location =
  parseHash pageParser location