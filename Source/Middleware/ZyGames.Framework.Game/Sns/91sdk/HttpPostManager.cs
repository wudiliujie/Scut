﻿using System.Net;
using System.IO;
using System.Text;

namespace ZyGames.Framework.Game.Sns._91sdk
{
    /// <summary>
    /// Http Get Post 工具类
    /// </summary>
    public static class HttpPostManager
    {
        private static int httpRequestTimeOut = 10000; // 10秒超时

        #region GetPostData Post数据并获取返回数据
        /// <summary>
        /// Post数据并获取返回数据，带Http返回状态码
        /// </summary>
        /// <param name="url">请求地址URL</param>
        /// <param name="postBytes">要发送的数据</param>
        /// <param name="statusCode">Http服务器返回的状态码</param>
        /// <returns>返回的二进制数据</returns>
        public static byte[] GetPostData(string url, byte[] postBytes, out HttpStatusCode statusCode)
        {
            statusCode = (HttpStatusCode)0;
            byte[] responseData = null;

            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                if (postBytes != null && postBytes.Length > 0)
                {
                    request.Method = "POST";
                    request.ContentType = "multipart/form-data";
                    request.ContentLength = postBytes.Length;
                    request.Timeout = httpRequestTimeOut;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                }
            }
            catch
            {
                statusCode = (HttpStatusCode)(-1);
            }

            if (statusCode != (HttpStatusCode)(-1) && request != null)
            {
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    statusCode = response.StatusCode;
                    using (Stream stream = response.GetResponseStream())
                    {
                        //接收数据
                        int responseDataLength = (int)response.ContentLength;
                        if (responseDataLength > 0)
                        {
                            responseData = new byte[responseDataLength];
                            int len = 102400; // 每次读取10K
                            int startIndex = 0;
                            int readSize = 0;
                            while (startIndex < responseDataLength)
                            {
                                if (startIndex + len < responseDataLength)
                                {
                                    readSize = len;
                                }
                                else
                                {
                                    readSize = (int)(responseDataLength - startIndex);
                                }
                                int intRead = stream.Read(responseData, startIndex, readSize);
                                startIndex += intRead;
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                        statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    else if (response != null)
                        statusCode = response.StatusCode;
                    else
                        statusCode = (HttpStatusCode)(-1);
                    responseData = null;
                }
                catch
                {
                    if (response != null)
                        statusCode = response.StatusCode;
                    else
                        statusCode = (HttpStatusCode)(-1);
                    responseData = null;
                }
            }

            return responseData;
        }
        #endregion

        #region GetStringData Http Get 获取返回字符串
        /// <summary>
        /// Http Get 获取返回字符串
        /// </summary>
        /// <param name="url">请求地址URL</param>
        /// <param name="statusCode">Http服务器返回的状态码</param>
        /// <returns>返回字符串</returns>
        public static string GetStringData(string url, out HttpStatusCode statusCode)
        {
            byte[] byteData = GetPostData(url, null, out statusCode);
            if (byteData != null)
                return Encoding.UTF8.GetString(byteData);
            else
                return null;
        }

        /// <summary>
        /// Http Get 获取返回字符串
        /// </summary>
        /// <param name="url">请求地址URL</param>
        /// <returns>返回字符串</returns>
        public static string GetStringData(string url)
        {
            HttpStatusCode statusCode = (HttpStatusCode)0;
            return GetStringData(url, out statusCode);
        }
        #endregion

    }

}
