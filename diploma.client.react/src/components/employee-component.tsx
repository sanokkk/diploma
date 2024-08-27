import Employee from "../models/employee";
import { ChangeEvent, useEffect, useState } from "react";
import axios from "axios";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Modal from "@mui/material/Modal";
import CreateEmployeeDto from "../models/dto/create-employee";
import {
  Card,
  CardActions,
  CardContent,
  FormControl,
  Grid,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
  TextField,
} from "@mui/material";
import AddIcon from "@mui/icons-material/Add";
import { HandleDeleteUser } from "../utils/usersHttp";
import { UUID, randomUUID } from "crypto";
import {
  GetNotifyLevels,
  LevelToNotify,
  NotifyLevel,
} from "../models/notifyLevel";
import { UpdateEmployeeComponent } from "./employee/update";

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

const EmployeeComponent = () => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [open, setOpen] = useState<boolean>(false);
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);
  const [email, setEmail] = useState<string>("none");
  const [name, setName] = useState<string>("none");
  const [notifyLevel, setLevel] = useState<number>(0);
  const [conditionStates, setStates] = useState<LevelToNotify[]>([]);
  const [openUpdate, setOpenUpdate] = useState<boolean>(false);
  const [employeeToUpdate, setEmployeeToUpdate] = useState<Employee>(
    new Employee(crypto.randomUUID() as UUID, "", "", 0)
  );
  useEffect(() => {
    handleGetEmployees();
    setStates(GetNotifyLevels());
    console.log(conditionStates);
  }, [open, setEmployees, openUpdate]);

  async function handleGetEmployees() {
    axios
      //.get("http://localhost:5072/v1/users")
      .get("http://localhost:5000/v1/users")
      .then((response) => {
        const json = response.data;
        const employees: Employee[] = json.map(
          (obj: any) =>
            new Employee(obj.id, obj.email, obj.name, obj.notifyLevel)
        );
        console.log(employees);
        setEmployees(employees);
      })
      .catch((err) => console.log(err));
  }

  async function handleSend() {
    var userToCreate = new CreateEmployeeDto(email, name, notifyLevel);
    console.log(userToCreate);

    axios
      //.post("http://localhost:5072/v1/users", userToCreate)
      .post("http://localhost:5000/v1/users", userToCreate)
      .then((response) => {
        console.log(response.status);

        if (response.status === 204) {
          handleClose();
        }
      });
  }

  async function handleDeleteUser(id: UUID) {
    await HandleDeleteUser(id);
    setEmployees(employees.filter((x) => x.id != id));
  }

  function handleUpdateUser(user: Employee) {
    console.log("open update");
    setOpenUpdate(true);
    setEmployeeToUpdate(user);
  }

  return (
    <div>
      <Typography textAlign={"center"} color={"rgb(74, 68, 45)"}>
        <h2>Список сотрудников</h2>
      </Typography>
      <Modal
        open={open}
        onClose={handleClose}
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
            Добавление пользователя
          </Typography>
          <Box mb={2}>
            <TextField
              fullWidth={true}
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
                {conditionStates.map((level, i) => (
                  <MenuItem key={level.id} value={i}>
                    {level.value}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Box>
          <Box
            my={4}
            display="flex"
            alignItems="center"
            justifyContent="center"
          >
            <Button variant="contained" onClick={handleSend}>
              Создать
            </Button>
          </Box>
        </Box>
      </Modal>
      <Box marginLeft={7} marginRight={2}>
        <Grid sx={{ flexGrow: 2 }} container spacing={2}>
          {employees.map((el) => (
            <Grid key={el.id} item xs={4}>
              <Card
                sx={{ width: "100%", backgroundColor: "rgb(88, 176, 156)" }}
                key={el.id}
                variant="outlined"
              >
                <CardContent>
                  <Typography fontFamily={"Poppins"} fontSize={"1.5em"}>
                    Сотрудник {el.name}
                  </Typography>
                  <Typography fontFamily={"Poppins"} fontSize={"1.5em"}>
                    Email: {el.email}
                  </Typography>
                  <Typography fontFamily={"Poppins"} fontSize={"1.5em"}>
                    Состояние оповещения:{" "}
                    {conditionStates[el.notifyLevel].value}
                  </Typography>
                </CardContent>
                <CardActions>
                  <Box display={"flex"} justifyContent={"center"} width={"100%"}>
                    <Button size="large" onClick={() => handleUpdateUser(el)} color="info" 
                    sx={{fontWeight: "bold"}}
                    >
                      Изменить
                    </Button>
                    <Button
                      size={"large"}
                      color="error"
                      onClick={() => handleDeleteUser(el.id)}
                      sx={{fontWeight: "bold"}}
                    >
                      Удалить
                    </Button>
                  </Box>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
      <Box display={"flex"} alignContent={"center"} justifyContent={"center"}>
        <Button
          variant="contained"
          sx={{ marginBottom: 0, marginTop: 5 }}
          onClick={handleOpen}
        >
          <AddIcon />
        </Button>
      </Box>
      <UpdateEmployeeComponent
        isOpen={openUpdate}
        setOpen={setOpenUpdate}
        employee={employeeToUpdate}
        conditions={conditionStates}
      />
    </div>
  );
};

export default EmployeeComponent;
