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
			var elementType = type.IsGenericType ? type.GetGenericArguments () [0] : typeof(object);
			this.ser = serializer.FromDeclared (elementType);
			this.count = type.DelegateForGetPropertyValue ("Count");		
			var addMethod = type.GetMethods ().First (n => n.Name == "Add" && n.GetParameters().Length == 1);
			this.add = addMethod.DelegateForCallMethod ();
			this.newObj = type.DelegateForCreateInstance ();
			this.classSer = new ClassSerializer (serializer, type);
		}
		ISerializer ser;
		MethodInvoker add;
		ConstructorInvoker newObj;
		MemberGetter count;
		ClassSerializer classSer;

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
			classSer.Serialize (stream, item);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			var count = ZigZag.DeserializeUInt32 (stream);
			if(count == 1) return null;
			int c = (int)(count >> 1);
			var obj = newObj ();
			while (c-- != 0) {
				var v = ser.Deserialize (stream);
				add (obj, v);
			}
			classSer.DeserializeInto (stream, obj);
			return obj;
		}			
	}
}