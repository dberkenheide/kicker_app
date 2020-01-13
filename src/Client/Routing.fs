module Routing

open System
open Elmish.UrlParser

[<RequireQualifiedAccess>]
type Page =
  | Home
  | NotFound
  | Login
  | CreateNew
  | Preparation of int
  | Standing of int

let toPath =
  function
  | Page.Home -> "#home"
  | Page.Login -> "#login"
  | Page.NotFound -> "#not-found"
  | Page.CreateNew -> "#create-new"
  | Page.Preparation id -> "#prepare/" + (id |> string)
  | Page.Standing id -> "#standing/" + (id |> string)

/// The URL is turned into a Result.
let pageParser: Parser<Page -> Page, Page> =
  oneOf [
    map Page.Home (s "")
    map Page.Home (s "home")
    map Page.Login (s "login")
    map Page.CreateNew (s "create-new")
    map Page.NotFound (s "notfound")
    map Page.Preparation (s "prepare" </> i32)
    map Page.Standing (s "standing" </> i32)
  ]

let urlParser location =
  parseHash pageParser location