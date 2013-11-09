using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using Fasterflect;

namespace ObjectSerializer
{
	public class Serializers
	{
		TypeMap types = new TypeMap();
		Dictionary<Type, ISerializer> serializers = new Dictionary<Type, ISerializer> ();

		public ISerializer Tagged {get; private set;}
		//		public ISerializer Untagged { get; private set; }

		Serializers Add<T>(ISerializer s){
			serializers.Add (typeof(T), s);
			return this;
		}

		public Serializers(){
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
			//Untagged = new ClassSerializer (this, types);
#if EXPR
			serializers.Add(new KeyValuePair<Type, ISerializer>(
				typeof(Expression), (ISerializer)new ExprSerializer(this)));
#endif
//			serializers.Add(new KeyValuePair<Type, ISerializer>(
//				typeof(object), NamedTypeSerializer));		
#if EXPR
			serializers.Add(new KeyValuePair<Type, ISerializer>(
				(Type)null, (ISerializer)new JsonSerializer()));
#endif

//			serializerMap = serializers.Take(serializers.Count()-1)
//				.ToDictionary(n=>n.Key, n=>n.Value);
		}

		public ISerializer FromDeclared(Type t){
			if (t.IsValueType || t.IsSealed)
				return Get(t);
			return Tagged;
		}

		public ISerializer Get(Type t){
			ISerializer s;
			if(serializers.TryGetValue(t, out s))
				return s;
			if (t.IsArray)
				return serializers [t] = new ArraySerializer (this, t);
			if (IsGenericEnumerable (t)) {
				if (t.GetGenericTypeDefinition () == typeof(Dictionary<,>))
					return serializers [t] = new DictionarySerializer (this, t);
				return serializers [t] = new EnumerableSerializer (this, t);
			}
			if (!t.IsValueType)
				return serializers [t] = new ClassSerializer (this, t);
			if (t.IsGenericType && typeof(KeyValuePair<,>) == t.GetGenericTypeDefinition ())
				return serializers [t] = new KeyValuePairSerializer (this, t);
			return serializers [t] = new StructSerializer (this, t);
		}

		static bool IsGenericEnumerable(Type t){
			return t.IsGenericType && typeof(IEnumerable).IsAssignableFrom (t);
		}
	}
}

