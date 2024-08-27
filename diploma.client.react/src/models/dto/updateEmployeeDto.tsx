import { UUID } from "crypto";
import { NotifyLevel } from "../notifyLevel";

export class UpdateEmpoyeeDto {
  Id: UUID;
  Email: string;
  Name: string;
  NotifyLevel: NotifyLevel;

  constructor(id: UUID, email: string, name: string, notifyLevel: number) {
    this.Id = id;
    this.Email = email;
    this.Name = name;
    this.NotifyLevel = notifyLevel as NotifyLevel;
  }
}
