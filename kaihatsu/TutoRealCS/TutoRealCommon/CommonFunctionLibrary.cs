using Microsoft.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Text;
using CC = TutoRealCommon.CommonConst;
using TutoRealBE;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Serialization;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TutoRealCommon
{
    public static class CommonFunctionLibrary
    {
        private static int drGetIndex(SqlDataReader dr, string fieldname)
        {
            int index = -1;
            try
            {
                index = dr.GetOrdinal(fieldname);
            }
            catch (Exception)
            {
                return index;
            }
            return index;
        }
        public static int ToI(string? data)
        {
            if (string.IsNullOrWhiteSpace(data)) return 0;

            int retValue = 0;
            int.TryParse(data, out retValue);

            return retValue;
        }
        public static string ToS(object data)
        {
            return data is null ? string.Empty : data.ToString();
        }
        public static string ToS(SqlDataReader dr, string fieldname)
        {
            int index = drGetIndex(dr, fieldname);
            string retValue = string.Empty;
            if (index >= 0)
            {
                retValue = dr.IsDBNull(index) ? string.Empty : dr.GetString(index);
            }
            return retValue;
        }
        public static DateTime? ToDateTime(SqlDataReader dr, string fieldname)
        {
            int index = drGetIndex(dr, fieldname);
            DateTime? retValue = null;
            if (index >= 0)
            {
                retValue = dr.IsDBNull(index) ? null : dr.GetDateTime(index);
            }
            return retValue;
        }
        public static string ToYMD(SqlDataReader dr, string fieldname)
        {
            int index = drGetIndex(dr, fieldname);
            string retValue = string.Empty;
            if (index >= 0)
            {
                retValue = dr.IsDBNull(index) ? string.Empty : string.Format(CommonConst.FMT_YMD, dr.GetDateTime(index));
            }
            return retValue;
        }

        public static string ToHash(string inputData)
        {
            using (SHA256 hash = SHA256.Create())
            {
                // ComputeHash - バイト配列を返します
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(inputData));

                // バイト配列を16進数の文字列に変換します。
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                // 16進数の文字列を返します。
                return builder.ToString().ToUpper();
            }
        }

        public static string TblSet(string tblName)
        {
            return string.Format(CC.FMT_TBL, CC.DBNAME, tblName);
        }

        public static string SetDdlValue<T>(T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var type = typeof(T);
            var stringBuilder = new StringBuilder();

            // プロパティ情報を取得し、プロパティ名と値を連結する
            PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(instance);
                if (value != null)
                {
                    stringBuilder.Append($"{property.Name}={value}|");
                }
            }

            return stringBuilder.ToString();
        }
        public static string GetDdlValue(string input, string key)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 正規表現パターンを作成
            // このパターンはキーに続く任意の文字列をキャプチャし、その後に '|' が来る前までの文字列をマッチさせます。
            string pattern = $@"{Regex.Escape(key)}=(.*?)(?=\||$)";

            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                // グループ 1 はキャプチャされた値
                return match.Groups[1].Value;
            }

            return string.Empty; // キーが見つからない場合は null を返す
        }

        public static string Right(string value, int len)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            if (value.Length < len) return value;

            return value.Substring(value.Length - len);
        }
        public static string Left(string value, int len)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            if (value.Length < len) return value;

            return value.Substring(0, len);
        }
        public static string PadLeftZero(int number, int totalWidth)
        {
            return number.ToString().PadLeft(totalWidth, '0');
        }
        public static string PadLeftZero(string? number, int totalWidth)
        {
            return string.IsNullOrWhiteSpace(number) ? string.Empty : number.PadLeft(totalWidth, '0');
        }

        public static void LogOutput(string cls ,string method ,string data)
        {

        }
    }
}
