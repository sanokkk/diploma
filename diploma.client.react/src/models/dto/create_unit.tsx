export class CreateParameterDto {
  parameterTypeId: number; 
  minValue: number;
  maxValue: number;

  constructor(parameterTypeId: number, minValue: number, maxValue: number) {
    this.parameterTypeId = parameterTypeId;
    this.maxValue = maxValue;
    this.minValue = minValue;
  }
}

export class CreateUnitDto {
  type: number;
  name: string;
  parameters: CreateParameterDto[];

  constructor(type: number, name: string, parameters: CreateParameterDto[]) {
    this.type = type;
    this.name = name;
    this.parameters = parameters;
  }
}

export class CreateParameterTypeDto {
  Name: string;
  Weight: number;
  UnitType: number;
  IsStatic: boolean;

  constructor(
    name: string,
    weight: number,
    unitType: number,
    isStatic: boolean
  ) {
    this.Name = name;
    this.Weight = weight;
    this.UnitType = unitType;
    this.IsStatic = isStatic;
  }
}
