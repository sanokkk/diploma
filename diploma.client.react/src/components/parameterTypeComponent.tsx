import { SetStateAction, useEffect, useLayoutEffect, useState } from "react";
import { ParameterType } from "../models/parameterType";
import {
  Box,
  Button,
  Card,
  CardActions,
  Fab,
  Grid,
  Input,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
  Typography,
  autocompleteClasses,
} from "@mui/material";
import { UnitType } from "../models/unitType";
import { GetParameterTypesForUnitType, GetUnitTypes } from "../utils/enums";
import axios from "axios";
import { CardBody, CardHeader } from "react-bootstrap";
import { AddParameterTypeComponent } from "./parameterTypes/add";
import AddIcon from "@mui/icons-material/Add";
import { HandleDeleteParameterType } from "../utils/parameterTypesHttp";
import { MuiFileInput } from "mui-file-input";

export const ParameterTypeComponent = () => {
  const [types, setTypes] = useState<ParameterType[]>([]);
  const [unitType, setUnitType] = useState<number>(-1);
  const [unitTypes, setUnitTypes] = useState<UnitType[]>([]);
  const [openAdd, setOpenAdd] = useState<boolean>(false);
  const [file, setFile] = useState<File>();

  useEffect(() => {
    if (unitTypes.length == 0) {
      const fetchUnitTypes = async () => {
        let resp = new Array<UnitType>();
        try {
          await axios
            //.get("http://localhost:5072/v1/enum/units")
            .get("http://localhost:5000/v1/enum/units")
            .then((response) => {
              let data = response.data as string[];
              return data.map((val, i) => {
                resp.push(new UnitType(i, val));
              });
            });
        } catch (err) {
          console.log(err);
          return [];
        }
        setUnitTypes(resp);
      };

      fetchUnitTypes();
      console.log("запрашиваю узлы");
    }
    setTypes(types);
  }, [types, openAdd]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const fileList = e.target.files;
    if (!fileList) return;
    let newFile = fileList[0];
    if (!newFile) return;
    setFile(newFile);
  };

  async function handleSendFile() {
    const formData = new FormData();
    let upl = file;
    if (!upl) return;
    formData.append("file", upl);
    try {
      axios
        .post("http://localhost:5000/v1/parameter-type/file", formData)
        .then((resp) => {
          console.log(resp.status);
        })
        .catch((err) => console.log(err));
    } catch (err) {
      console.log("error while sending file: ", err);
    }
  }

  const handleUpdateUnitType = (e: SelectChangeEvent<number | "">) => {
    setUnitType(e.target.value as number);

    let parameters = async () => {
      let parameters = await GetParameterTypesForUnitType(
        e.target.value as number
      );

      setTypes(parameters);
      console.log(JSON.stringify(types) + "params from state");
    };
    parameters();
  };

  function deleteParameterType(id: number) {
    HandleDeleteParameterType(id);
    setTypes(types.filter((x) => x.id != id));
  }

  function getAddButton() {
    return unitType === -1 ? (
      ""
    ) : (
      <Button
        variant="contained"
        onClick={() => {
          setOpenAdd(true);
          console.log(openAdd);
        }}
      >
        <AddIcon />
      </Button>
    );
  }

  return (
    <Box>
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
        }}
      >
        <Typography textAlign={"center"} color={"rgb(74, 68, 45)"}>
          <h2>Типы параметров для узлов</h2>
        </Typography>

        <Select
          value={unitType}
          onChange={handleUpdateUnitType}
          style={{
            width: "fit-content",
            margin: 20,
            padding: 10,
            borderRadius: 10,
            border: "1px solid #ccc",
            backgroundColor: "rgb(88, 176, 156)",
            color: "rgb(74, 68, 45)",
            fontSize: "1.4em",
          }}
        >
          <MenuItem value={-1}>Выберите тип узла</MenuItem>
          {unitTypes.map((val) => (
            <MenuItem value={val.id} key={val.id}>
              {val.name}
            </MenuItem>
          ))}
        </Select>
        <Grid
          sx={{ flexGrow: 1 }}
          container
          spacing={2}
          marginLeft={2}
          marginRight={2}
        >
          {types.map((t) => (
            <Grid item xs={3}>
              <Card
                sx={{ width: "95%", backgroundColor: "rgb(88, 176, 156)" }}
                key={t.id}
                variant="outlined"
              >
                <Box display={"flex"} justifyContent={"center"}>
                  <Typography
                    fontFamily={"Poppins"}
                    fontSize={"1.8em"}
                    textAlign={"center"}
                  >
                    {t.parameterType}
                  </Typography>
                </Box>
                <CardBody>
                  <Box display={"flex"} justifyContent={"space-around"}>
                    <Typography fontFamily={"Poppins"} fontSize={"1.5em"}>
                      {t.isStatic ? "Статический" : "Измеряемый"}
                    </Typography>

                    <Typography fontFamily={"Poppins"} fontSize={"1.5em"}>
                      Вес: {t.weight}
                    </Typography>
                  </Box>
                </CardBody>
                <CardActions>
                  <Box
                    display={"flex"}
                    justifyContent={"center"}
                    width={"100%"}
                  >
                    <Button
                      size="large"
                      color="info"
                      sx={{ fontWeight: "bold" }}
                    >
                      Изменить
                    </Button>
                    <Button
                      sx={{ fontWeight: "bold" }}
                      size="large"
                      color="error"
                      onClick={() => deleteParameterType(t.id)}
                    >
                      Удалить
                    </Button>
                  </Box>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
        <Box marginTop={2}>{getAddButton()}</Box>
        <AddParameterTypeComponent
          isOpen={openAdd}
          setOpen={setOpenAdd}
          unitTypes={unitTypes}
        ></AddParameterTypeComponent>
      </div>
      <Box marginTop={2} marginBottom={2}>
        <Box marginBottom={2}>
          <Typography
            textAlign={"center"}
            color={"rgb(74, 68, 45)"}
            fontWeight={"bold"}
            variant="body1"
          >
            Загрузка типов из XML-файла
          </Typography>
        </Box>
        <Box display={"flex"} justifyItems={"center"} alignContent={"center"} alignItems={"center"} justifyContent={'center'}>
          <Input type="file" onChange={handleChange} />
          <Button
            component="span"
            variant="contained"
            onClick={handleSendFile}
            sx={{ width: "fit-content" }}
          >
            Отправить
          </Button>
        </Box>
      </Box>
    </Box>
  );
};
