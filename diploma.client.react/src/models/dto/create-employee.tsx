import { NotifyLevel } from "../notifyLevel";

class CreateEmployeeDto{
    Email: string;
    Name: string;
    NotifyLevel: NotifyLevel;
  
    constructor( email: string, name: string, notifyLevel: number) {
      this.Email = email;
      this.Name = name;
      this.NotifyLevel = notifyLevel as NotifyLevel;
    }
  }

  export default CreateEmployeeDto;