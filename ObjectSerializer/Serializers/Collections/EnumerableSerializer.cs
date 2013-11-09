using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fasterflect;
using System;
using System.Collections;

namespace ObjectSerializer
{
	class EnumerableSerializer: ISerializer
	{
		public EnumerableSerializer(Serializers serializer, Type type)
			 {
			Type elementType;
			if (type.GetInterfaces ().Any (n => n.IsGenericType &&
				n.GetGenericTypeDefinition () == typeof(IDictionary<,>))) {
				elementType = typeof(KeyValuePair<,>).MakeGenericType (type.GetGenericArguments ());
			} else {
				elementType = type.GetGenericArguments () [0];
			}
			this.ser = serializer.FromDeclared (elementType);
			this.count = type.DelegateForGetPropertyValue ("Count");
			var add = type.Method ("Add", new[] { elementType }) ?? type.Method ("Add");
			this.add = add.DelegateForCallMethod ();
			this.newObj = type.DelegateForCreateInstance ();
		}
		ISerializer ser;
		MethodInvoker add;
		ConstructorInvoker newObj;
		MemberGetter count;

		public void Serialize(System.IO.Stream stream, object item)
		{
			int count = (int)this.count (item);//(int)this.count.Invoke (null, item);
			ZigZag.Serialize (stream, (uint)count);
			foreach (var i in (IEnumerable)item)
				ser.Serialize (stream, i);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			var obj = newObj ();
			var count = ZigZag.DeserializeUInt32 (stream);
			while (count-- != 0)
				add.Invoke (obj, ser.Deserialize (stream));
			return obj;
		}			
	}
}