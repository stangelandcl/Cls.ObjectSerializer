using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt16Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (ushort)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return (ushort)ZigZag.DeserializeInt32 (stream);
		}
	}
}


