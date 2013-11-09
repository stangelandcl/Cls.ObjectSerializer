using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ObjectSerializer
{
    class ByteArraySerializer : ISerializer
    {
        public void Serialize(System.IO.Stream stream, object item)
        {
			if (item == null) {
				ZigZag.Serialize (stream, 1);
				return;
			}
			var array = (byte[])item;
			ZigZag.Serialize (stream, (long)array.Length << 1);
			stream.Write (array, 0, array.Length);           
        }

        public object Deserialize(System.IO.Stream stream)
        {
			long count = ZigZag.DeserializeInt64 (stream);
			if(count == 1) return null;
			int c = (int)(count >> 1);
			var bytes = new byte[c];
			int offset = 0;
			do {
				int read = stream.Read (bytes, offset, bytes.Length - offset);
				offset += read;
			} while(offset != bytes.Length);
			return bytes;
        }			
   	}
}
