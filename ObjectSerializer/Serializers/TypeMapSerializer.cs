using System;
using System.IO;

namespace ObjectSerializer
{
	public class TypeMapSerializer : ISerializer
	{
		public TypeMapSerializer(TypeMap typeNames){
			this.typeNames = typeNames;
		}
		TypeMap typeNames;

		public void Serialize(Stream stream, object item){
			var types = (TypeMap)item;
			var tags = types.TypeTags;
			ZigZag.Serialize (stream, (uint)tags.Length);
			foreach (var kvp in tags) {
				ZigZag.Serialize (stream, kvp.Key);
				var name = types.ToString (kvp.Value);
				stream.Write (name);
			}
		}

		public object Deserialize(Stream stream){
			int count = (int)ZigZag.DeserializeUInt32 (stream);
			var types = new TypeMap ();
			while(count-- != 0) {
				uint tag = ZigZag.DeserializeUInt32 (stream);
				var typeName = stream.ReadString ();
				var type = this.typeNames.GetType (typeName);
				types.Add(tag, type);
			}	
			return types;
		}
	}
}

