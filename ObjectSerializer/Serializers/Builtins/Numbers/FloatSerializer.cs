using System;
using System.IO;

namespace ObjectSerializer
{
	public unsafe class FloatSerializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			var f = (float)item;
			uint* a = (uint*)&f;
			ZigZag.Serialize (stream, *a);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			uint a = ZigZag.DeserializeUInt32 (stream);
			return *(float*)&a;
		}
	}
}


