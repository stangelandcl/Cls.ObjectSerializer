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
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override ushort Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadUInt16();
		}
	}
}


