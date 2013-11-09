using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt32Serializer : SpecificSerializer<uint>
	{
		public UInt32Serializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, uint item)
		{
			ZigZag.Serialize (stream, item);
		}
		protected override uint Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeUInt32 (stream);
		}
	}
}


