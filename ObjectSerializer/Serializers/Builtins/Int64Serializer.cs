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
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}

		protected override long Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadInt64();
		}

		#endregion


	}
}

