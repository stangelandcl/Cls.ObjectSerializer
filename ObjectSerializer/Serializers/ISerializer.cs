using System;
using System.IO;

namespace ObjectSerializer
{
    public interface ISerializer
	{
        void Serialize(Stream stream, object item);
        object Deserialize(Stream stream);
	}

	public interface ISerializer<T>
	{
		void Serialize(Stream stream, T item);
		T Deserialize(Stream stream);
	}

    public static class SerializerExtensions
    {
        public static ArraySegment<byte> SerializeToBytes<T>(this ISerializer serializer, T item)
        {
            var ms = new MemoryStream();
            serializer.Serialize(ms, item);			
            return new ArraySegment<byte>(ms.GetBuffer(), 0, (int)ms.Position);
        }
        public static byte[] Serialize<T>(this ISerializer serializer, T item)
        {
            var ms = new MemoryStream();
            serializer.Serialize(ms, item);
            var buf = ms.GetBuffer();
            Array.Resize(ref buf, (int)ms.Position);
            return buf;
        }
        public static T Deserialize<T>(this ISerializer serializer, ref ArraySegment<byte> bytes)
        {
            var ms = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count);
            return (T)serializer.Deserialize(ms);
        }
        public static object Deserialize(this ISerializer serializer, ref ArraySegment<byte> bytes)
        {
            var ms = new MemoryStream(bytes.Array, bytes.Offset, bytes.Count);
            return serializer.Deserialize(ms);
        }
        public static T Deserialize<T>(this ISerializer serializer, byte[] bytes)
        {            
            return (T)serializer.Deserialize(new MemoryStream(bytes));
        }

		public static object Deserialize(this ISerializer serializer, byte[] bytes)
		{
			return serializer.Deserialize(new MemoryStream(bytes));
		}
    }
}

