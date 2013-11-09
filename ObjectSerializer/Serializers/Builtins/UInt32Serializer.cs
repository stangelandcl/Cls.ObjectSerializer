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
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override uint Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadUInt32();
		}
	}
}


