
using System.IO;

namespace ObjectSerializer
{
	public class Int8Serializer : SpecificSerializer<sbyte>
	{
		public Int8Serializer(Serializers s)
		: base(s){}
		protected override void Serialize (System.IO.Stream stream, sbyte item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override sbyte Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadSByte();
		}
	}
}
