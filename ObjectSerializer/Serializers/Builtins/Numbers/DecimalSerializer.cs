using System;
using System.IO;

namespace ObjectSerializer
{
	public class DecimalSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			var w = new BinaryWriter(stream);
			w.Write((decimal)item);
			w.Flush();
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadDecimal();
		}
	}
}

