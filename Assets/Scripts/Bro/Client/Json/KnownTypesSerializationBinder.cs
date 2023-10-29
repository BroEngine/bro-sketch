using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;


namespace Bro.Client.Json
{
    public class KnownTypesSerializationBinder : ISerializationBinder
    {
        private readonly DefaultSerializationBinder _defaultSerializationBinder = new DefaultSerializationBinder();
        
        private readonly Dictionary<string,Type> _knownTypes = new Dictionary<string, Type>();
        private readonly Dictionary<Type,string> _knownNames = new Dictionary<Type, string>();

        public void RegisterType(string name, Type type)
        {
            _knownTypes.Add( name, type );
            _knownNames.Add( type, name );
        }

        // reader
        public Type BindToType(string assemblyName, string typeName)
        {
            if (_knownTypes.ContainsKey(typeName))
            {
                return _knownTypes[typeName];
            }

            return _defaultSerializationBinder.BindToType(assemblyName,typeName);
        }
        
        // writer
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (_knownNames.ContainsKey( serializedType ))
            {
                assemblyName = null;
                typeName = _knownNames[serializedType]; 
                return;
            }

            _defaultSerializationBinder.BindToName(serializedType, out assemblyName, out typeName);
        }
    }
}