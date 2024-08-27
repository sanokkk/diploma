import axios from "axios";
import { ParameterType } from "../models/parameterType";
import { UnitType } from "../models/unitType";

export async function GetParameterTypesForUnitType(typeId: number): Promise<ParameterType[]> {
  let resp = new Array<ParameterType>();
  try {
    await axios
    //.get("http://localhost:5072/v1/enum/parameters/" + typeId)
      .get("http://localhost:5000/v1/enum/parameters/" + typeId)
      .then((response) => {
        let data = response.data as Object[];

        data.map((o) => {
          let converted = o as ParameterType;

          resp.push(converted);
        });
      });
  } catch (err) {
    console.log(err);
  }

  return resp;
}

export async function GetUnitTypes(): Promise<UnitType[]> {
  let resp = new Array<UnitType>();
  try {
    //await axios.get("http://localhost:5072/v1/enum/units").then((response) => {
    await axios.get("http://localhost:5000/v1/enum/units").then((response) => {
      let data = response.data as string[];
      return data.map((val, i) => {
        resp.push(new UnitType(i, val));
      });
    });
  } catch (err) {
    console.log(err);
    return [];
  }

  return resp;
}
