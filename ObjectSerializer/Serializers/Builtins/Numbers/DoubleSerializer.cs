using System;
using System.IO;

namespace ObjectSerializer
{
	public class DoubleSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			var l = BitConverter.DoubleToInt64Bits ((double)item);
			ZigZag.Serialize (stream, l);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return BitConverter.Int64BitsToDouble (ZigZag.DeserializeInt64 (stream));
		}
	}
}

