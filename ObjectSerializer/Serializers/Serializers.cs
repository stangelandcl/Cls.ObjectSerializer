using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ObjectSerializer
{
	public class Serializers
	{
		TypeMap types = new TypeMap();
		Dictionary<Type, ISerializer> serializers = new Dictionary<Type, ISerializer> ();

		public ISerializer Tagged {get; private set;}
		//		public ISerializer Untagged { get; private set; }

		Serializers Add<T>(SpecificSerializer<T> s){
			serializers.Add (typeof(T), s);
			return this;
		}

		public Serializers(){
			Add (new ByteArraySerializer (this))
			.Add (new StringSerializer (this))
			.Add (new Int8Serializer (this))
			.Add (new Int16Serializer (this))
			.Add (new Int32Serializer (this))
			.Add (new Int64Serializer (this))
			.Add (new UInt8Serializer (this))
			.Add (new UInt16Serializer (this))
			.Add (new UInt32Serializer (this))
			.Add (new UInt64Serializer (this))
			.Add (new FloatSerializer (this))
			.Add (new DoubleSerializer (this))
			.Add (new GuidSerializer (this))
			.Add (new DecimalSerializer (this))
			.Add (new TimeSpanSerializer (this));

			Tagged = new TaggedSerializer(this, types);
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

		public ISerializer Get(Type t){
			ISerializer s;
			if(serializers.TryGetValue(t, out s))
				return s;
			if (!t.IsValueType)
				return serializers [t] = new ClassSerializer (this, t);
			return serializers [t] = new StructSerializer (this, t);
		}

		public ISerializer GetOrTagged(Type t){
			ISerializer s;
			if(serializers.TryGetValue(t, out s))
				return s;
			return Tagged;
		}
	}
}

