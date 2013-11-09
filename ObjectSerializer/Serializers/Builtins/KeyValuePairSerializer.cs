using System;
using System.IO;
using System.Collections.Generic;
using Fasterflect;

namespace ObjectSerializer
{
	public class KeyValuePairSerializer : ISerializer
	{
		public KeyValuePairSerializer(Serializers s, Type type) 
		{
			getKey = type.DelegateForGetPropertyValue ("Key");
			getValue = type.DelegateForGetPropertyValue ("Value");
			key = s.FromDeclared (type.GetGenericArguments()[0]);
			value = s.FromDeclared (type.GetGenericArguments()[1]);
			newObj = type.DelegateForCreateInstance (type.GetGenericArguments ());
		}
		MemberGetter getKey;
		MemberGetter getValue;
		ConstructorInvoker newObj;
		ISerializer key;
		ISerializer value;
		public void Serialize (System.IO.Stream stream, object item)	{
			var x = item.WrapIfValueType ();
			key.Serialize (stream, getKey(x));
			value.Serialize (stream, getValue(x));
		}
		public object Deserialize (System.IO.Stream stream)
		{
			var k = key.Deserialize (stream);
			var v = value.Deserialize (stream);
			return newObj(k, v);
		}
	}
}

