using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using Fasterflect;
using System.Text;

namespace ObjectSerializer
{
	public class Serializers
	{
		public Serializers(){
			serializers = new Dictionary<Type, ISerializer> ();

			Add <byte[]>(new ByteArraySerializer ())
			.Add<string> (new StringSerializer ())
			.Add <sbyte>(new Int8Serializer ())
			.Add<short> (new Int16Serializer ())
			.Add <int>(new Int32Serializer ())
			.Add<long> (new Int64Serializer ())
			.Add <byte>(new UInt8Serializer ())
			.Add<ushort> (new UInt16Serializer ())
			.Add <uint>(new UInt32Serializer ())
			.Add <ulong>(new UInt64Serializer ())
			.Add<float> (new FloatSerializer ())
			.Add <double>(new DoubleSerializer ())
			.Add <Guid>(new GuidSerializer ())
			.Add <decimal>(new DecimalSerializer ())
			.Add <TimeSpan>(new TimeSpanSerializer ())
			.Add <bool>(new BoolSerializer ())
			.Add<DateTime> (new DateTimeSerializer ());

			Tagged = new UnknownTypeSerializer(this, types);
		}

		public Serializers(Dictionary<Type, ISerializer> map){
			serializers = map;
			Tagged = new UnknownTypeSerializer(this, types);
		}

		public Serializers CopyNoTags(){
			return new Serializers(serializers);
		}
		public Serializers CopyNewTags(IEnumerable<KeyValuePair<uint,Type>> types){
			var s = new Serializers(serializers);
			s.Add (types);
			return s;
		}
		public void Add(IEnumerable<KeyValuePair<uint, Type>> types){
			foreach (var type in types)
				this.types.Add (type.Key, type.Value);
		}

		public UnknownTypeSerializer Tagged { get; private set; }

		public void SerializeTypeMap(Stream stream){
			var tags = types.TypeTags;
			ZigZag.Serialize (stream, (uint)tags.Length);
			foreach (var kvp in tags) {
				ZigZag.Serialize (stream, kvp.Key);
				var name = types.ToString (kvp.Value);
				stream.Write (name);
			}
		}

		public KeyValuePair<uint,Type>[] DeserializeTypeMap(Stream stream){
			int count = (int)ZigZag.DeserializeUInt32 (stream);
			var types = new KeyValuePair<uint,Type>[count];
			for(int i=0;i<types.Length;i++) {
				uint tag = ZigZag.DeserializeUInt32 (stream);
				var typeName = stream.ReadString ();
				var type = this.types.GetType (typeName);
				types [i] = new KeyValuePair<uint, Type> (tag, type);
			}	
			return types;
		}

		TypeMap types = new TypeMap();
		Dictionary<Type, ISerializer> serializers;



		Serializers Add<T>(ISerializer s){
			serializers.Add (typeof(T), s);
			return this;
		}


		public ISerializer FromDeclared(Type t){
			if (t.IsValueType || t.IsSealed)
				return Get(t);
			return Tagged;
		}

		public ISerializer Get(Type t){
			lock (serializers) 
				return GetInternal (t);		
		}

		ISerializer GetInternal (Type t)
		{
			ISerializer s;
			if (serializers.TryGetValue (t, out s))
				return s;
			if (t.IsArray)
				return serializers [t] = new ArraySerializer (this, t);
			if (IsGenericEnumerable (t)) {
				if (t.GetGenericTypeDefinition () == typeof(Dictionary<, >))
					return serializers [t] = new DictionarySerializer (this, t);
				return serializers [t] = new EnumerableSerializer (this, t);
			}
			if (!t.IsValueType)
				return serializers [t] = new ClassSerializer (this, t);
			if (t.IsGenericType && typeof(KeyValuePair<, >) == t.GetGenericTypeDefinition ())
				return serializers [t] = new KeyValuePairSerializer (this, t);
			return serializers [t] = new StructSerializer (this, t);
		}

		static bool IsGenericEnumerable(Type t){
			return t.IsGenericType && typeof(IEnumerable).IsAssignableFrom (t);
		}
	}
}

