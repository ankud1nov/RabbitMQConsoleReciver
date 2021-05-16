using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSender
{
    class Message
    {
        int number;
        DateTime timeSend;
        string comment;
        string hashSumm;
        public Message()
        {
            number = 1;
            comment = "";
            SetParams();
        }
        public Message(string comment)
        {
            number = 1;
            this.comment = comment;
            SetParams();
        }
        public void Next()
        {
            number += 1;
            SetParams();
        }
        public void Next(string comment)
        {
            number += 1;
            this.comment = comment;
            SetParams();
        }
        public string GetMessage()
        {
            return hashSumm;
        }
        void SetParams()
        {
            timeSend = DateTime.Now;
            hashSumm = $"{number} {timeSend} {comment}";
        }
    }
}
