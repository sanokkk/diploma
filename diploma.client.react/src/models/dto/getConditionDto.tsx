export class unitCondition {
    unitName: string;
    index: number;

    constructor(name: string, index: number) {
        this.unitName = name;
        this.index = index;
    }
}

export class conditionResponse {
    units: unitCondition[]

    constructor(units: unitCondition[]) {
        this.units = units;
    }
}

// export class conditionResponseWrapper {
//     response: conditionResponse;

//     constructor(response: conditionResponse) {
//         this.response = response;
//     }
// }