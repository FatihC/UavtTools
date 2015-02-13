using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Ionic.Zlib;

namespace UAVTWebapi.Utils {
    public static class Utilities {

        public static long DateTimeNowLong() {
            long val = default(long);
            if(long.TryParse(DateTime.Now.ToString("yyyyMMddHHmmss"),out val)) { }

            return val;

        }

        public static long ConvertToLong(this DateTime dt) {
            long val = default(long);
            if(long.TryParse(DateTime.Now.ToString("yyyyMMddHHmmss"),out val)) { }

            return val;

        }

        public static byte[] DeflateByte(byte[] source) {
            if(source == null) {
                return null;
            }

            using(var ms = new MemoryStream()) {
                using(var compressor = new DeflateStream(ms,CompressionMode.Compress,CompressionLevel.BestSpeed)) {
                    compressor.Write(source,0,source.Length);
                }
                return ms.ToArray();
            }
        }

    }
}