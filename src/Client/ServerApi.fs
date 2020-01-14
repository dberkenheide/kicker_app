module ServerApi

open Shared.Dtos
open Shared.Apis

open Fable.Remoting.Client

/// A proxy you can use to talk to server directly
let api: IApi =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Route.builder
  |> Remoting.buildProxy<IApi>