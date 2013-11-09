using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int64Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (long)item);
		}

		public object Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeInt64 (stream);
		}
	}
}

