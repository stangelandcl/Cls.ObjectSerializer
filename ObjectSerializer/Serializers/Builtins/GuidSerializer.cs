using System;
using System.IO;

namespace ObjectSerializer
{
	public class GuidSerializer : SpecificSerializer<Guid>
	{
		public GuidSerializer(Serializers s) : base(s){}
		protected override void Serialize (System.IO.Stream stream, Guid item)	{
			var w = new BinaryWriter(stream);
			w.Write (item.ToByteArray ());
			w.Flush();
		}
		protected override Guid Deserialize (System.IO.Stream stream)
		{
			return new Guid (new BinaryReader (stream).ReadBytes (16));
		}
	}
}


