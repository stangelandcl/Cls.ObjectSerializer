using System;
using System.IO;

namespace ObjectSerializer
{
	public static class ZigZag
	{
		public static void Serialize(Stream stream, int v){
			var x = (uint)((v << 1) ^ (v >> 31));
			Serialize (stream, x);
		}
		public static void Serialize(Stream stream, uint v){
			do {
				var x = (byte)(v & 127);
				v >>= 7;
				if(v != 0) x |= 128;
				stream.WriteByte(x);
			} while(v != 0);
		}
		public static void Serialize(Stream stream, long v){
			var x = (ulong)((v << 1) ^ (v >> 63));
			Serialize (stream, x);
		}
		public static void Serialize(Stream stream, ulong v){
			do {
				var x = (byte)(v & 127);
				v >>= 7;
				if(v != 0) x |= 128;
				stream.WriteByte(x);
			} while(v != 0);
		}
		public static int DeserializeInt32(Stream stream){
			var x = DeserializeUInt32 (stream);
			return (int)((x >> 1) ^ (x << 31));
		}
		public static long DeserializeInt64(Stream stream){
			var x = DeserializeUInt64 (stream);
			return (long)((x >> 1) ^ (x << 63));
		}
		public static uint DeserializeUInt32(Stream stream){
			uint i = 0;
			byte b;
			int shift = 0;
			while(true) {
				b = (byte)stream.ReadByte ();
				i |= (uint)(b & 127) << shift;
				if (b <= 127) break;
				shift += 7;
			}
			return i;
		}
		public static ulong DeserializeUInt64(Stream stream){
			ulong i = 0;
			byte b;
			int shift = 0;
			while(true) {
				b = (byte)stream.ReadByte ();
				i |= (ulong)(b & 127) << shift;
				if (b <= 127) break;
				shift += 7;
			}
			return i;
		}
	}
}

