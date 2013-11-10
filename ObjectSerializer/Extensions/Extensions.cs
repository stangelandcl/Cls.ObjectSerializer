using System;
using System.IO;
using System.Text;

namespace ObjectSerializer
{
	public static class Extensions
	{
		public static byte[] ReadBytes(this Stream stream, int count){
			var bytes = new byte[count];
			int offset = 0;
			do {
				int read = stream.Read (bytes, offset, bytes.Length - offset);
				offset += read;
			} while(offset != bytes.Length);
			return bytes;
		}

		public static string ReadString(this Stream stream){
			var count = (int)ZigZag.DeserializeUInt32 (stream);
			var bytes = stream.ReadBytes (count);
			return Encoding.UTF8.GetString (bytes);
		}

		public static void Write(this Stream stream, byte[] bytes){
			stream.Write (bytes, 0, bytes.Length);
		}

		public static void Write(this Stream stream, string str){
			var bytes = Encoding.UTF8.GetBytes (str);
			ZigZag.Serialize (stream, (uint)bytes.Length);
			stream.Write (bytes);
		}
	}
}

