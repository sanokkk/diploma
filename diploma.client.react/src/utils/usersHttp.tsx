import axios from "axios";
import { UUID } from "crypto";

export function HandleDeleteUser(id: UUID) {
  try {
    //axios.delete("http://localhost:5072/v1/users/" + id);
    axios.delete("http://localhost:5000/v1/users/" + id);
  } catch (err) {
    console.log("error while deleting user: ", err);
  }
}
