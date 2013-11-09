using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int64Serializer : SpecificSerializer<long>
	{
		public Int64Serializer(Serializers s)
		: base(s){}

		#region ISerializer implementation

		protected override void Serialize (System.IO.Stream stream, long item)
		{
			ZigZag.Serialize (stream, item);
		}

		protected override long Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeInt64 (stream);
		}

		#endregion


	}
}

