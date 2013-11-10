using System;
using NUnit.Framework;
using System.Collections.Generic;
using ObjectSerializer;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Linq.Expressions;

namespace TestSerializer
{
	[TestFixture]
	public class TestSerializers
	{
		[Test]
		public void Test ()
		{
			var t = CreateTestItem ();

			var serializer = Serializer.Default;
			var bytes = serializer.Serialize(t);
			var t2 = (Test1)serializer.Deserialize (bytes);

			var method = t2.Expr.Compile ();
			Assert.AreEqual (11, method (10));
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

		[Test]
		public void TestPerformance(){
			var t = CreatePerformanceTestItem ();
			const int count = 100000;
			var sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (t));
			Console.WriteLine (sw.Elapsed + " json");

			sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				Serializer.Default.SerializeMessage (t);
			Console.WriteLine (sw.Elapsed + " binary message");

			sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				Serializer.Default.Serialize(t);
			Console.WriteLine (sw.Elapsed + " binary reuse");

			var b1 = Encoding.UTF8.GetBytes (JsonConvert.SerializeObject (t));
			var b2 = Serializer.Default.SerializeMessage (t);
			var b3 = Serializer.Default.Serialize (t);

			Console.WriteLine ("json={0} message={1} reuse={2}", b1.Length, b2.Length, b3.Length);

			sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				JsonConvert.DeserializeObject (Encoding.UTF8.GetString (b1));
			Console.WriteLine (sw.Elapsed + " json");

			sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				Serializer.Default.DeserializeMessage (b2);
			Console.WriteLine (sw.Elapsed + " binary message");

			sw = Stopwatch.StartNew ();
			for (int i=0; i<count; i++)
				Serializer.Default.Deserialize(b3);
			Console.WriteLine (sw.Elapsed + " binary reuse");
		}

		class Test1{
			public int Id {get;set;}
			public string Name { get; set; }
			public ITest2 T2 {get;set;}
			public ITest2 T3 {get;set;}
			public Expression<Func<int,int>> Expr {get;set;}
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

		class Test5{
			public string Name {get;set;}
			public int Id {get;set;}
			public string Prop1 {get;set;}
			public string Prop2 {get;set;}
			public int Prop3 {get;set;}
			public double Prop4 {get;set;}
			public Test6 Prop5 {get;set;}
		}
		enum Test6 { A,B,C,D,E,F};

		static Test5 CreatePerformanceTestItem(){
			return new Test5{
				Name = "saotehrcdrgdbrb",
				Id = 1280256,
				Prop1 = "asoteuhsantoed",
				Prop2 = "asoteusbaoebiaobeiao",
				Prop3 = 812654,
				Prop4 = 8136508.096086,
				Prop5 = Test6.D,
			};
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
				Expr = n=> n + 1,
			};
			return t;
		}

	}
}

