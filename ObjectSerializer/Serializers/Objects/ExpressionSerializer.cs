using System;
using System.Linq.Expressions;

namespace ObjectSerializer
{
	public class ExpressionSerializer : ISerializer
	{
		public ExpressionSerializer(Serializers s){
			serializer = new Serialize.Linq.Serializers.ExpressionSerializer (
				new ExprSerializer(s));
		}

		Serialize.Linq.Serializers.ExpressionSerializer serializer;

		class ExprSerializer : Serialize.Linq.Interfaces.ISerializer
		{
			public ExprSerializer(Serializers s){
				this.s = s;
			}
			Serializers s;
			public void Serialize<T> (System.IO.Stream stream, T obj)
			{
				s.Tagged.Serialize (stream, obj);
			}
			public T Deserialize<T> (System.IO.Stream stream)
			{
				return (T)s.Tagged.Deserialize (stream);
			}
		}

		public void Serialize (System.IO.Stream stream, object item)
		{
			serializer.Serialize (stream, (Expression)item);
		}
		public object Deserialize (System.IO.Stream stream)
		{
			return serializer.Deserialize (stream);
		}
	}
}

