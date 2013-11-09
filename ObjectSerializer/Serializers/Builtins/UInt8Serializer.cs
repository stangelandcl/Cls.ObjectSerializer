using System;
using System.IO;

namespace ObjectSerializer
{
	public class UInt8Serializer : SpecificSerializer<byte>
	{
		public UInt8Serializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, byte item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override byte Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadByte();
		}
	}
}
