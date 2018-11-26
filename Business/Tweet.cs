using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_cw1_maria
{
    public class Tweet : Message
    {
        private string _sender;
        private string _text;

        public string Sender
        {
            get { return _sender; }
            set
            {
                if (value.StartsWith("@") && (value.Length < 16) && (value.Length > 0))
                    _sender = value;
                else
                    throw new ArgumentException("Sender must start with @ and have a max of 15 characters.");
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if ((value.Length < 141) && (value.Length > 0))
                    _text = value;
                else
                    throw new ArgumentException("Tweet text can only be max 140 characters long.");
            }
        }

    }
}
