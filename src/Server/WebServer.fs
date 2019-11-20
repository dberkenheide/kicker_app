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

                return! if credentials |> Authenticator.validateUserCredentials then
                            ctx.WriteJsonAsync {UserName=credentials.UserName; Token=JsonWebToken.generateToken credentials.UserName}
                        else
                            Response.unauthorized ctx "Bearer" "" (sprintf "User '%s' can't be logged in." credentials.UserName)
            })
        forward "" secured
    }