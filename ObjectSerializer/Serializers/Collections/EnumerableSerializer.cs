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
			var elementType = type.GetGenericArguments () [0];
			this.ser = serializer.FromDeclared (elementType);
			this.count = type.DelegateForGetPropertyValue ("Count");			
			this.add = type.DelegateForCallMethod ("Add", elementType);
			this.newObj = type.DelegateForCreateInstance ();
		}
		ISerializer ser;
		MethodInvoker add;
		ConstructorInvoker newObj;
		MemberGetter count;

		public void Serialize(System.IO.Stream stream, object item)
		{
			int count = (int)this.count (item);
			ZigZag.Serialize (stream, (uint)count);
			foreach (var i in (IEnumerable)item)
				ser.Serialize (stream, i);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			var obj = newObj ();
			var count = ZigZag.DeserializeUInt32 (stream);
			while (count-- != 0)
				add(obj, ser.Deserialize (stream));
			return obj;
		}			
	}
}