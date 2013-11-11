using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class Serializer : ISerializer
	{
		Serializers serializer = new Serializers();

        /// <summary>
        /// Default serializer. Use this unless there is a good reason not to.
        /// Serializers cache reflection.emit getters and settings, type tags, 
        /// type to serializer maps, constructors and maybe other things. Recreating
        /// a serializer will lose all that and make serialization much slower.
        /// </summary>
		public static Serializer Default = new Serializer();

        /// <summary>
        /// Inject a specific serializer for a type. Return null to ignore to indicate
        /// the serializer does not apply and return a serializer object for any type that does apply
        /// </summary>
        /// <param name="use"></param>
		public void AddSerializer(Func<Serializers, Type, ISerializer> use){
			serializer.AddSerializer (use);
		}

        /// <summary>
        /// Serialize an object. Types are tagged with numbers, but the 
        /// tag to type mapping must be save separately and reloaded into the deserializer
        /// before trying to deserialize the message. Unless the same serializer is used 
        /// during the same application run. In that case the types will be found.
        /// </summary>      
		public void Serialize(System.IO.Stream stream, object item)
		{
			serializer.Tagged.Serialize(stream, item);
		}

        /// <summary>
        /// Deserialize an object. Types are tagged with numbers, but the 
        /// tag to type mapping must be loaded before calling this method. Unless the same serializer is used 
        /// during the same application run. In that case the types will be found.
        /// </summary>      
		public object Deserialize(System.IO.Stream stream)
		{
			return serializer.Tagged.Deserialize(stream);
		}

        /// <summary>
        /// Deserialize a self contained message. All type information is included.
        /// </summary>    
		public object DeserializeMessage(byte[] bytes){
			return DeserializeMessage (new MemoryStream (bytes));
		}

        /// <summary>
        /// Deserialize a self contained message. All type information is included.
        /// </summary>    
		public object DeserializeMessage(Stream stream){
			var types = (TypeMap)this.serializer.TypeMapSerializer.Deserialize (stream);
			var serializer = this.serializer.CopyNewTags (types);
			return serializer.Tagged.Deserialize (stream);
		}

        /// <summary>
        /// Serialize a self contained message. All type information is included.
        /// </summary>    
		public void SerializeMessage(Stream stream, object item){
			var serializer = this.serializer.CopyNoTags ();
			var ms = new MemoryStream ();
			serializer.Tagged.Serialize (ms, item);
			serializer.TypeMapSerializer.Serialize (stream, serializer.TypeMap);
			var buf = ms.GetBuffer ();
			stream.Write (buf, 0, (int)ms.Position);
		}

        /// <summary>
        /// Serialize a self contained message. All type information is included.
        /// </summary>       
		public byte[] SerializeMessage(object item){
			var ms = new MemoryStream ();
			SerializeMessage (ms, item);
			var array = ms.GetBuffer ();
			Array.Resize (ref array, (int)ms.Position);
			return array;
		}
	}
}

