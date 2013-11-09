using System;
using System.IO;
using System.Collections.Generic;

namespace ObjectSerializer
{
	public class KeyValuePairSerializer<TKey, TValue> : SpecificSerializer<KeyValuePair<TKey, TValue>>
	{
		public KeyValuePairSerializer(Serializers s) : base(s){
			key = s.FromDeclared (typeof(TKey));
			value = s.FromDeclared (typeof(TValue));
		}
		ISerializer key;
		ISerializer value;
		protected override void Serialize (System.IO.Stream stream, KeyValuePair<TKey,TValue> item)	{
			key.Serialize (stream, item.Key);
			value.Serialize (stream, item.Value);
		}
		protected override KeyValuePair<TKey,TValue> Deserialize (System.IO.Stream stream)
		{
			var k = key.Deserialize<TKey> (stream);
			var v = value.Deserialize<TValue> (stream);
			return new KeyValuePair<TKey, TValue> (k, v);
		}
	}
}

