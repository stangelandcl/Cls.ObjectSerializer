using System;
using System.IO;

namespace ObjectSerializer
{
	public class DateTimeSerializer : SpecificSerializer<DateTime>
	{
		public DateTimeSerializer(Serializers s) : base(s){}
		protected override void Serialize (System.IO.Stream stream, DateTime item)	{
			var w = new BinaryWriter(stream);
			w.Write (item.ToBinary());
			w.Flush();
		}
		protected override DateTime Deserialize (System.IO.Stream stream)
		{
			return DateTime.FromBinary (new BinaryReader (stream).ReadInt64 ());
		}
	}
}

