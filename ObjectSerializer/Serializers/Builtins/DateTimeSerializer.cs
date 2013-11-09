using System;
using System.IO;

namespace ObjectSerializer
{
	public class DateTimeSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object obj)	{
			var item = (DateTime)obj;
			var w = new BinaryWriter(stream);
			w.Write (item.ToBinary());
			w.Flush();
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return DateTime.FromBinary (new BinaryReader (stream).ReadInt64 ());
		}
	}
}

