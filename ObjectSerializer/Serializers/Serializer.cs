using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class Serializer : ISerializer
	{
		Serializers s = new Serializers();

		public void Serialize(System.IO.Stream stream, object item)
		{
			s.Tagged.Serialize(stream, item);
		}

		public object Deserialize(System.IO.Stream stream)
		{
			return s.Tagged.Deserialize(stream);
		}
	}
}

