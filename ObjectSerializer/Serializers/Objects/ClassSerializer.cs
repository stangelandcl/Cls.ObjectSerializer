using System;
using Fasterflect;
using System.Reflection;
using System.Linq;
using System.Runtime.Serialization;

namespace ObjectSerializer
{
	public class ClassSerializer : ISerializer
	{
		public ClassSerializer (Serializers serializer, Type type)
		{ 
			this.type = type;
			var properties = type.Properties (Flags.InstancePublic)
				.Where (n => !serializer.IsIgnored(n)).ToArray ();

			this.getters = properties.Select (n => n.DelegateForGetPropertyValue ()).ToArray ();
			this.setters = properties.Select (n => n.DelegateForSetPropertyValue ()).ToArray ();
			this.serializers = properties.Select (n => serializer.FromDeclared (n.PropertyType)).ToArray ();
			this.newObj = this.type.DelegateForCreateInstance ();
		}
		Type type;
		MemberGetter[] getters;
		MemberSetter[] setters;
		ISerializer[] serializers;
		ConstructorInvoker newObj;

		#region implemented abstract members of SpecificSerializer
		public void Serialize (System.IO.Stream stream, object item)
		{
			for (int i=0; i<getters.Length; i++)
				serializers [i].Serialize (stream, getters[i](item));
		}
		public object Deserialize (System.IO.Stream stream)
		{
			var obj = newObj();
			DeserializeInto (stream, obj);
			return obj;
		}

		public void DeserializeInto (System.IO.Stream stream, object obj)
		{
			for (int i = 0; i < setters.Length; i++) {
				var s = serializers [i].Deserialize (stream);
				setters [i] (obj, s);
			}
		}
		#endregion
	}
}