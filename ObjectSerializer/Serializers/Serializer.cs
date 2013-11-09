using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class Serializer : ISerializer
	{
		Serializers s = new Serializers();

		public void Serialize<T>(System.IO.Stream stream, T item)
		{
			s.Tagged.Serialize(stream, item);
		}

		public T Deserialize<T>(System.IO.Stream stream)
		{
			return s.Tagged.Deserialize<T>(stream);
		}

		public object Deserialize(Stream stream){
			return s.Tagged.Deserialize (stream);
		}
	}
}

