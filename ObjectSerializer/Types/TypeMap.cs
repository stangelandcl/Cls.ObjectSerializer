using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjectSerializer
{
	public class TypeMap{
		TwoWayDictionary<string,Type> map = new TwoWayDictionary<string, Type>();
		TwoWayDictionary<int, Type> intMap = new TwoWayDictionary<int, Type>();
		int topId = 1;

		public int ToInt32(Type type){
			return intMap.Get (type, n => topId++);
		}

		public Type GetType(int id){
			return intMap.Get (id).Value;
		}

		public string ToString(Type type){
			return map.Get(type, n=>{
				var assembly = n.Assembly.GetName().Name;
				return assembly + ":" + n.Name;	
			});
		}

		public Type GetType(string name){
			return map.Get(name, x=>{
				var split = x.Split(':');
				var assemblyName = split[0];
				var typeName = split[1];
				var t = AppDomain.CurrentDomain.GetAssemblies()
					.Where(n=>n.GetName().Name == assemblyName)
					.SelectMany(n=>n.GetTypes())
					.FirstOrDefault(n=>n.Name == typeName);				
				if(t == null) throw new Exception("Type not found " + x);
				return t;
			});
		}
	}
}

