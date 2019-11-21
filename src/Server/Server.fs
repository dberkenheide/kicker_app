module ServerCode.Program

open System.IO
open System
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Thoth.Json.Giraffe
open Saturn
open Saturn.Application
open Shared

let login credentials =
    ()


let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

let app = application {
    use_router (WebServer.webApp "db-String")
    url ("http://0.0.0.0:" + port.ToString() + "/")    
    use_jwt_authentication JsonWebToken.secret JsonWebToken.issuer
    use_static publicPath
    use_gzip
    use_cors "Allow_Configured_Cors" (fun builder ->         
        builder.AllowAnyMethod() |> ignore
        builder.AllowAnyOrigin() |> ignore
        builder.AllowAnyHeader() |> ignore
        ())
}
run app

    //memory_cache
    //use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())

