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
                    throw new Exception("Email not in valid format.");
                }
            }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

    }
}
