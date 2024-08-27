import Parameter from "./parameter"

export class Unit{
    id: string
    name: string
    unitType: string
    parameters: Parameter[]

    constructor(id: string, name: string, unitType: string, condition: string, parameters: Parameter[]){
        this.id = id;
        this.name = name;
        this.unitType = unitType;
        this.parameters = parameters;
    }
} 