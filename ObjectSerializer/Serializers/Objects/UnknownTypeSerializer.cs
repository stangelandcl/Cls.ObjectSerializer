using System;

namespace ObjectSerializer
{
	public class UnknownTypeSerializer : ISerializer
	{
		public UnknownTypeSerializer (Serializers serializer, TypeMap type)
		{ 
			this.serializer = serializer;
			this.map = type;
		}
		Serializers serializer;
		TypeMap map;

		#region implemented abstract members of SpecificSerializer
		public void Serialize (System.IO.Stream stream, object item)
		{
			if (item == null) {
				ZigZag.Serialize (stream, 1U);
				return;
			}
			var id = map.ToUInt32 (item.GetType ()) << 1;
			ZigZag.Serialize (stream, id);			
			serializer.Get(item.GetType()).Serialize(stream, item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			var id = ZigZag.DeserializeUInt32 (stream);
			if ((id & 1) != 0) return null;
			var type = map.GetType (id >> 1);
			return serializer.Get(type).Deserialize(stream);
		}
		#endregion
	}
}

