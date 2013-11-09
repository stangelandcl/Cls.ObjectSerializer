using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int16Serializer : SpecificSerializer<short>
	{
		public Int16Serializer(Serializers s)
			: base(s){}

		#region ISerializer implementation

		protected override void Serialize (System.IO.Stream stream, short item)
		{
			ZigZag.Serialize (stream, item);
		}

		protected override short Deserialize (System.IO.Stream stream)
		{
			return (short)ZigZag.DeserializeInt32 (stream);
		}

		#endregion


	}
}



