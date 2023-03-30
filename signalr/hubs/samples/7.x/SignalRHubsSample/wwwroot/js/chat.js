document.addEventListener("DOMContentLoaded", () => {
  const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

  connection.on("ReceiveMessage", (user, message) => {
    const li = document.createElement("li");
    li.textContent = `${user}: ${message}`;
    document.getElementById("messageList").appendChild(li);
  });

  document.getElementById("send").addEventListener("click", async () => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;

    // <snippet_TryCatch>
    try {
      await connection.invoke("SendMessage", user, message);
    } catch (err) {
      console.error(err);
    }
    // </snippet_TryCatch>
  });

  async function start() {
    try {
      await connection.start();
      console.log("SignalR Connected.");
    } catch (err) {
      console.log(err);
      setTimeout(start, 5000);
    }
  }

  connection.onclose(async () => {
    await start();
  });

  // Start the connection.
  start();
});
