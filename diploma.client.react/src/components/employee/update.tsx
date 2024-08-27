import { ChangeEvent, Dispatch, SetStateAction, useEffect, useState } from "react";
import Employee from "../../models/employee";
import {
  Box,
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Modal,
  Select,
  SelectChangeEvent,
  TextField,
  Typography,
} from "@mui/material";
import { LevelToNotify } from "../../models/notifyLevel";
import { UpdateEmpoyeeDto } from "../../models/dto/updateEmployeeDto";
import axios from "axios";

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

export const UpdateEmployeeComponent = ({
  isOpen,
  setOpen,
  employee,
  conditions,
}: {
  isOpen: boolean;
  setOpen: Dispatch<SetStateAction<boolean>>;
  employee: Employee;
  conditions: LevelToNotify[];
}) => {
  const [email, setEmail] = useState<string>(employee.email);
  const [name, setName] = useState<string>(employee.name);
  const [notifyLevel, setLevel] = useState<number>(0);

  useEffect(() => {
    if (!employee)
        return

    setEmail(employee.email);
    setName(employee.name);
    setLevel(employee.notifyLevel);
  }, [employee])

  const handleUpdate = () => {
    var userToUpdate = new UpdateEmpoyeeDto(
      employee.id,
      email,
      name,
      notifyLevel
    );
    console.log(userToUpdate);

    try {
      axios
        //.patch("http://localhost:5072/v1/users", userToUpdate)
        .patch("http://localhost:5000/v1/users", userToUpdate)
        .then((response) => {
          console.log(response.status);

          setOpen(false);
          isOpen = false;
        });
    } catch (err) {
      console.log("error while updating user: " + err);
    }
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
          Изменение пользователя
        </Typography>
        <Box mb={2}>
          <TextField
            fullWidth={true}
            value={name}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setName(e.target.value as string)
            }
            label="Имя"
            type="text"
          />
        </Box>
        <Box mb={2} marginBottom={2}>
          <TextField
            fullWidth={true}
            value={email}
            onChange={(e: ChangeEvent<HTMLInputElement>) =>
              setEmail(e.target.value as string)
            }
            label="Почта"
            type="email"
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
              Состояние
            </InputLabel>
            <Select
              labelId="select-label"
              id="select"
              value={notifyLevel.toString()}
              label="Состояние"
              onChange={(e: SelectChangeEvent) => {
                let id = Number(e.target.value);
                setLevel(id);
              }}
            >
              {conditions.map((level, i) => (
                <MenuItem key={level.id} value={i}>
                  {level.value}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        <Box my={4} display="flex" alignItems="center" justifyContent="center">
          <Button variant="contained" onClick={handleUpdate}>
            Подтвердить
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
