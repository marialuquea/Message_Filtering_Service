using System;
using System.Text.RegularExpressions;

namespace SE_cw1_maria
{
    public class Sms : Message
    {
        private string _sender;
        private string _text;
        private string _regex = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2})?(?<areaCode>0?[1-9]\d{0,4})(?<number>[1-9][\d]{5,12})(?<extension>x\d{0,4})?$";

        public string Sender
        {
            get { return _sender; }
            set
            {
                if (!Regex.IsMatch(value, _regex))
                {
                    throw new FormatException("Telephone number is in wrong format");
                }
                _sender = value;
            }
        
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }
}
