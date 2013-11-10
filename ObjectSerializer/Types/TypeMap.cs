using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class TypeMap{
		TwoWayDictionary<string,Type> map = new TwoWayDictionary<string, Type>();
		TwoWayDictionary<uint, Type> intMap = new TwoWayDictionary<uint, Type>();
		uint topId = 1;

		public static bool RequiresTag(Type t){
			return !t.IsSealed && !t.IsValueType;
		}

		public void Add(uint tag, Type type){
			intMap.Add (tag, type);
		}

		public KeyValuePair<uint, Type>[] TypeTags{
			get{ return intMap.Items1; }
		}

		public uint ToUInt32(Type type){
			return intMap.Get (type, n => topId++);
		}

		public Type GetType(uint id){
			return intMap.Get (id).Value;
		}

		public string ToString(Type type){
			return map.Get (type, n => n.AssemblyQualifiedName);
		}

		public Type GetType(string name){
			return map.Get(name, x=>{
				var t = Type.GetType(x);
				if(t == null) throw new Exception("Type not found " + x);
				return t;
			});
		}
	}
}

