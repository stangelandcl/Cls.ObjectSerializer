using System;
using Fasterflect;

namespace ObjectSerializer
{
	public class EnumSerializer : ISerializer
	{
		public EnumSerializer (Serializers s, Type type)
		{
			this.type = type;
			this.underlying = Enum.GetUnderlyingType(type);
			this.serializer = s.Get(underlying);
		}
		Type type;
		Type underlying;
		ISerializer serializer;
		public void Serialize (System.IO.Stream stream, object item)
		{
			serializer.Serialize(stream, Convert.ChangeType(item, underlying));
		}

		public object Deserialize (System.IO.Stream stream)
		{
			var obj = serializer.Deserialize(stream);
			return Enum.ToObject(type, obj);
		}
	}
}

