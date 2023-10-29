using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Bro.Client
{
    [Serializable]
    public class SerializableResourceDictionary<TEnum, TValue> : SerializableResource, ISerializationCallbackReceiver where TEnum : Enum
    {
        [NonSerialized] private Dictionary<string, TValue> _dict;
        [NonSerialized] private IEqualityComparer<string> _comparer;
        
        [SerializeField] private string[] _keys;
        [SerializeField] private TValue[] _values;
        
        public SerializableResourceDictionary()
        {

        }

        public SerializableResourceDictionary(IEqualityComparer<string> comparer)
        {
            _comparer = comparer;
        }
        
        public IEqualityComparer<string> Comparer
        {
            get { return _comparer; }
        }

        public int Count
        {
            get { return _dict?.Count ?? 0; }
        }

        public void Add(string key, TValue value)
        {
            CreateIfNull();
            _dict.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _dict != null && _dict.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get
            {
                CreateIfNull();
                return _dict.Keys;
            }
        }

        public bool Remove(string key)
        {
            return _dict != null && _dict.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            if (_dict != null)
            {
                return _dict.TryGetValue(key, out value);
            }
            value = default;
            return false;
        }

        public ICollection<TValue> Values
        {
            get
            {
                CreateIfNull();
                return _dict.Values;
            }
        }

        public TValue this[string key]
        {
            get
            {
                if (_dict == null)
                {
                    throw new KeyNotFoundException();
                }
                return _dict[key];
            }
            set
            {
                CreateIfNull();
                _dict[key] = value;
            }
        }

        public void Clear()
        {
            if (_dict != null)
            {
                _dict.Clear();
            }
        }

        private void CreateIfNull()
        {
            if (_dict == null)
            {
                _dict = new Dictionary<string, TValue>(_comparer);
            }
        }


        public Dictionary<string, TValue>.Enumerator GetEnumerator()
        {
            return _dict?.GetEnumerator() ?? default(Dictionary<string, TValue>.Enumerator);
        }

      
        
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if(_keys != null && _values != null)
            {
                CreateIfNull(); 
                _dict.Clear();
                for(var i = 0; i < _keys.Length; i++)
                {
                    if (i < _values.Length)
                    {
                        _dict[_keys[i]] = _values[i];
                    }
                    else
                    {
                        _dict[_keys[i]] = default;
                    }

                    ReOrderDict();
                }
            }

            _keys = null;
            _values = null;
        }
        
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // _enum = (TEnum) new Int32();
                
            if(_dict == null || _dict.Count == 0)
            {
                _keys = null;
                _values = null;
            }
            else
            {
                var count = _dict.Count;
                _keys = new string[count];
                _values = new TValue[count];
                var i = 0;

                ReOrderDict();
                
                var e = _dict.GetEnumerator();
                while (e.MoveNext())
                {
                    _keys[i] = e.Current.Key;
                    _values[i] = e.Current.Value;
                    i++;
                }
                e.Dispose();
            }
        }

        private void ReOrderDict()
        {
            if (_dict == null)
            {
                return;
            }
            
            var a = new Dictionary<string, TValue>();
            
            foreach (var e in (TEnum[]) Enum.GetValues(typeof(TEnum)))
            {
                var description = e.GetMemberDescription();
                if (_dict.ContainsKey(description))
                {
                    a[description] = _dict[description];
                    _dict.Remove(description);
                }
            }

            foreach (var pair in _dict)
            {
                a[pair.Key] = pair.Value;
            }
            
            _dict = new Dictionary<string, TValue>(a);
        }
        
        public Dictionary<string, TValue> Dict => _dict;

        public override void Add()
        {
            _dict[string.Empty] = default;
            this.Serialize();
        }

        public override bool IsValidKey(string key)
        {
            foreach (var e in (TEnum[]) Enum.GetValues(typeof(TEnum)))
            {
                var description = e.GetMemberDescription();
                if (description == key)
                {
                    return true;
                }
            }

            return false;
        }
    }
}