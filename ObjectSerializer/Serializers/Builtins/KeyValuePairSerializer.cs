using System;
using System.IO;
using System.Collections.Generic;
using Fasterflect;

namespace ObjectSerializer
{
	public class KeyValuePairSerializer : SpecificSerializer<object>
	{
		public KeyValuePairSerializer(Serializers s, Type type) : base(s){
			getKey = type.GetProperty ("Key").DelegateForGetPropertyValue ();
			getValue = type.GetProperty ("Value").DelegateForGetPropertyValue ();
			key = s.FromDeclared (type.GetGenericArguments()[0]);
			value = s.FromDeclared (type.GetGenericArguments()[1]);
		}
		MemberGetter getKey;
		MemberGetter getValue;
		ConstructorInvoker newObj;
		ISerializer key;
		ISerializer value;
		protected override void Serialize (System.IO.Stream stream, object item)	{
			key.Serialize (stream, getKey(item));
			value.Serialize (stream, getValue(item));
		}
		protected override object Deserialize (System.IO.Stream stream)
		{
			var k = key.Deserialize (stream);
			var v = value.Deserialize (stream);
			return newObj.Invoke(k, v);
		}
	}
}

