using System;
using System.IO;
using System.Text;

namespace ObjectSerializer
{
	public class StringSerializer : ISerializer
	{
		public void Serialize(System.IO.Stream stream, object item)
		{
			if (item == null) {
				ZigZag.Serialize (stream, 1);
				return;
			}
			var str = (string)item;
			ZigZag.Serialize (stream, (long)str.Length << 1);
			var bytes = Encoding.UTF8.GetBytes (str);
			stream.Write (bytes, 0, bytes.Length);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			long count = ZigZag.DeserializeInt64 (stream);
			if(count == 1) return null;
			var c = (int)(count >> 1);
			var bytes = stream.ReadBytes (c);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}

