using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt64Serializer : SpecificSerializer<ulong>
	{
		public UInt64Serializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, ulong item)
		{
			ZigZag.Serialize (stream, item);
		}
		protected override ulong Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeUInt64 (stream);
		}
	}
}

