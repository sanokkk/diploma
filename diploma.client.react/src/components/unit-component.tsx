import { useEffect, useState } from "react";
import { Unit } from "../models/unit";
import {
  Box,
  Button,
  Card,
  CardActions,
  CardContent,
  Input,
  Typography,
} from "@mui/material";
import { CreateParameterDto, CreateUnitDto } from "../models/dto/create_unit";
import { HandleGetUnits } from "../utils/unitsHttp";
import AddIcon from "@mui/icons-material/Add";
import { AddUnitComponent } from "./unit/add";
import axios from "axios";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
  overflow: "auto",
  height: "80vh",
};

export const UnitComponent = () => {
  //objects
  const [units, setUnits] = useState<Unit[]>([]);
  const [file, setFile] = useState<File>();

  //toCreate
  const [isOpenAdd, setIsOpenAdd] = useState<boolean>(false);

  //toUpdate

  useEffect(() => {
    //if (units.length != 0) return;

    const callHandleGetUnits = async () => {
      await handleSetUnits();
    };

    callHandleGetUnits();
  }, [setUnits, setIsOpenAdd]);

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
        .post("http://localhost:5000/v1/unit/file", formData)
        .then((resp) => {
          console.log(resp.status);
        })
        .catch((err) => console.log(err));
    } catch (err) {
      console.log("error while sending file: ", err);
    }
  }

  async function handleDeleteUnit(id: string) {
    let successDelete = false;
    try {
      axios
        //.delete("http://localhost:5072/v1/unit/" + id)
        .delete("http://localhost:5000/v1/unit/" + id)
        .then((resp) => {
          if (resp.status === 200) {
            successDelete = true;
          }
        })
        .catch((err) => {
          console.log("error while deleting unit: ", err);
        });
    } catch (err) {
      console.log("error while deleting unit: ", err);
    }

    setUnits(units.filter((x) => x.id != id));
  }

  async function handleSetUnits() {
    try {
      await axios
      //.get("http://localhost:5072/v1/unit")
        .get("http://localhost:5000/v1/unit")
        .then((resp) => {
          let newUnits = resp.data as Unit[];
          console.log("setted units with lenght of ", newUnits.length);
          setUnits(newUnits);
        })
        .catch((err) => {
          console.log("error while getting units: ", err);
        });
    } catch (err) {
      console.log("error while getting units: ", err);
    }
  }

  return (
    <Box>
      <Typography textAlign={"center"} color={"rgb(74, 68, 45)"}>
        <h2>Список узлов</h2>
      </Typography>

      <Box
        marginLeft={2}
        marginRight={2}
        flexWrap={"wrap"}
        display={"grid"}
        gridTemplateColumns={"repeat(2, minmax(0, 1fr))"}
        padding={2}
      >
        {units.map((unit, i) => {
          return (
            <Box key={i} width={"95%"} margin={1}>
              <Card
                sx={{ width: "100%", backgroundColor: "rgb(88, 176, 156)" }}
                key={unit.id}
                variant="outlined"
              >
                <CardContent>
                  <Typography
                    fontWeight={"bold"}
                    fontFamily={"Poppins"}
                    fontSize={"1.5em"}
                    textAlign={"center"}
                  >
                    Узел "{unit.name}"
                  </Typography>
                  <Typography
                    fontFamily={"Poppins"}
                    fontSize={"1.5em"}
                    textAlign={"center"}
                  >
                    <strong>Тип узла:</strong> {unit.unitType}
                  </Typography>
                  <hr></hr>
                  {unit.parameters.map((param, i) => {
                    return (
                      <Box>
                        <Typography
                          fontFamily={"Poppins"}
                          fontSize={"1.5em"}
                          textAlign={"center"}
                        >
                          <strong>Параметр:</strong> "{param.parameterType}"
                        </Typography>
                        <Typography
                          fontFamily={"Poppins"}
                          fontSize={"1.5em"}
                          textAlign={"center"}
                        >
                          Диапазон: {param.minValue} - {param.maxValue}
                        </Typography>
                        <hr></hr>
                      </Box>
                    );
                  })}
                </CardContent>
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
                      size={"large"}
                      color="error"
                      sx={{ fontWeight: "bold" }}
                      onClick={() => {
                        console.log(unit.id);
                        handleDeleteUnit(unit.id);
                      }}
                    >
                      Удалить
                    </Button>
                  </Box>
                </CardActions>
              </Card>
            </Box>
          );
        })}
      </Box>

      <Box
        display={"flex"}
        justifyContent={"center"}
        marginTop={2}
        marginBottom={2}
      >
        <Button
          variant="contained"
          onClick={() => {
            setIsOpenAdd(true);
          }}
        >
          <AddIcon />
        </Button>
      </Box>
      <AddUnitComponent isOpen={isOpenAdd} setOpen={setIsOpenAdd} />

      <Box marginTop={2} marginBottom={2}>
        <Box marginBottom={2}>
          <Typography
            textAlign={"center"}
            color={"rgb(74, 68, 45)"}
            fontWeight={"bold"}
            variant="body1"
          >
            Загрузка узлов из XML-файла
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
