class Parameter {
    id: string
    parameterType: string
    maxValue: number
    minValue: number

    constructor(id: string, parameterType: string, maxVal: number, minVal: number) {
        this.id = id;
        this.parameterType = parameterType;
        this.minValue = minVal;
        this.maxValue = maxVal;
    }
}

export default Parameter;