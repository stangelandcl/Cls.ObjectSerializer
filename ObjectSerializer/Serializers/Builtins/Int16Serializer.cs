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
			var w = new BinaryWriter(stream);
			w.Write(item);
			w.Flush();
		}

		protected override short Deserialize (System.IO.Stream stream)
		{
			return new BinaryReader(stream).ReadInt16();
		}

		#endregion


	}
}



