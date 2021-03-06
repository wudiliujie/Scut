﻿using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using ZyGames.Framework.Game.Lang;

namespace ZyGames.Framework.Game.Service
{

    /// <summary>
    /// HttpGet 的摘要说明
    /// </summary>
    public class HttpGet
    {
        class myCultureComparer : IEqualityComparer
        {
            public CaseInsensitiveComparer myComparer;

            public myCultureComparer()
            {
                myComparer = CaseInsensitiveComparer.DefaultInvariant;
            }

            public new bool Equals(object x, object y)
            {
                if (myComparer.Compare(x, y) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public int GetHashCode(object obj)
            {
                // Compare the hash code for the lowercase versions of the strings.
                return obj.ToString().ToLower().GetHashCode();
            }
        }


        private Hashtable hashtable = new Hashtable(3, (float).8, new myCultureComparer());
        private string _requestParam = string.Empty;
        private StringBuilder _error = new StringBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpGet(HttpRequest request)
        {
            _remoteAddress = request.UserHostAddress;
            if (request["d"] != null)
            {
                _paramString = request["d"];
                InitData(_paramString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">自定义参数字串</param>
        /// <param name="remoteAddress"></param>
        public HttpGet(string param, string remoteAddress)
        {
            _paramString = param;
            _remoteAddress = remoteAddress;
            InitData(_paramString);
        }

        private string _remoteAddress;

        /// <summary>
        /// 远端地址
        /// </summary>
        public string RemoteAddress
        {
            get { return _remoteAddress; }
        }

        private string _paramString;

        /// <summary>
        /// 参数源字串
        /// </summary>
        public string ParamString
        {
            get { return _paramString; }
        }

        private void InitData(string d)
        {
            int index = d.LastIndexOf("&sign=");
            if (index != -1)
            {
                _requestParam = d.Substring(0, index);
            }
            string[] splitresult = d.Split(new char[] { '&' });
            foreach (string result in splitresult)
            {
                string[] equalsplitresult = result.Split(new char[] { '=' });
                if (equalsplitresult.Length == 2)
                {
                    string key = equalsplitresult[0];
                    string value = HttpUtility.UrlDecode(equalsplitresult[1], Encoding.UTF8);
                    if (hashtable.ContainsKey(key))
                    {
                        hashtable[key] = value;
                    }
                    else
                    {
                        hashtable.Add(key, value);
                    }
                }
            }
        }
        private const int ZeroNum = 0;

        /// <summary>
        /// 是否有出错
        /// </summary>
        public bool HasError
        {
            get { return _error.Length > 0; }
        }
        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _error.ToString(); }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetIntValue(string param)
        {
            return GetIntValue(param, ZeroNum, int.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public int GetIntValue(string param, int min, int max, bool isRequired = true)
        {
            int value = 0;
            if (!GetInt(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public short GetWordValue(string param)
        {
            return GetWordValue(param, ZeroNum, short.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public short GetWordValue(string param, short min, short max, bool isRequired = true)
        {
            short value = 0;
            if (!GetWord(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public byte GetByteValue(string param)
        {
            return GetByteValue(param, ZeroNum, byte.MaxValue, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public byte GetByteValue(string param, byte min, byte max, bool isRequired = true)
        {
            byte value = 0;
            if (!GetByte(param, ref value, min, max) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetStringValue(string param)
        {
            return GetStringValue(param, ZeroNum, -1, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="isRequired"></param>
        /// <returns></returns>
        public string GetStringValue(string param, int min, int max, bool isRequired = true)
        {
            string value = "";
            if (!GetString(param, ref value, min, max, true) && isRequired)
            {
                throw new ArgumentOutOfRangeException("param", string.Format("{0} value length out of range[{1}-{2}]", param, min, max));
            }
            return value;
        }


        /// <summary>
        /// 读取INT类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public int GetInt(string param)
        {
            return GetIntValue(param);
        }

        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public short GetWord(string param)
        {
            return GetWordValue(param);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Byte GetByte(string param)
        {
            return GetByteValue(param);
        }
        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetString(string param)
        {
            return GetStringValue(param);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public T GetEnum<T>(string param) where T : struct
        {
            T value = default(T);
            GetEnum(param, ref value);
            return value;
        }

        /// <summary>
        /// 检查是否包括指定参数
        /// </summary>
        /// <param name="param">参数名</param>
        /// <returns></returns>
        public bool Contains(string param)
        {
            if (hashtable.ContainsKey(param))
            {
                return true;
            }
            WriteContainsError(param);
            return false;
        }

        /// <summary>
        /// 读取INT类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public bool GetInt(string aName, ref Int32 rValue)
        {
            return GetInt(aName, ref rValue, ZeroNum, Int32.MaxValue);
        }
        /// <summary>
        /// 读取INT类型的请求参数,验证值的取值范围
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public bool GetInt(string aName, ref Int32 rValue, Int32 minValue, Int32 maxValue)
        {
            bool result = false;
            if (hashtable.ContainsKey(aName))
            {
                result = Int32.TryParse(hashtable[aName].ToString(), out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public bool GetWord(string aName, ref Int16 rValue)
        {
            return GetWord(aName, ref rValue, ZeroNum, Int16.MaxValue);
        }
        /// <summary>
        /// 读取short类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public bool GetWord(string aName, ref Int16 rValue, Int16 minValue, Int16 maxValue)
        {
            bool result = false;
            if (hashtable.ContainsKey(aName))
            {
                result = Int16.TryParse(hashtable[aName].ToString(), out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取Byte类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <returns></returns>
        public bool GetByte(string aName, ref Byte rValue)
        {
            return GetByte(aName, ref rValue, ZeroNum, Byte.MaxValue);
        }
        /// <summary>
        /// 读取Byte类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">取值最小范围</param>
        /// <param name="maxValue">取值最大范围</param>
        /// <returns></returns>
        public bool GetByte(string aName, ref Byte rValue, Byte minValue, Byte maxValue)
        {
            bool result = false;
            if (hashtable.ContainsKey(aName))
            {
                result = Byte.TryParse(hashtable[aName].ToString(), out rValue);
                if (result)
                {
                    result = rValue >= minValue && rValue <= maxValue;
                }
                if (!result)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="aName">URL参数名</param>
        /// <param name="rValue">返回变量</param>
        /// <param name="ignoreError"></param>
        /// <returns></returns>
        public bool GetString(string aName, ref String rValue, bool ignoreError = false)
        {
            return GetString(aName, ref rValue, ZeroNum, -1, ignoreError);
        }

        /// <summary>
        /// 读取String类型的请求参数
        /// </summary>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <param name="minValue">长度最小范围</param>
        /// <param name="maxValue">长度最大范围，-1:不限制</param>
        /// <param name="ignoreError"></param>
        /// <returns></returns>
        public bool GetString(string aName, ref String rValue, int minValue, int maxValue, bool ignoreError = false)
        {
            bool result = false;
            if (hashtable.ContainsKey(aName))
            {
                rValue = hashtable[aName].ToString().Trim();
                result = rValue.Length >= minValue && (maxValue < 0 || rValue.Length <= maxValue);
                if (!result && !ignoreError)
                {
                    WriteRangOutError(aName, minValue, maxValue);
                }
            }
            else if (!ignoreError)
            {
                WriteContainsError(aName);
            }
            return result;
        }

        /// <summary>
        /// 读取枚举类型的请求参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="aName"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public bool GetEnum<T>(string aName, ref T rValue) where T : struct
        {
            bool result = false;
            if (hashtable.ContainsKey(aName))
            {
                result = true;
                try
                {
                    rValue = (T)Enum.Parse(typeof(T), hashtable[aName].ToString().Trim());
                }
                catch
                {
                    result = false;
                }
            }
            else
            {
                WriteContainsError(aName);
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 签名检查
        /// </summary>
        /// <returns></returns>
        public bool CheckSign()
        {
            string sign = "";
            if (GetString("sign", ref sign))
            {
                if (_requestParam != null)
                {
                    string attachParam = _requestParam + "44CAC8ED53714BF18D60C5C7B6296000";
                    string key = FormsAuthentication.HashPasswordForStoringInConfigFile(attachParam, "MD5");
                    if (!string.IsNullOrEmpty(key) && key.ToLower() == sign)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void WriteContainsError(string param)
        {
            if (_error.Length > 0)
            {
                _error.Append(",");
            }
            _error.AppendFormat(LanguageHelper.GetLang().UrlNoParam, param);
        }

        private void WriteRangOutError(string param, int min, int max)
        {
            if (_error.Length > 0)
            {
                _error.Append(",");
            }
            _error.AppendFormat(LanguageHelper.GetLang().UrlParamOutRange, param, min, max);
        }

    }
}