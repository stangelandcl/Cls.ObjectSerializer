using System;
using System.IO;

namespace ObjectSerializer
{
	public abstract class SpecificSerializer<T> : ISerializer
	{
		public SpecificSerializer (Serializers serializer)
		{
			this.serializer = serializer;
		}
		protected Serializers serializer;
		protected abstract void Serialize(Stream stream, T item);
		public abstract object Deserialize(Stream stream);


		#region ISerializer implementation

		public void Serialize (System.IO.Stream stream, object item)
		{
			Serialize(stream, (T)item);
		}

		#endregion
	}
}

