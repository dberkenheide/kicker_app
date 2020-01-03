module AuthApi

open Shared.Dtos
open Shared.Apis

let (authApi: IAuthApi) = {
  login = fun login -> async { return { UserName= "Test"; Token= "42" } }
}
