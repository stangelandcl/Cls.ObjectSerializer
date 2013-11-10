
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fasterflect;
using System;
using System.Collections;

namespace ObjectSerializer
{
	class DictionarySerializer: ISerializer
	{
		public DictionarySerializer(Serializers serializer, Type type)
		{
			var elementType = typeof(KeyValuePair<,>).MakeGenericType (type.GetGenericArguments ());			
			this.ser = serializer.FromDeclared (elementType);
			this.count = type.DelegateForGetPropertyValue ("Count");			
			this.add = type.DelegateForSetIndexer (elementType.GetGenericArguments ());
			this.newObj = type.DelegateForCreateInstance ();
			this.key = elementType.DelegateForGetPropertyValue ("Key");
			this.value = elementType.DelegateForGetPropertyValue ("Value");
		}
		ISerializer ser;
		MethodInvoker add;
		ConstructorInvoker newObj;
		MemberGetter count;
		MemberGetter key;
		MemberGetter value;

		public void Serialize(System.IO.Stream stream, object item)
		{
			if (item == null) {
				ZigZag.Serialize (stream, 1U);
				return;
			}
			int count = (int)this.count (item);
			ZigZag.Serialize (stream, (uint)count << 1);
			foreach (var i in (IEnumerable)item)
				ser.Serialize (stream, i);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			var count = ZigZag.DeserializeUInt32 (stream);
			if(count == 1) return null;
			int c = (int)(count >> 1);
			var obj = newObj ();
			while (c-- != 0) {
				var x = ser.Deserialize (stream).WrapIfValueType ();
				add (obj, key(x), value(x));
			}
			return obj;
		}			
	}
}
