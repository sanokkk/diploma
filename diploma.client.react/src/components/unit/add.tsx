import {
  ChangeEvent,
  Dispatch,
  SetStateAction,
  useCallback,
  useEffect,
  useState,
} from "react";
import { UnitType } from "../../models/unitType";
import {
  CreateParameterDto,
  CreateUnitDto,
} from "../../models/dto/create_unit";
import { ParameterType } from "../../models/parameterType";
import axios from "axios";
import {
  Box,
  Button,
  FormControl,
  Grid,
  InputLabel,
  MenuItem,
  Modal,
  Select,
  SelectChangeEvent,
  Slider,
  TextField,
  Typography,
} from "@mui/material";
import { GetParameterTypesForUnitType } from "../../utils/enums";

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 500,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

export const AddUnitComponent = ({
  isOpen,
  setOpen,
}: {
  isOpen: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
}) => {
  //to Create
  const [unitTypes, setUnitTypes] = useState<UnitType[]>([]);
  const [unitParameters, setUnitParameters] = useState<ParameterType[]>(
    new Array<ParameterType>()
  );

  //to send post
  const [unitType, setUnitType] = useState<number>(-1);
  const [name, setName] = useState<string>("");
  const [parameters, setParameters] = useState<CreateParameterDto[]>(
    new Array<CreateParameterDto>()
  );

  const handleSetUnitParams = useCallback(
    async (id: number) => {
      let parametersForUnit = await GetParameterTypesForUnitType(id);

      setUnitParameters(parametersForUnit);
      console.log("unit params:");
      console.log(unitParameters);
      let newParams = new Array<CreateParameterDto>();

      parametersForUnit.forEach((param) => {
        newParams.push(new CreateParameterDto(param.id, 0, 10));
        setParameters(newParams);
      });
    },
    [setUnitParameters, setParameters]
  );

  useEffect(() => {
    if (unitType === -1) return;
    handleSetUnitParams(unitType);
  }, [unitType, handleSetUnitParams]);

  useEffect(() => {
    handleSetUnitTypes();
  }, []);

  const handleUnitTypeSelect = async (e: SelectChangeEvent) => {
    let id = Number(e.target.value);
    if (id <= -1) {
      setParameters([]);
      setUnitParameters([]);
      return;
    }
    setUnitType(id);
  };

  function handleSetUnitTypes() {
    //console.log("getting unit types");
    let resp = new Array<UnitType>();
    try {
      //axios.get("http://localhost:5072/v1/enum/units").then((response) => {
      axios.get("http://localhost:5000/v1/enum/units").then((response) => {
        let data = response.data as string[];
        return data.map((val, i) => {
          resp.push(new UnitType(i, val));
        });
      });
    } catch (err) {
      console.log(err);
    }
    setUnitTypes(resp);
  }

  async function handleSendUnit() {
    let unitToSend = new CreateUnitDto(unitType, name, parameters);
    let newIsOpen = true;
    try {
      axios
        //.post("http://localhost:5072/v1/unit", unitToSend)

        .post("http://localhost:5000/v1/unit", unitToSend)
        .then((resp) => {
          newIsOpen = resp.status > 300;
          console.log("after checking status: ", newIsOpen, resp.status);
        })
        .catch((err) => {
          newIsOpen = true;
        });
      newIsOpen = false;
    } catch (err) {
      console.log("error while sending unit: ", err);
      newIsOpen = true;
    }

    setOpen(newIsOpen);
    isOpen = newIsOpen;
  }

  return (
    <Modal
      open={isOpen}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
      onClose={() => setOpen(false)}
    >
      <Box sx={style}>
        <Typography
          textAlign={"center"}
          variant="h5"
          color="textPrimary"
          marginBottom={3}
        >
          Добавление узла
        </Typography>
        <Box mb={2}>
          <TextField
            fullWidth={true}
            value={name}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setName(e.target.value as string)
            }
            label="Название"
            type="text"
          />
        </Box>

        <Box mb={2}>
          <FormControl fullWidth>
            <InputLabel
              shrink
              margin="dense"
              htmlFor="select"
              id="select-label"
            >
              Узел
            </InputLabel>
            <Select
              labelId="select-label"
              id="select"
              value={unitType.toString()}
              label="Состояние"
              onChange={handleUnitTypeSelect}
            >
              <MenuItem key={-1} value={-1}>
                Не выбрано
              </MenuItem>
              {unitTypes.map((unit, i) => (
                <MenuItem key={unit.id} value={i}>
                  {unit.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        <Box>
          {parameters.map((parameter, i) => {
            return (
              <Box key={i} mb={2}>
                <Grid container direction="row" alignItems="center">
                  <Grid item xs={4}>
                    <Typography variant="body1" color="textSecondary">
                      Диапазон значений для{" "}
                      {
                        unitParameters.find(
                          (x) => x.id == parameter.parameterTypeId
                        )?.parameterType
                      }
                    </Typography>
                  </Grid>
                  <Grid item xs={8}>
                    <Slider
                      step={0.1}
                      min={-100}
                      max={200}
                      getAriaLabel={() => "Minimum distance shift"}
                      value={[parameter.minValue, parameter.maxValue]}
                      onChange={(event: Event, newValue: number | number[]) => {
                        if (!Array.isArray(newValue)) return;
                        parameter.minValue = newValue[0];
                        parameter.maxValue = newValue[1];
                        const newParams = [...parameters];
                        newParams[i] = parameter;
                        setParameters(newParams);
                      }}
                      valueLabelDisplay="auto"
                      disableSwap
                    />
                  </Grid>
                </Grid>
              </Box>
            );
          })}
        </Box>
        <Box
          my={4}
          display="flex"
          alignItems="center"
          justifyContent="center"
          marginBottom={0}
        >
          <Button variant="contained" onClick={handleSendUnit}>
            Подтвердить
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
