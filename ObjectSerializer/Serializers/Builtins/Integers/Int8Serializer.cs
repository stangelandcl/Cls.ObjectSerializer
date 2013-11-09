
using System.IO;

namespace ObjectSerializer
{
	public class Int8Serializer : SpecificSerializer<sbyte>
	{
		public Int8Serializer(Serializers s)
		: base(s){}
		protected override void Serialize (System.IO.Stream stream, sbyte item)
		{
			stream.WriteByte ((byte)item);
		}
		protected override sbyte Deserialize (System.IO.Stream stream)
		{
			return (sbyte)stream.ReadByte ();
		}
	}
}
