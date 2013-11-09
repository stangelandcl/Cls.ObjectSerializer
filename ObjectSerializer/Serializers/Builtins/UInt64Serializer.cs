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
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override ulong Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadUInt64();
		}
	}
}

