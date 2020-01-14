module Server

open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn

open SqlProvider

open Fable.Remoting.Server
open Fable.Remoting.Giraffe

let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let dbPath = Path.GetFullPath "../Database"

let port =
  "SERVER_PORT"
  |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let createApi (ctx) =
  Remoting.createApi()
  |> Remoting.withRouteBuilder Shared.Apis.Route.builder
  |> Remoting.fromValue (Api.createApi ctx)
  |> Remoting.buildHttpHandler


let app (ctx) = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router (createApi ctx)
    memory_cache
    use_static publicPath
    use_gzip
}

do
  let ctx = SqlProvider.ctx

  run (app ctx)
