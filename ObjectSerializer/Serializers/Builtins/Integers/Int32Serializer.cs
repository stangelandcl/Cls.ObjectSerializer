using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int32Serializer : SpecificSerializer<int>
	{
		public Int32Serializer(Serializers s)
		: base(s){}

		#region ISerializer implementation

		protected override void Serialize (System.IO.Stream stream, int item)
		{
			ZigZag.Serialize (stream, item);
		}

		protected override int Deserialize (System.IO.Stream stream)
		{
			return ZigZag.DeserializeInt32 (stream);
		}

		#endregion


	}
}

