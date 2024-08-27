import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Box, Button, Card, Link, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import {
  conditionResponse,
  unitCondition,
} from "../models/dto/getConditionDto";
import axios from "axios";

export const ConditionComponent = () => {
  const [connection, setConnection] = useState<HubConnection>();
  const [isConnected, setIsConnected] = useState<boolean>(false);
  const [conditions, setConditions] = useState<unitCondition[]>([]);

  useEffect(() => {
    if (connection && connection?.state == "Connected") return;

    setNewConnection();
  }, [setConnection]);

  useEffect(() => {
    console.log("update");
    getInitialCondition();
  }, []);

  function setNewConnection() {
    let newConnection = new HubConnectionBuilder()
      //.withUrl("http://localhost:5072/plc")
      .withUrl("http://localhost:5000/plc")
      .withKeepAliveInterval(60000000)
      .withServerTimeout(60000)
      .withAutomaticReconnect()
      .build();
    newConnection?.start();
    setConnection(newConnection);
    console.log(newConnection?.state);
    setIsConnected(true);
  }

  function getInitialCondition() {
    console.log("запрашиваю состояния без сокетов");
    try {
      axios
        //.get("http://localhost:5072/v1/condition")
        .get("http://localhost:5000/v1/condition")
        .then((response) => {
          let responseAsConditions = response.data as conditionResponse;
          console.log(responseAsConditions);

          setConditions(responseAsConditions.units);
        })
        .catch((err) =>
          console.log("error while getting initial conditions: ", err)
        );
    } catch (err) {
      console.log("error while getting initial conditions: ", err);
    }
  }

  function getBackgroundColor(index: number): string {
    switch (index) {
      case 1:
        return "green";
      case 2:
        return "lightgreen";
      case 3:
        return "yellow";
      case 4:
        return "orange";
      case 5:
        return "red";
      default:
        return "red";
    }
  }

  function randomDouble(min: number, max: number): number {
    return Math.random() * (max - min) + min;
  }

  async function handleGetPlcMessage() {
    let objToSend1 = {
      Name: "Экструдер Основной",
      Parameters: [
        { ParameterName: "Длина", Value: 36.7 },
        { ParameterName: "Сопротивление изоляции", Value: 0.6 },
        { ParameterName: "Давление", Value: randomDouble(40, 85) },
        { ParameterName: "Температура камеры", Value: randomDouble(15, 220) },
        { ParameterName: "Вибрация", Value: 38 },
        { ParameterName: "Температура", Value: randomDouble(25, 95) },
      ],
    };

    let objToSend2 = {
      Name: "Конвейер основной",
      Parameters: [
        { ParameterName: "Длина", Value: 20 },
        { ParameterName: "Сопротивление изоляции", Value: 0.6 },
        { ParameterName: "Высота", Value: 3 },
        { ParameterName: "Вибрация", Value: randomDouble(0, 15) },
      ],
    };

    let arr = [objToSend1, objToSend2];
    await connection?.invoke("GetPlcMessage", JSON.stringify(arr));
  }

  async function handleDownloadReport() {
    const url = "http://localhost:5000/v1/condition/report";
    //const url = "http://localhost:5072/v1/condition/report";

    try {
      await axios({
        url: url,
        method: "POST",
        responseType: "blob", // указываем, что ожидаем получить файл в формате Blob
      })
        .then((response) => {
          const file = new Blob([response.data], {
            type: "application/octet-stream",
          });
          const fileURL = URL.createObjectURL(file);
          const a = document.createElement("a");
          a.href = fileURL;
          a.target = "_blank";
          a.download = "report.xlsx"; // указываем имя файла для сохранения
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
        })
        .catch((error) => {
          console.error("Error downloading file:", error);
        });
    } catch (err) {
      console.log("error while dowloading report: ", err);
    }
  }

  try {
    connection?.on("GetConditionData", (data) => {
      let newConditions = new Array<unitCondition>();
      let converted = JSON.parse(data) as {
        Units: { UnitName: string; Index: number }[];
      };
      //console.log(converted);
      if (converted.Units.length !== 0) {
        converted.Units.map((condition) => {
          console.log("cond ", condition);
          let newCondition = new unitCondition(
            condition.UnitName,
            condition.Index
          );

          let conditionFromStateIndex = conditions.findIndex(
            (x) => x.unitName == newCondition.unitName
          );
          if (conditionFromStateIndex !== -1) {
            newConditions.push(newCondition);
          }
        });

        console.log(newConditions);
        setConditions(newConditions);
      } else {
        console.log("Пропускаю");
      }
    });
  } catch (err) {
    console.log("error while connection to socket: ", err);
  }

  connection?.onclose(() => {
    setIsConnected(false);
  });

  return (
    <Box>
      <Box>
        <Typography
          variant="h5"
          fontWeight={"bold"}
          textAlign={"center"}
          color={"rgb(74, 68, 45)"}
          marginTop={2}
        >
          Состояние узлов
        </Typography>
        <Typography
          variant="h5"
          fontWeight={"bold"}
          textAlign={"center"}
          color={"rgb(74, 68, 45)"}
        >
          Состояние подключения к серверу:{" "}
          {isConnected ? "Подключено" : "Не подключено"}
        </Typography>
        <Box
          marginLeft={2}
          marginRight={2}
          flexWrap={"wrap"}
          display={"grid"}
          gridTemplateColumns={"repeat(2, minmax(0, 1fr))"}
          padding={2}
        >
          {conditions.map((condition, i) => {
            return (
              <Card
                key={i}
                sx={{
                  backgroundColor: getBackgroundColor(condition.index),
                  width: "95%",
                  height: "15em",
                  marginBottom: 2,
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                <Typography
                  fontWeight={"bold"}
                  variant="h4"
                  fontFamily={"Poppins"}
                >
                  {condition.unitName}
                </Typography>
              </Card>
            );
          })}
        </Box>
      </Box>
      <Box
        display={"flex"}
        flexDirection={"column"}
        alignItems={"center"}
        justifyContent={"center"}
      >
        <Button sx={{marginBottom:2}} variant="contained" onClick={handleGetPlcMessage}>
          Вызов GetPlcMessage
        </Button>
        <Button variant="contained" onClick={handleDownloadReport}>
          Скачать отчет
        </Button>
      </Box>
    </Box>
  );
};
