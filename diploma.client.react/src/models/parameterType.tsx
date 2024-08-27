export class ParameterType {
  id: number;
  parameterType: string;
  unitType: number;
  isStatic: boolean;
  weight: number;

  constructor(
    id: number,
    parameterType: string,
    unitType: number,
    isStatic: boolean,
    weight: number
  ) {
    this.id = id;
    this.parameterType = parameterType;
    this.unitType = unitType;
    this.isStatic = isStatic;
    this.weight = weight;
  }
}
