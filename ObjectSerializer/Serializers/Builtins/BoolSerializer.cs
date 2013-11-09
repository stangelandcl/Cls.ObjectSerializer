using System;
using System.IO;

namespace ObjectSerializer
{
	public class BoolSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)	{
			var b = (bool)item;
			stream.WriteByte (b ? (byte)1 : (byte)0);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return stream.ReadByte () == 1;
		}
	}
}