module GroupPhaseModel

type TeamPoint = {Name: string; Point: int}

type Group = {GroupName:string; Teams: TeamPoint list }

type Model = {Groups: Group list }

let group name =
    {
        GroupName= "Gruppe " + name
        Teams=
          [
            { Name="Team A"; Point=6 }
            { Name="Team B"; Point=3 }
            { Name="Team C"; Point=0 }
          ]
    }

let initialModel = { Groups = [group "A"; group "B"; group "C"] }