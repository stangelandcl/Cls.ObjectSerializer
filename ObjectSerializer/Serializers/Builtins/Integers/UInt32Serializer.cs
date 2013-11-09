using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt32Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (uint)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeUInt32 (stream);
		}
	}
}


