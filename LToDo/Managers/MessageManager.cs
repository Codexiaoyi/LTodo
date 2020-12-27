using LTodo.Model;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LToDo.Managers
{
    public class MessageManager
    {
        private readonly static Lazy<MessageManager> _messageManager = new Lazy<MessageManager>(new MessageManager());

        public static event Action<MessageType, TaskModel> OnReceiveMessage;

        public static MessageManager Instance
        {
            get
            {
                return _messageManager.Value;
            }
        }

        HubConnection connection;

        public MessageManager()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5006/MessageHub")
                .WithAutomaticReconnect()
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(2500);
                await connection.StartAsync();
            };

            connection.On<MessageType, TaskModel>("Receive_SR", (message, task) =>
            {
                OnReceiveMessage?.Invoke(message, task);
            });
        }

        public async void Connect(string userEmail, string password)
        {
            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("Connect_SR", userEmail, password);
            }
            catch (Exception)
            {

            }
        }

        public async void Disconnect(string userEmail)
        {
            if (connection.State != HubConnectionState.Connected)
                return;
            await connection.InvokeAsync("Disconnect_SR", userEmail);
        }

        public async void SendMessage(string userEmail, MessageType messageType, TaskModel task)
        {
            if (connection.State != HubConnectionState.Connected)
                return;
            await connection.InvokeAsync("Send_SR", userEmail, messageType, task);
        }
    }
}
