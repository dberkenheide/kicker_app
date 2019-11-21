module GroupPhaseModel

type TeamPoint = {Id: int; Name: string; Point: int}

type Group = {Id: int; GroupName:string; Teams: TeamPoint list }

type Model = {Groups: Group list }