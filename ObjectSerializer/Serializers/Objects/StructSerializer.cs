using System;
using Fasterflect;
using System.Reflection;
using System.Linq;

namespace ObjectSerializer
{
	public class StructSerializer : ISerializer
	{
		public StructSerializer (Serializers serializer, Type type)
	    { 
			var properties = type.Properties (Flags.InstancePublic)
				.Where (n => !serializer.IsIgnored(n)).ToArray ();
			getters = properties.Select (n => n.DelegateForGetPropertyValue ()).ToArray ();
			setters = properties.Select (n => n.DelegateForSetPropertyValue ()).ToArray ();
			this.serializers = properties.Select (n => serializer.FromDeclared (n.PropertyType)).ToArray ();
			this.newObj = type.DelegateForCreateInstance ();
		}
		MemberGetter[] getters;
		MemberSetter[] setters;
		ISerializer[] serializers;
		ConstructorInvoker newObj;

		#region implemented abstract members of SpecificSerializer
		public void Serialize (System.IO.Stream stream, object item)
		{
			var wrap = item.WrapIfValueType ();
			for (int i=0; i<getters.Length; i++)
				serializers [i].Serialize (stream, getters[i] (wrap));
		}
		public object Deserialize (System.IO.Stream stream)
		{
			var obj = newObj ();
			var wrap = obj.WrapIfValueType ();
			for (int i=0; i<setters.Length; i++)
				setters [i](wrap, serializers [i].Deserialize (stream));
			return obj;
		}
		#endregion
	}
}

