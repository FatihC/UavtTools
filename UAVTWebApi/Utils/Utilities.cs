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

        public static string NormalizeForEnglish(string countyName) {
            var turkishChars = new[] { "Ö","Ç","İ","Ğ","Ü","Ş","ö","ç","ı","ğ","ü","ş" };
            var turkishCharsEquivalent = new[] { "O","C","I","G","U","S","o","c","i","g","u","s" };

            for(int i = 0; i < turkishChars.Length; i++) {
                countyName = countyName.Replace(turkishChars[i],turkishCharsEquivalent[i]);
            }

            return countyName;
        }

        public static string LowercaseFirstLetter(string value) {
            var firstLetter = value.First();
            var normalizedFirstLetter = NormalizeForEnglish(firstLetter.ToString());
            var normalizedRest = NormalizeForEnglish(value.Substring(1,value.Length - 1));
            return normalizedFirstLetter.ToLower() + normalizedRest;
        }

        public static string UppercaseFirstLetter(string value)
        {
            var firstLetter = value.First();
            var normalizedFirstLetter = NormalizeForEnglish(firstLetter.ToString());
            var normalizedRest = NormalizeForEnglish(value.Substring(1,value.Length - 1));
            return normalizedFirstLetter.ToUpper() + normalizedRest;
        }

    }
}