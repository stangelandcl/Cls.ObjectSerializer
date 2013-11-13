using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ObjectSerializer
{
	public class SchemaMap
	{
		public SchemaMap(){
			AddPrimitive<sbyte> ();
			AddPrimitive<byte> ();
			AddPrimitive<short> ();
			AddPrimitive<ushort> ();
			AddPrimitive<int> ();
			AddPrimitive<uint> ();
			AddPrimitive<long> ();
			AddPrimitive<ulong> ();

			AddPrimitive<float> ();
			AddPrimitive<double> ();

			AddPrimitive<string> ();
			AddPrimitive<Guid> ();
			AddPrimitive<decimal> ();
			AddPrimitive<TimeSpan> ();
			AddPrimitive<DateTime> ();
			AddPrimitive<bool> ();

			AddPrimitive<byte[]> ();
		}

		void Add<T>(Schema s){
			map [typeof(T)] = s;
		}

		void AddPrimitive<T>(){
			map [typeof(T)] = new PrimitiveSchema { Type = typeof(T)};
		}

		Dictionary<Type, Schema> map = new Dictionary<Type, Schema> ();

		Schema PrimitiveToSchema(Type type){
			Schema s;
			map.TryGetValue (type, out s);
			return s;
		}

		public Schema GetSchema(Type type){
			lock (map) {
				var s = PrimitiveToSchema (type);
				if (s != null)
					return s;
				return map [type] = Create (type);
			}
		}	

		Schema Create(Type type){
			var properties = type.GetProperties (BindingFlags.Public | BindingFlags.Instance)
				.Where (n => n.CanRead && n.CanWrite)
				.OrderBy(n=>n.Name)
				.ToArray ();
			var s = new ClassSchema ();
			foreach (var p in properties)
				s.Properties.Add (new Property { Name = p.Name, Schema = GetSchema (p.PropertyType) });
			return s;
		}
	}
}

