module LoginModel

type Model = { UserName: string option; Password : string option }

let hasEmtptyLoginFields model =
    model.UserName.IsNone || model.Password.IsNone