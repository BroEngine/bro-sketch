using System.Collections.Generic;

namespace Bro.Client.ConfigParsing
{
    public class RowFields
    {
        public readonly Dictionary<string, object> _fields = new Dictionary<string, object>();

        public object GetField(string columnName)
        {
            if (_fields.ContainsKey(columnName))
            {
                return _fields[columnName];
            }

            return null;
        }

        public bool Contains(string columnName)
        {
            return _fields.ContainsKey(columnName);
        }

        public void Add(string columnName, object fieldValue)
        {
            _fields.Add(columnName, fieldValue);
        }

        public bool IsEmpty(string columnName)
        {
            return string.IsNullOrEmpty(GetField(columnName).ToString());
        }
    }
}