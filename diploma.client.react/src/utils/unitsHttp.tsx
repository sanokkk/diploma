import axios from "axios";
import { Unit } from "../models/unit";

export async function HandleGetUnits(): Promise<Unit[]> {
  let units = new Array<Unit>();
  console.log("request units");

  try {
    //await axios.get("http://localhost:5072/v1/unit").then((response) => {
    await axios.get("http://localhost:5000/v1/unit").then((response) => {
      let newUnits = response.data as Unit[];
      console.log("from response converted");
      console.log(newUnits);
      units = [...newUnits];
    });
  } catch (err) {
    console.log("error while getting units: " + err);
    return units;
  }

  return units;
}
