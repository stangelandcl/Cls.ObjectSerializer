using System;
using System.IO;

namespace ObjectSerializer
{
	public class BoolSerializer : SpecificSerializer<bool>
	{
		public BoolSerializer(Serializers s) : base(s){}
		protected override void Serialize (System.IO.Stream stream, bool item)	{
			var w = new BinaryWriter(stream);
			w.Write (item);
			w.Flush();
		}
		protected override bool Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader (stream).ReadBoolean ();
		}
	}
}