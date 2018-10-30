using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_cw1_maria
{
    public class Sms : Message
    {
        private string _sender;
        private string _text;

        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
