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
			TypeMap = new TypeMap ();

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

			Tagged = new UnknownTypeSerializer(this, TypeMap);
			TypeMapSerializer = new TypeMapSerializer (TypeMap);

		}

		public Serializers(Dictionary<Type, ISerializer> map, TypeMap types){
			this.serializers = map;
			this.TypeMap = types;
			Tagged = new UnknownTypeSerializer(this, types);
			TypeMapSerializer = new TypeMapSerializer (types);
		}

		public Serializers CopyNoTags(){
			return new Serializers(serializers, new TypeMap());
		}
		public Serializers CopyNewTags(TypeMap types){
			return new Serializers(serializers, types);
		}
		public void Add(IEnumerable<KeyValuePair<uint, Type>> types){
			foreach (var type in types)
				this.TypeMap.Add (type.Key, type.Value);
		}

		public UnknownTypeSerializer Tagged { get; private set; }
		public TypeMapSerializer TypeMapSerializer {get; private set;}	
		public TypeMap TypeMap {get; private set;}
		Dictionary<Type, ISerializer> serializers;
		SynchronizedCollection<Func<Serializers,Type,ISerializer>> lazy = 
			new SynchronizedCollection<Func<Serializers, Type, ISerializer>> ();			

		public void AddSerializer(Func<Serializers,Type,ISerializer> newObj){
			lazy.Add (newObj);
		}


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

			foreach (var x in lazy) {
				s = x (this, t);
				if(s != null) return serializers[t] = s;
			}


			if (t.IsArray)
				return serializers [t] = new ArraySerializer (this, t);
			if (IsGenericEnumerable (t)) {
				if (t.GetGenericTypeDefinition () == typeof(Dictionary<, >))
					return serializers [t] = new DictionarySerializer (this, t);
				return serializers [t] = new EnumerableSerializer (this, t);
			}
			if (!t.IsValueType) {
				if (typeof(Expression).IsAssignableFrom (t))
					return serializers [t] = new ExpressionSerializer (this);
				return serializers [t] = new ClassSerializer (this, t);
			}
			if (t.IsGenericType && typeof(KeyValuePair<, >) == t.GetGenericTypeDefinition ())
				return serializers [t] = new KeyValuePairSerializer (this, t);
			if(t.IsEnum)
				return serializers[t] = new EnumSerializer(this, t);
			return serializers [t] = new StructSerializer (this, t);
		}

		static bool IsGenericEnumerable(Type t){
			return t.IsGenericType && typeof(IEnumerable).IsAssignableFrom (t);
		}
	}
}

