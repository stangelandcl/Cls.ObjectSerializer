using System;
using NUnit.Framework;
using System.Collections.Generic;
using ObjectSerializer;

namespace TestSerializer
{
	[TestFixture]
	public class TestSerializers
	{
		[Test]
		public void Test ()
		{
			var t = CreateTestItem ();

			var serializer = new Serializer ();
			var bytes = serializer.Serialize(t);
			var t2 = (Test1)serializer.Deserialize (bytes);

			Assert.AreEqual (t.Name, t2.Name);
			Assert.AreEqual (t.Id, t2.Id);
			CollectionAssert.AreEqual (t.T2.Items, t2.T2.Items);
			CollectionAssert.AreEqual (t.T2.Data, t2.T2.Data);
		}

		[Test]
		public void TestMessages(){
			var t = CreateTestItem ();

			var serializer1 = new Serializer ();
			var bytes = serializer1.SerializeMessage(t);
			var serializer2 = new Serializer ();
			var t2 = (Test1)serializer2.DeserializeMessage(bytes);

			Assert.AreEqual (t.Name, t2.Name);
			Assert.AreEqual (t.Id, t2.Id);
			CollectionAssert.AreEqual (t.T2.Items, t2.T2.Items);
			CollectionAssert.AreEqual (t.T2.Data, t2.T2.Data);
		}

		class Test1{
			public int Id {get;set;}
			public string Name { get; set; }
			public ITest2 T2 {get;set;}
			public ITest2 T3 {get;set;}
		}

		interface ITest2{
			List<int> Items {get;set;}
			byte[] Data {get;set;}
			object Unknown {get;set;}
		}

		class Test2 : ITest2{
			public List<int> Items {get;set;}
			public byte[] Data {get;set;}
			public object Unknown {get;set;}
			public IDictionary<string,int> Map {get;set;}
		}
		class Test3: ITest2{
			public List<int> Items {get;set;}
			public byte[] Data {get;set;}
			public object Unknown {get;set;}
			public IEnumerable<double> En {get;set;}
			public List<Test4> Values {get;set;}
		}

		struct Test4{
			public string Value {get;set;}
			public bool YesNo {get;set;}
		}

		static Test1 CreateTestItem ()
		{
			var t = new Test1 {
				Id = 1240,
				Name = "123al.hdxul.",
				T2 = new Test2 {
					Items = new List<int> {
						1,
						2,
						3,
						4
					},
					Data = new byte[] {
						12,
						87,
						12,
						2,
						99
					},
					Unknown = new Test4 {
						Value = "lrgcxl[92g3x[9gx",
						YesNo = true,
					},
					Map = new Dictionary<string, int> {
						{
							"shbxlrgxlgx",
							388
						}
					},
				},
				T3 = new Test3 {
					Values = new List<Test4> {
						new Test4 ()
					},
					En = new double[] {
						138.997,
						89870.08,
						896008607.00009
					},
				},
			};
			return t;
		}

	}
}

