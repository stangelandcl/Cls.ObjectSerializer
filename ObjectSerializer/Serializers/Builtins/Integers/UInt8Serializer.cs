using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt8Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			stream.WriteByte ((byte)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return (byte)stream.ReadByte ();
		}
	}
}
