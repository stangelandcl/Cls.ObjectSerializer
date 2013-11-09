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
			stream.WriteByte (item);
		}
		protected override byte Deserialize (System.IO.Stream stream)
		{
			return (byte)stream.ReadByte ();
		}
	}
}
