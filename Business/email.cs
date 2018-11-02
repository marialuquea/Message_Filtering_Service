using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_cw1_maria
{
    public class Email : Message
    {
        private string _sender;
        private string _subject;
        private string _text;

        public string Sender
        {
            get { return _sender; }
            set
            {
                try
                {
                    var addr = new System.Net.Mail.MailAddress(value);
                    _sender = value;
                }
                catch
                {
                    throw new ArgumentException("Email not in valid format.");
                }
            }
        }

        public string Subject
        {
            get { return _subject; }
            set
            {
                if (value.Length < 21)
                    _subject = value;
                else
                    throw new ArgumentException("Subject can be a max of 20 characters.");
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value.Length < 1029)
                    _text = value;
                else
                    throw new ArgumentException("Text can be a maximum of 1028 characters.");
            }
        }

    }
}
