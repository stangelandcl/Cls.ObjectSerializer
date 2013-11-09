using System;
using Fasterflect;
using System.Reflection;
using System.Linq;

namespace ObjectSerializer
{
	public class ClassSerializer : SpecificSerializer<object> 
	{
		public ClassSerializer (Serializers serializer, Type type)
			: base(serializer)  { 
			this.type = type;
			this.properties = type.Properties (Flags.InstancePublic)
				.Where (n => n.CanRead && n.CanWrite)
				.ToArray ();
			this.serializers = properties.Select (n => serializer.FromDeclared (n.PropertyType)).ToArray ();
		}
		Type type;
		PropertyInfo[] properties;
		ISerializer[] serializers;

		#region implemented abstract members of SpecificSerializer
		protected override void Serialize (System.IO.Stream stream, object item)
		{
			for (int i=0; i<properties.Length; i++)
				serializers [i].Serialize (stream, properties[i].GetValue(item, null));
		}
		protected override object Deserialize (System.IO.Stream stream)
		{
			var obj = type.CreateInstance ();
			for (int i=0; i<properties.Length; i++)
				properties [i].Set (obj, serializers [i].Deserialize (stream));
			return obj;
		}
		#endregion
	}
}