using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt64Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (ulong)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeUInt64 (stream);
		}
	}
}

