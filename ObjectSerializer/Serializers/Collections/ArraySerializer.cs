using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fasterflect;
using System;

namespace ObjectSerializer
{
	class ArraySerializer : SpecificSerializer<Array>
	{
		public ArraySerializer(Serializers s, Type type)
			: base(s) {
			this.ser = serializer.FromDeclared (type.GetElementType());
			this.type = type;
		}
		ISerializer ser;
		Type type;

		protected override void Serialize(System.IO.Stream stream, Array array)
		{
			ZigZag.Serialize (stream, (uint)array.Length);
			foreach (var i in array)
				ser.Serialize (i);
		}

		protected override Array Deserialize(System.IO.Stream stream)
		{
			var count = ZigZag.DeserializeUInt32 (stream);
			var array = Array.CreateInstance (type.GetElementType (), count);
			for (int i=0; i<count; i++)
				array.SetValue (ser.Deserialize (stream), i);
			return array;
		}			
	}
}