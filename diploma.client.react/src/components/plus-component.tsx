import { useState } from "react";
import EmployeeComponent from "./employee-component";
import { UnitComponent } from "./unit-component";
import { ParameterTypeComponent } from "./parameterTypeComponent";
import { Box, Button, Link, Menu, MenuItem } from "@mui/material";
import { ConditionComponent } from "./conditionComponent";

const PlusComponent = () => {
  const [selectedValue, setSelected] = useState<string>("");

  function handleSelectPage(event: React.ChangeEvent<HTMLSelectElement>) {
    setSelected(event.target.value);
  }

  function getComponent(value: string): any {
    switch (value) {
      case "0":
        return <ConditionComponent />;
      case "1":
        return <EmployeeComponent />;
      case "2":
        return <UnitComponent />;
      case "3":
        return <ParameterTypeComponent />;
    }
  }

  return (
    <div>
      <Box
        borderBottom={"solid black 1px"}
        display={"flex"}
        justifyContent="center"
        gap={2}
        sx={{ backgroundColor: "rgb(88, 176, 156)" }}
      >
        <Button
          sx={{ color: "rgb(74, 68, 45)", fontSize: "1.5em" }}
          size="large"
          component="button"
          onClick={() => setSelected("0")}
        >
          Состояние
        </Button>
        <Button
          sx={{ color: "rgb(74, 68, 45)", fontSize: "1.5em" }}
          size="large"
          component="button"
          onClick={() => setSelected("1")}
        >
          Сотрудники
        </Button>
        <Button
          size="large"
          sx={{ color: "rgb(74, 68, 45)", fontSize: "1.5em" }}
          component="button"
          onClick={() => setSelected("2")}
        >
          Узлы
        </Button>
        <Button
          sx={{ color: "rgb(74, 68, 45)", fontSize: "1.5em" }}
          size="large"
          component="button"
          onClick={() => setSelected("3")}
        >
          Параметры
        </Button>
      </Box>
      <div>{getComponent(selectedValue)}</div>
    </div>
  );
};

export default PlusComponent;

/*
<select
          value={selectedValue}
          defaultValue={""}
          onChange={handleSelectPage}
        >
          <option value=""></option>
          <option value="1">Сотрудники</option>
          <option value="2">Узлы</option>
          <option value="3">Параметры</option>
        </select>
*/
