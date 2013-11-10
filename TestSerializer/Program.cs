using System;

namespace TestSerializer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var test = new TestSerializers ();
			test.Test ();
			test.TestMessages ();
			test.TestPerformance ();
		}
	}
}
