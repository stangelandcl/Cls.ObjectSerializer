using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class Serializer : ISerializer
	{
		Serializers serializer = new Serializers();

		public void Serialize(System.IO.Stream stream, object item)
		{
			serializer.Tagged.Serialize(stream, item);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			return serializer.Tagged.Deserialize(stream);
		}

		public object DeserializeMessage(byte[] bytes){
			return DeserializeMessage (new MemoryStream (bytes));
		}

		public object DeserializeMessage(Stream stream){
			var types = this.serializer.DeserializeTypeMap (stream);
			var serializer = this.serializer.CopyNewTags (types);
			return serializer.Tagged.Deserialize (stream);
		}

		public void SerializeMessage(Stream stream, object item){
			var serializer = this.serializer.CopyNoTags ();
			var ms = new MemoryStream ();
			serializer.Tagged.Serialize (ms, item);
			serializer.SerializeTypeMap (stream);
			var buf = ms.GetBuffer ();
			stream.Write (buf, 0, (int)ms.Position);
		}

		public byte[] SerializeMessage(object item){
			var ms = new MemoryStream ();
			SerializeMessage (ms, item);
			var array = ms.GetBuffer ();
			Array.Resize (ref array, (int)ms.Position);
			return array;
		}
	}
}

