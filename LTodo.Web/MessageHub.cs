using LTodo.Web.IRepository;
using LTodo.Web.Model;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTodo.Web
{
    public class MessageHub : Hub
    {
        private readonly IUserRepository userRepository;
        private readonly ITaskRepository taskRepository;

        public MessageHub(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            this.userRepository = userRepository;
            this.taskRepository = taskRepository;
        }

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// 客户端连接
        /// </summary>
        public async Task Connect_SR(string userEmail, string password)
        {
            var user = await userRepository.QueryByEmailAsync(userEmail);
            if (user.Password == password)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userEmail);
            }
        }

        /// <summary>
        /// 客户端断开连接
        /// </summary>
        public async Task Disconnect_SR(string userEmail)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userEmail);
        }

        /// <summary>
        /// 发送数据同步
        /// </summary>
        public async Task Send_SR(string userEmail, MessageType messageType, TaskModel task)
        {
            task.UserEmail = userEmail;
            //服务端留档
            switch (messageType)
            {
                case MessageType.Add:
                    await taskRepository.AddAsync(task);
                    break;
                case MessageType.Remove:
                    await taskRepository.DeleteByIdAsync(task.Id);
                    break;
                case MessageType.Update:
                    await taskRepository.UpdateAsync(task);
                    break;
                default:
                    break;
            }
            //给剩余客户端发通知
            await Clients.GroupExcept(userEmail, Context.ConnectionId).SendAsync("Receive_SR", messageType, task);
        }
    }
}
