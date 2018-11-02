using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE_cw1_maria
{
    public class Message
    {
        private string _id;
        private string _body;

        public string id
        {
            get { return _id; }
            set
            {
                // if the id is 10 chars long (1 letter and 9 numbers) and the other 9 are numbers
                if((value.Length == 10) && (int.TryParse((value.Remove(0, 1)), out int k)))
                {
                    _id = value;
                }
                else
                {
                    throw new ArgumentException("ID is not written in the correct format: 'S','E' or 'T' followed by 9 numeric characters");
                }
            }
        }

        public string body
        {
            get { return _body; }
            set { _body = value; }
        }
    }
}
