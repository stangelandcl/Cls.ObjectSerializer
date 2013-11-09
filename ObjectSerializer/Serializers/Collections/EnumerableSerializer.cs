using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Linq;
using System;

namespace ObjectSerializer
{
	class EnumerableSerializer<T, U> : SpecificSerializer<T> where T : IEnumerable<U>
	{
		public EnumerableSerializer(Serializers s)
			: base(s) {}

		protected override void Serialize(System.IO.Stream stream, T item)
		{
			var w = new BinaryWriter(stream);
			w.Write(item.Count());
			w.Flush ();
			var s = serializer.Get (typeof(U));
			foreach (var i in item)
				s.Serialize (i);
		}

		protected override T Deserialize(System.IO.Stream stream)
		{
			var r = new BinaryReader(stream);
			int count = r.ReadInt32();
			var s = serializer.Get (typeof(U));
			throw new NotImplementedException ();
		}			
	}
}