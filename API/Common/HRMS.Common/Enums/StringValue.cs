using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Common.Enumeration
{
    [Obsolete]
    public class StringValue : System.Attribute 
    { 
        private string _value; 
        public StringValue(string value) 
        {
            _value = value; 
        } 
        public string Value 
        { 
            get { return _value; } 
        } 
    }
}
