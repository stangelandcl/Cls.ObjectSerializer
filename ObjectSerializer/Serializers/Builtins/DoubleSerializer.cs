using System;
using System.IO;

namespace ObjectSerializer
{
	public class DoubleSerializer : SpecificSerializer<double>
	{
		public DoubleSerializer(Serializers s)
			: base(s){}
		protected override void Serialize (System.IO.Stream stream, double item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}
		protected override double Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadDouble();
		}
	}
}

