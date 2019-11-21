module Authenticator

open Shared
open System
open Novell.Directory.Ldap

let private memberOfAttribute = "memberOf"
let private displayNameAttribute = "dsiplayName"
let private samAccountNameAttribute = "sAMAccountName"

let connectToLdapWithUserCredentials password (connection :LdapConnection) (result : ILdapSearchResults) =
    let user = result.Next()

    if (user |> isNull) then
        Error "Authenticate failed."
    else
        connection.Bind(user.Dn, password)

        if (connection.Bound) then
            Ok (user.GetAttribute(samAccountNameAttribute).StringValue)
        else
            Error "Authenticate failed."

let findLdapUserWithGivenUserName username (connection:LdapConnection) =
    let searchFilter = sprintf "(&(objectClass=User)(sAMAccountName=%s))" username
    let searchBase = "ou=users,ou=company,dc=lmis,dc=de"
    let attributes = [|memberOfAttribute; displayNameAttribute; samAccountNameAttribute|]
    connection.Search(searchBase, LdapConnection.ScopeSub, searchFilter, attributes, false)

let authenticateWithLdap (credentials: Credentials) =
    use connection = new LdapConnection()

    connection.Connect("192.168.3.74", 389)
    connection.StartTls |> ignore

    connection.Bind("XXX", "XXX") |> ignore

    let connectedUser = findLdapUserWithGivenUserName credentials.UserName connection
                       |> connectToLdapWithUserCredentials credentials.Password connection

    connection.StopTls |> ignore
    connection.Disconnect() |> ignore

    connectedUser

let connectDebug (credentials: Credentials) =
    if (credentials.UserName = "Test") && (credentials.Password = "Test") then
        Ok "Test"
    else
        credentials |> authenticateWithLdap