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
            Reciver reciver = new Reciver();//Инициализация новой подписки на события
            var factory = new ConnectionFactory() { HostName = "localhost" };//Установка настроек подключения
            reciver.Init(factory);
        }
    }
}