using System;

namespace ObjectSerializer
{
	public class MultiTypeMap : TypeMap
	{
		public MultiTypeMap (TypeMap types)
		{
			this.types = types;
		}
		TypeMap types;

		public override string ToString (Type type)
		{
			return nameMap.Get (type, n => types.ToString (type));
		}
	}
}

