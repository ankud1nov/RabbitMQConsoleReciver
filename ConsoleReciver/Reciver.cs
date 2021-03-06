using System;
using System.IO;
using System.Threading;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Generic;

namespace ConsoleReciver
{
    class Reciver 
    {
        ConnectionFactory factory;
        string queueName;
        bool durable;
        bool exclusive;
        bool autoDelete;
        IDictionary<string, object> arguments;

        public Reciver(ConnectionFactory factory)
        {
            this.factory = factory;
            queueName = "hello";
            durable = false;
            exclusive = false;
            autoDelete = true;
            arguments = null;
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }

        public Reciver(ConnectionFactory factory, string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string,object> arguments = null)
        {
            this.factory = factory;
            this.queueName = queueName;
            this.durable = durable;
            this.exclusive = exclusive;
            this.autoDelete = autoDelete;
            this.arguments = arguments;
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }

        //TODO: Конструктор подписки на несколько очередей

        void InitInNewThread()
        {
            try
            {
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null
                                         );

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        if (WriteInBase(message))
                        {
                            try
                            {
                                channel.BasicReject(ea.DeliveryTag, false); //принятиt при ручном принятии сообщений
                                Console.WriteLine($" [V] Received: {message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                        }
                        else
                        {
                            try
                            {
                                channel.BasicReject(ea.DeliveryTag, false); //отклонение принятия при ручном принятии сообщений
                                Console.WriteLine($" [X] Rejected: {message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    };
                    channel.BasicConsume(queue: "hello",
                                         autoAck: false,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Init();
            }
        }

        void Init()
        {
            Console.WriteLine("Перезапуск");
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }

        //TODO: Проверки для проверки Ack/Reject

        bool WriteInBase(string messsage)
        {
            messsage = $"{DateTime.Now} {messsage}\n";
            try
            {
                using (FileStream fstream = new FileStream($"log", FileMode.OpenOrCreate))
                {
                    byte[] array = Encoding.Default.GetBytes(messsage);
                    fstream.Seek(0, SeekOrigin.End);
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }
                return true;
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
