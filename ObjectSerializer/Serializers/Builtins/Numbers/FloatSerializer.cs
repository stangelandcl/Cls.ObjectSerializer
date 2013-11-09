using System;
using System.IO;

namespace ObjectSerializer
{
	public class FloatSerializer : SpecificSerializer<float>
	{
		public FloatSerializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, float item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override float Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadSingle();
		}
	}
}


