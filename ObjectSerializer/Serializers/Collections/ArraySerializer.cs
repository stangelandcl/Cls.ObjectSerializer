using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fasterflect;
using System;

namespace ObjectSerializer
{
	class ArraySerializer : ISerializer
	{
		public ArraySerializer(Serializers serializer, Type type)
		{
			this.ser = serializer.FromDeclared (type.GetElementType());
			this.newObj = type.DelegateForCreateInstance (typeof(int));
			this.setter = type.DelegateForSetElement();
		}
		ISerializer ser;
		ArrayElementSetter setter;
		ConstructorInvoker newObj;

		public void Serialize(System.IO.Stream stream, object  item)
		{
			var array = (Array)item;
			ZigZag.Serialize (stream, (uint)array.Length);
			foreach (var i in array)
				ser.Serialize (stream, i);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			var count = (int)ZigZag.DeserializeUInt32 (stream);
			var array = (Array)newObj( count);
			for (int i=0; i<count; i++)
				setter (array, i, ser.Deserialize (stream));
			return array;
		}			
	}
}