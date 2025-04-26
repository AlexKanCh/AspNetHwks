const signalR = require("@microsoft/signalr");

// Создаем подключение к SignalR Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:8091/notificationHub") // URL вашего SignalR сервера
    .build();

// Присоединяемся к группе по partnerId
const partnerId = "0da65561-cf56-4942-bff2-22f50cf70d43"; // Уникальный идентификатор партнера
connection.start()
    .then(() => {
        console.log("SignalR connected.");
        return connection.invoke("JoinGroup", partnerId);
    })
    .catch(err => console.error("Error starting connection:", err));

// Обработка уведомлений
connection.on("ReceiveNotification", (message) => {
    console.log(`Received notification: ${message}`);
});