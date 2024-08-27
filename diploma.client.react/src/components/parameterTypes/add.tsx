import {
  ChangeEvent,
  Dispatch,
  SetStateAction,
  useEffect,
  useState,
} from "react";
import { UnitType } from "../../models/unitType";
import Modal from "@mui/material/Modal/Modal";
import {
  Grid,
  Typography,
  TextField,
  Select,
  MenuItem,
  FormControlLabel,
  Box,
  Slider,
  SelectChangeEvent,
  InputLabel,
  FormControl,
  Checkbox,
  Button,
} from "@mui/material";
import {
  CreateParameterDto,
  CreateParameterTypeDto,
} from "../../models/dto/create_unit";
import axios from "axios";
import { HandleSendCreateParameterType } from "../../utils/parameterTypesHttp";

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

export const AddParameterTypeComponent = ({
  unitTypes,
  isOpen,
  setOpen,
}: {
  unitTypes: UnitType[];
  setOpen: Dispatch<SetStateAction<boolean>>;
  isOpen: boolean;
}) => {
  //composing model
  const [parameterName, setParameterName] = useState<string>("");
  const [weight, setWeight] = useState<number>(1);
  const [unitType, setUnitType] = useState<number>(-1);
  const [isStatic, setIsStatic] = useState<boolean>(false);

  //others
  const [unitTypeString, setUnitTypeString] = useState<string>("Не выбрано");

  const handleSendParameterType = () => {
    let parameterToCreate = new CreateParameterTypeDto(
      parameterName,
      weight,
      unitType,
      isStatic
    );

    console.log(parameterToCreate);
    let result = HandleSendCreateParameterType(parameterToCreate);

    if (result) {
      console.log("zakrivau");
      setOpen(false);
      isOpen = false;
      return;
    }
    console.log("ne zakrivau");
  };

  return (
    <Modal
      open={isOpen}
      onClose={() => setOpen(false)}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Box sx={style}>
        <Typography
          textAlign={"center"}
          variant="h5"
          color="textPrimary"
          marginBottom={3}
        >
          Добавление типа параметра
        </Typography>
        <Box mb={2}>
          <TextField
            fullWidth={true}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setParameterName(e.target.value as string)
            }
            label="Название"
            type="text"
          />
        </Box>

        <Box mb={2}>
          <Grid container direction="row" alignItems="center">
            <Grid item xs={4}>
              <Typography variant="body1" color="textSecondary">
                Вес
              </Typography>
            </Grid>
            <Grid item xs={8}>
              <Slider
                onChange={(e: Event, newVal: number | number[]) =>
                  setWeight(newVal as number)
                }
                defaultValue={1}
                min={1}
                max={10}
                aria-label="Вес"
                valueLabelDisplay="auto"
              />
            </Grid>
          </Grid>
        </Box>

        <Box mb={2}>
          <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">Узел</InputLabel>
            <Select
              labelId="demo-simple-select-label"
              id="demo-simple-select"
              value={unitType.toString()}
              label="Узел"
              onChange={(event: SelectChangeEvent) => {
                console.log("change");
                let index = Number(event.target.value);
                console.log(index);
                setUnitType(index);
                if (index < 0) {
                  setUnitTypeString("Не выбрано");
                } else {
                  let type = unitTypes[Number(event.target.value)];
                  setUnitTypeString(type.name);
                }
              }}
            >
              <MenuItem value={-1}>Не выбрано</MenuItem>
              {unitTypes.map((type, i) => (
                <MenuItem key={type.id} value={i}>
                  {type.name}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>

        <Box mb={2}>
          <FormControlLabel
            value="start"
            control={
              <Checkbox
                onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                  setIsStatic(e.target.checked) //todo check why it gives true always
                }
              />
            }
            sx={{ marginLeft: 0 }}
            label="Статический"
            labelPlacement="start"
          />
        </Box>

        <Box my={4} display="flex" alignItems="center" justifyContent="center">
          <Button variant="contained" onClick={handleSendParameterType}>
            Создать
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
