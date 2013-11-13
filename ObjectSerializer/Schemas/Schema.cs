using System;
using System.Collections.Generic;
using System.Reflection;

namespace ObjectSerializer
{
	public class Schema{}
	public struct Property{
		public string Name;
		public Schema Schema;
	}
	public class ObjectSchema : Schema{}
	public class ClassSchema : Schema{
		public List<Property> Properties {get;set;}		
	}
	public class PrimitiveSchema: Schema{
		public Type Type {get;set;}
	}
	public class MapSchema{
		public Schema Key { get; set; }
		public Schema Value { get; set; }
	}
	public class ArraySchema{
		public Schema ElementType { get; set; }
	}
}

