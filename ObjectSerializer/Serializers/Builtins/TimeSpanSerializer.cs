using System;
using System.IO;

namespace ObjectSerializer
{
	public class TimeSpanSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object obj)	{
			var item = (TimeSpan)obj;
			var w = new BinaryWriter(stream);
			w.Write (item.TotalMilliseconds);
			w.Flush();
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return TimeSpan.FromMilliseconds (new BinaryReader (stream).ReadDouble ());
		}
	}
}

