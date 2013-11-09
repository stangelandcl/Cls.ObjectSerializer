using System;
using System.IO;

namespace ObjectSerializer
{
	public class TimeSpanSerializer : SpecificSerializer<TimeSpan>
	{
		public TimeSpanSerializer(Serializers s) : base(s){}
		protected override void Serialize (System.IO.Stream stream, TimeSpan item)	{
			var w = new BinaryWriter(stream);
			w.Write (item.TotalMilliseconds);
			w.Flush();
		}
		protected override TimeSpan Deserialize (System.IO.Stream stream)
		{
			return TimeSpan.FromMilliseconds (new BinaryReader (stream).ReadDouble ());
		}
	}
}

