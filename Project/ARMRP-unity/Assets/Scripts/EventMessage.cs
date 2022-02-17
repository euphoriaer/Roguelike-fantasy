using System;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    public class EventMessage : EventArgs
    {
        private Vector3 rolePosition;

        public Vector3 RolePosition
        {
            get => rolePosition;
            set
            {
                rolePosition = value;
            }
        }
    }

    public static class EventManager
    {
        public static event EventHandler<EventMessage> OnNewMessage;

        public static void NewMessage(EventMessage e)
        {
            EventHandler<EventMessage> temp = Volatile.Read(ref OnNewMessage);
            if (temp != null)
            {
                //通知注册了消息的对象
                temp(null, e);
            }
        }
    }
}