using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class TypeMap{
		protected TwoWayDictionary<string,Type> nameMap = new TwoWayDictionary<string, Type>();
		protected TwoWayDictionary<uint, Type> intMap = new TwoWayDictionary<uint, Type>();
		protected uint topId = 1;

		public static bool RequiresTag(Type t){
			return !t.IsSealed && !t.IsValueType;
		}

		public void Add(uint tag, Type type){
			intMap.Add (tag, type);
		}

		public void Add(uint tag, string name, Type type){
			intMap.Add (tag, type);
			nameMap.Add (name, type);
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

		public virtual string ToString(Type type){
			return nameMap.Get (type, n => GetName(n));
		}

		string GetName(Type type){
			var split = type.AssemblyQualifiedName.Split (',');
			return string.Join (",", split.Where (n => 
			                                      !n.StartsWith (" Version=") &&
			                                      !n.StartsWith(" Culture=") &&
			                                      n != " PublicKeyToken=null"));
		}

		public Type GetType(string name){
			return nameMap.Get(name, x=>{
				var t = Type.GetType(x);
				if(t == null) throw new Exception("Type not found " + x);
				return t;
			});
		}
	}
}

