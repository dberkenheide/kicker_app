module ServerCode.WebServer

open ServerCode
//open ServerCode.ServerUrls
open Giraffe
open Saturn.Router
open Saturn.Pipeline
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2
open Shared
open Saturn

let webApp (str: string) =
    let startupTime = System.DateTime.UtcNow

    let secured = router {
        pipe_through (Saturn.Auth.requireAuthentication Saturn.ChallengeType.JWT)
    }

    router {
        not_found_handler (RequestErrors.NOT_FOUND "Page not found")
        get "/api/init" (fun next ctx ->
            task {
                
                return! json 5 next ctx
            })
            
        post "/api/login" (fun next ctx ->
            task {
                let! credentials = ctx.BindJsonAsync<Shared.Credentials>()

                let user = credentials |> Authenticator.authenticateWithLdap

                return! match user with
                        | Ok userName -> 
                            ctx.WriteJsonAsync {UserName=userName; Token=JsonWebToken.generateToken userName}

                        | Error error -> 
                            ctx.WriteJsonAsync {UserName=error; Token=JsonWebToken.generateToken error}//Response.unauthorized ctx "Bearer" "" error
            })
        forward "" secured
    }