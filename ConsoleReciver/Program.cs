using System;
using System.Threading;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace ConsoleReciver
{

    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };//Установка настроек подключения
            Reciver reciver = new Reciver(factory);//Инициализация новой подписки на события
        }
    }
}