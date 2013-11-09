using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt16Serializer : SpecificSerializer<ushort>
	{
		public UInt16Serializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, ushort item)
		{
			ZigZag.Serialize (stream, (uint)item);
		}
		protected override ushort Deserialize (System.IO.Stream stream)
		{
			return (ushort)ZigZag.DeserializeInt32 (stream);
		}
	}
}


