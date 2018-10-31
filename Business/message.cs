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
            set { _id = value; }
        }

        public string body
        {
            get { return _body; }
            set { _body = value; }
        }
    }
}
