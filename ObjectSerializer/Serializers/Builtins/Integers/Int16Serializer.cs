using System;
using System.IO;

namespace ObjectSerializer
{
	public class Int16Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			ZigZag.Serialize (stream, (short)item);
		}

		public object Deserialize (System.IO.Stream stream)
		{
			return (short)ZigZag.DeserializeInt32 (stream);
		}	
	}
}



