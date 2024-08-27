import axios from "axios";
import { CreateParameterTypeDto } from "../models/dto/create_unit";
import { useEffect, useState } from "react";

export function HandleSendCreateParameterType(model: CreateParameterTypeDto): boolean {
    try {
      //axios.post("http://localhost:5072/v1/parameter-type", model);
      axios.post("http://localhost:5000/v1/parameter-type", model);
      return true;
    } catch (err) {
      console.log("error while send parameterType: " + err);
      return false;
    }
  }

  export function HandleDeleteParameterType(id: number) {
    try {
      //axios.delete("http://localhost:5072/v1/parameter-type/" + id);
      axios.delete("http://localhost:5000/v1/parameter-type/" + id);
      return true;
    } catch (err) {
      console.log("error while send parameterType: " + err);
      return false;
    }
  }
