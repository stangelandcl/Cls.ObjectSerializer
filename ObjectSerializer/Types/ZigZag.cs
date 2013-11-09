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
				stream.WriteByte((byte)(v & 127));
				v >>= 7;
			} while(v != 0);
		}
		public static void Serialize(Stream stream, long v){
			var x = (ulong)((v << 1) ^ (v >> 63));
			Serialize (stream, x);
		}
		public static void Serialize(Stream stream, ulong v){
			do {
				stream.WriteByte((byte)(v & 127));
				v >>= 7;
			} while(v != 0);
		}
		public static int DeserializeInt32(Stream stream){
			return (int)DeserializeUInt32 (stream);
		}
		public static long DeserializeInt64(Stream stream){
			return (long)DeserializeUInt64 (stream);
		}
		public static uint DeserializeUInt32(Stream stream){
			uint i = 0;
			byte b;
			while(true) {
				b = (byte)stream.ReadByte ();
				i |= (uint)(b & 127);
				if (b <= 127) break;
				i <<= 7;
			}
			return i;
		}
		public static ulong DeserializeUInt64(Stream stream){
			ulong i = 0;
			byte b;
			while(true) {
				b = (byte)stream.ReadByte ();
				i |= (ulong)(b & 127);
				if (b <= 127) break;
				i <<= 7;
			}
			return i;
		}
	}
}

