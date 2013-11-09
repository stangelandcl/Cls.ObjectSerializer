
using System.IO;

namespace ObjectSerializer
{
	public class Int8Serializer : ISerializer
	{
		public void Serialize (System.IO.Stream stream, object item)
		{
			stream.WriteByte ((byte)(sbyte)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return (sbyte)stream.ReadByte ();
		}
	}
}
