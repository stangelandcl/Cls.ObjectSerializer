using System;
using System.IO;

namespace ObjectSerializer
{
	public class GuidSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)	{
			var bytes = ((Guid)item).ToByteArray ();
			stream.Write (bytes, 0, bytes.Length);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return new Guid (new BinaryReader (stream).ReadBytes (16));
		}
	}
}


