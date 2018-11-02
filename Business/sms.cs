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
                    throw new ArgumentException("Telephone number is in wrong format");
                else 
                    _sender = value;
            }
        
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if ((value.Length > 0) && (value.Length < 141))
                    _text = value;
                else
                    throw new ArgumentException("SMS has to have between 0 and 140 characters.");
            }
        }
    }
}
