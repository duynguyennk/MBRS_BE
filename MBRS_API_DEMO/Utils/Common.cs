using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using MBRS_API_DEMO.Models;
using XSystem.Security.Cryptography;
using System;

namespace MBRS_API_DEMO.Utils
{
    public static class Common
    {
        private static Random random = new Random();
        public static User GetUserByToken(string token)
        {
            User user = new User();
            if (string.IsNullOrEmpty(token)) return user;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            user.UserName = tokenS.Claims.First(claim => claim.Type == "UserName") != null ? tokenS.Claims.First(claim => claim.Type == "UserName").Value.Trim() : "";
            user.DepartmentCode = tokenS.Claims.First(claim => claim.Type == "DepartmentCode") != null ? tokenS.Claims.First(claim => claim.Type == "DepartmentCode").Value.Trim() : "";
            user.DepartmentName = tokenS.Claims.First(claim => claim.Type == "DepartmentName") != null ? tokenS.Claims.First(claim => claim.Type == "DepartmentName").Value.Trim() : "";
            user.Role = tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role) != null ? tokenS.Claims.First(claim => claim.Type == ClaimTypes.Role).Value.Trim() : "";
            user.Token = "Bearer " + token;
            return user;
        }
        public static DateTime ConvertUTCDateTime()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            return dateTime;
        }
        public static int RandomNumber()
        {
            Random rnd = new Random();
            int randomNumber = rnd.Next(1000, 100000);
            return randomNumber;
        }
            public static bool IsValidDate(string value, string dateFormats)
        {
            DateTime tempDate;
            bool validDate = DateTime.TryParseExact(value, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out tempDate);
            if (validDate)
                return true;
            else
                return false;
        }

        public static DateTime? StringToDate(object date, string format = "yyyy/MM/dd")
        {
            try
            {
                if (date == null) return null;
                DateTime dateResult;
                DateTime.TryParseExact((string)date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
                return dateResult;
            }
            catch
            {
                return null;
            }
        }

        public static string DateToString(DateTime? dateTime, string format = "yyyy/MM/dd")
        {
            try
            {
                return dateTime.Value.ToString(format);
            }
            catch
            {
                return null;
            }
        }

        public static bool ConvertToBool(this object obj)
        {
            if (obj == null) { return false; }
            return Convert.ToBoolean(obj);
        }

        public static string ConvertToString(this object obj, string defaultValue = "")
        {
            if (obj == null) { return defaultValue; }
            return obj.ToString();
        }

        public static object ConvertToDBValue(this object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }


        public static int ConvertToInt(this object _value, int defaultValue = 0)
        {
            int rt = 0;
            int.TryParse(_value.Convert_ToString(), out rt);
            return rt;
        }

        public static double ConvertToDouble(this object _value)
        {
            double rt = 0;
            double.TryParse(_value.Convert_ToString(), out rt);
            return rt;
        }

        public static decimal ConvertToDecimal(this object _value)
        {
            decimal rt = 0;
            decimal.TryParse(_value.Convert_ToString(), out rt);
            return rt;
        }

        public static float ConvertToFloat(this object _value, float defaultValue = 0)
        {
            float rt = 0;
            float.TryParse(_value.Convert_ToString(), out rt);
            return rt;
        }

        public static List<T> ToList<T>(this DataTable data) where T : new()
        {
            List<T> dtReturn = new List<T>();
            if (data == null)
                return dtReturn;

            Type typeParameterType = typeof(T);

            var props = typeParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetGetMethod() != null);

            foreach (DataRow item in data.AsEnumerable())
            {
                dtReturn.Add(GetValueT<T>(item, props));
            }
            return dtReturn;
        }

        public static List<T> ToListBasic<T>(this DataTable data)
        {
            List<T> dtReturn = new List<T>();
            if (data == null)
                return dtReturn;
            Type typeParameterType = typeof(T);
            foreach (DataRow item in data.AsEnumerable())
            {
                dtReturn.Add((T)Convert.ChangeType(item[0], typeParameterType));
            }
            return dtReturn;
        }


        public static IEnumerable<T> AsEnumerable<T>(this DataTable data) where T : new()
        {
            List<T> dtReturn = new List<T>();
            if (data == null)
                return dtReturn;

            Type typeParameterType = typeof(T);

            var props = typeParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetGetMethod() != null);

            foreach (DataRow item in data.AsEnumerable())
            {
                dtReturn.Add(GetValueT<T>(item, props));
            }
            return dtReturn;
        }


        private static T GetValueT<T>(DataRow row, IEnumerable<PropertyInfo> props) where T : new()
        {
            T objRow = new T();
            foreach (var field in props)
            {
                string fieldName = field.Name;
                var columnName = field.CustomAttributes.FirstOrDefault(x => x.AttributeType.UnderlyingSystemType.Name == "ColumnAttribute");
                if (columnName != null)
                    fieldName = columnName.ConstructorArguments[0].Value.ToString();
                if (row.Table.Columns.Contains(fieldName) && row[fieldName] != DBNull.Value)
                {
                    Type t = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
                    if (field.PropertyType.IsValueType)
                        field.SetValue(objRow, Convert.ChangeType(row[fieldName] == DBNull.Value ? Activator.CreateInstance(t) : row[fieldName], t));
                    else
                    {
                        field.SetValue(objRow, Convert.ChangeType(row[fieldName], t));
                    }
                }
            }
            return objRow;
        }

        public static string Convert_ToString(this object _value)
        {
            if (_value == null)
                return "";
            return _value.ToString();
        }

        public static int Convert_ToInt(this object _value)
        {
            int rt = 0;
            int.TryParse(_value.Convert_ToString(), out rt);
            return rt;
        }

        public static DateTime? ConvertToDateTime(this object obj)
        {
            if (obj == null) { return null; }
            DateTime dateResult;
            DateTime.TryParseExact(obj.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateResult);
            return dateResult;
        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null) return;
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and
            // populate them from their desination counterparts
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                var rs = CheckProperty(srcProp, typeDest);
                if (!rs) continue;
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyToList<T>(this IList<T> source, IList<T> destination) where T : new()
        {
            // If any this null throw an exception
            if (source == null || destination == null) return;
            // Getting the Types of the objects

            T newValue = new T();

            for (int i = 0; i < source.Count; i++)
            {
                source[i].CopyProperties(newValue);
                destination.Add(newValue);
            }
        }

        private static bool CheckProperty(PropertyInfo srcProp, Type typeDest)
        {
            PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
            if (targetProperty == null)
            {
                return false;
            }
            if (!targetProperty.CanWrite)
            {
                return false;
            }
            if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
            {
                return false;
            }
            if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
            {
                return false;
            }
            if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Thang.NguyenVan1
        /// </summary>
        /// <param name="date"></param>
        /// <returns>number of week in the month of date from input</returns>
        public static int GetWeekNumberOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public static string GetDescription<T>(this T enumValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        public class CustomDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), "yyyy/MM/dd HH:mm", null);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                //Don't implement this unless you're going to use the custom converter for serialization too
                throw new NotImplementedException();
            }
        }

        public class CustomDateTimeConverterWithSecond : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                return DateTime.ParseExact(reader.GetString(), "yyyy/MM/dd HH:mm:ss", null);
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                //Don't implement this unless you're going to use the custom converter for serialization too
                throw new NotImplementedException();
            }
        }
        public static string RandomPassword()
        {
            string randomPassword = "";
            try
            {

                string[] myIntArray = new string[10];
                int x;
                //that is to create the random # and add it to uour string
                Random autoRand = new Random();
                for (x = 0; x < 10; x++)
                {
                    myIntArray[x] = Convert.ToChar(Convert.ToInt32(autoRand.Next(65, 87))).ToString();
                    randomPassword += (myIntArray[x].ToString());
                }
            }
            catch (Exception ex)
            {
                randomPassword = "error";
            }
            return randomPassword;
        }
        public static string GetMD5Password(string password)
        {
            string str_md5 = "";
            byte[] mang = System.Text.Encoding.UTF8.GetBytes(password);

            MD5CryptoServiceProvider my_md5 = new MD5CryptoServiceProvider();
            mang = my_md5.ComputeHash(mang);

            foreach (byte b in mang)
            {
                str_md5 += b.ToString("X2");
            }

            return str_md5;
        }
        public static DataTable ToDataTable<T>(this IList<T> data, List<String> listColumn = null)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptorCollection props1 = new PropertyDescriptorCollection(null); ;
            DataTable table = new DataTable();
            List<int> arrInt = new List<int>();
            if (listColumn.Count > 0)
            {
                string[] Column = listColumn.ToArray();
                for (int i = props.Count - 1; i >= 0; i--)
                {
                    if (Column.Any(x => x.Equals(props[i].Name)))
                    {
                        props1.Add(props[i]);
                    }
                }
                props1 = props1.Sort(Column);
            }
            for (int i = 0; i < props1.Count; i++)
            {
                PropertyDescriptor prop = props1[i];
                var propertyType = prop.PropertyType;
                if (prop.PropertyType.Name.Contains("Nullable"))
                {
                    propertyType = typeof(float);
                }
                table.Columns.Add(prop.Name, propertyType);
            }
            object[] values = new object[props1.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props1[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
