import React, { useEffect, useState } from "react";
import "./App.css";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { Button } from "react-bootstrap";
import PlusComponent from "./components/plus-component";

function App() {
  const [conn, setConn] = useState<HubConnection>();
  const [messagesFromHub, setMessagesFromHub] = useState<string[]>([]);

  useEffect(() => {
    /*let newConn = new HubConnectionBuilder()
      .withUrl("http://localhost:5072/index")
      .withStatefulReconnect()
      .withAutomaticReconnect([1])
      .build();

    setConn(newConn);*/
  }, []);

  conn?.on("ReceiveMessage", (msg) => {
    console.log(msg);

    var newList = [...messagesFromHub, msg];
    setMessagesFromHub(newList);
  });

  async function send() {
    try {
      await conn?.invoke("Send");
    } catch (err) {
      console.log(err);
    }
  }

  async function closeConn() {
    if (conn?.state == "Connected") {
      var newList = [...messagesFromHub, "disconnected"];
      setMessagesFromHub(newList);

      await conn?.stop();
    }
  }

  async function startConn() {
    if (conn?.state != "Connected") {
      try {
        let newConn = conn;
        newConn?.start().then(() => {
          console.log(newConn?.connectionId);
          setConn(newConn);
        });
      } catch (err) {
        console.log(err);
      }
    }
  }

  return (
    <div>
      <PlusComponent/>
    </div>
  );
}

export default App;

/*return (
  <div>
    <p>From hub</p>
    <ul>
      {messagesFromHub.map((msg, idx) => {
        return <li key={idx}>{msg}</li>;
      })}
    </ul>
    <Button onClick={startConn}>Подключиться</Button>

    <Button onClick={closeConn}>Разорвать соединение</Button>

    <Button onClick={send}>Вызвать</Button>

    <PlusComponent/>
  </div>
);*/
