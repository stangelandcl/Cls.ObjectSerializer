using System;

namespace ObjectSerializer
{
	public class TaggedSerializer : SpecificSerializer<object> 
	{
		public TaggedSerializer (Serializers serializer, TypeMap type)
		: base(serializer)  { 
			this.map = type;
			intSerializer = serializer.Get (typeof(int));
		}
		TypeMap map;
		ISerializer intSerializer;


		#region implemented abstract members of SpecificSerializer
		protected override void Serialize (System.IO.Stream stream, object item)
		{
			var id = map.ToInt32(item.GetType());
			intSerializer.Serialize (stream, id);
			serializer.Get(item.GetType()).Serialize(stream, item);
		}
		protected override object Deserialize (System.IO.Stream stream)
		{
			var id = intSerializer.Deserialize<int>(stream);
			var type = map.GetType(id);
			return serializer.Get(type).Deserialize(stream);
		}
		#endregion
	}
}

