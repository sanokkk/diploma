import { UUID } from "crypto";
import { NotifyLevel } from "./notifyLevel";

class Employee {
  id: UUID;
  email: string;
  name: string;
  notifyLevel: NotifyLevel;

  constructor(id: UUID, email: string, name: string, notifyLevel: number) {
    this.email = email;
    this.id = id;
    this.name = name;
    this.notifyLevel = notifyLevel as NotifyLevel;
  }
}



export default Employee;
