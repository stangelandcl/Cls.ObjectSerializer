using System;
using System.IO;

namespace ObjectSerializer
{
	public class DecimalSerializer : SpecificSerializer<decimal>
	{
		public DecimalSerializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, decimal item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override decimal Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadSByte();
		}
	}
}

