using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int32Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (int)item);
		}

		public object Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeInt32 (stream);
		}
	}
}

