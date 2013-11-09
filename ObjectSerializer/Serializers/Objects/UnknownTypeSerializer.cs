using System;

namespace ObjectSerializer
{
	public class UnknownTypeSerializer : SpecificSerializer<object> 
	{
		public UnknownTypeSerializer (Serializers serializer, TypeMap type)
		: base(serializer)  { 
			this.map = type;
		}
		TypeMap map;

		#region implemented abstract members of SpecificSerializer
		protected override void Serialize (System.IO.Stream stream, object item)
		{
			var id = map.ToUInt32 (item.GetType ()) << 1;
			if (item == null) id |= 1;
			ZigZag.Serialize (stream, id);
			if(item != null)
				serializer.Get(item.GetType()).Serialize(stream, item);
		}
		protected override object Deserialize (System.IO.Stream stream)
		{
			var id = ZigZag.DeserializeUInt32 (stream);
			if ((id & 1) != 0) return null;
			var type = map.GetType(id>>1);
			return serializer.Get(type).Deserialize(stream);
		}
		#endregion
	}
}

