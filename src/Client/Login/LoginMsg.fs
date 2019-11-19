module LoginMsg

type Msg =
    | LoginClick
    | SignUpClick
    | ForgotPasswordClick
    | UserChanged of string option
    | PasswordChanged of string option