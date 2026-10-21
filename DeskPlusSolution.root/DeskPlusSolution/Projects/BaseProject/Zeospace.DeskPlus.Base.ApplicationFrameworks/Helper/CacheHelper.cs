using System;
using System.Data;
using System.IO;
using System.Web;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// Cache 관리 Class
    /// DataBase에서 DataSet 형식으로 데이터를 가져와서 서버의 DeskPlusEIP/Data/Cache 폴더에 저장하여 사용
    /// DataBase 컨넥션 사용을 줄여서 성능 향상에 사용
    /// </summary>
    public abstract class CACHEHelper
    {
        #region 변수 선언

        private static string cacheFileLocation = "";
        private const string CACHEPATH = "/DeskPlusEIP/Data/Cache/";

        #endregion 변수 선언

        #region Method

        /// <summary>
        /// M : Cache 데이터 가져오기
        /// </summary>
        /// <param name="_cacheName">캐쉬명</param>
        /// <returns>DataSet</returns>
        public static DataSet GetCacheData(string _cacheName)
        {
            DataSet ds = null;

            XmlTextReader xtr = null;
            try
            {
                //서버의 물리적 파일 위치 만들기
                HttpContext context = HttpContext.Current;
                cacheFileLocation = context.Server.MapPath(CACHEPATH + _cacheName + ".xml");

                if (File.Exists(cacheFileLocation))
                {
                    xtr = new XmlTextReader(cacheFileLocation);

                    DataSet tmp = (DataSet)context.Cache[_cacheName];

                    if (tmp != null)
                    {
                        ds = tmp.Clone();
                        ds.ReadXml(xtr, XmlReadMode.DiffGram);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.AppException.WriteException("CacheHelper.cs[GetCacheData] : " + ex.Message);
                DelCacheData(_cacheName);
                throw;
            }
            finally
            {
                if (xtr != null)
                {
                    xtr.Close();
                }
            }

            return ds;
        }

        /// <summary>
        /// M : Cache 데이터 넣기
        /// </summary>
        /// <param name="_ds">DataSet</param>
        /// <param name="_cacheName">캐쉬명</param>
        public static void SetCacheData(DataSet _ds, string _cacheName)
        {
            XmlTextWriter xtw = null;
            try
            {
                //서버의 물리적 파일 위치 만들기
                HttpContext context = HttpContext.Current;
                cacheFileLocation = context.Server.MapPath(CACHEPATH + _cacheName + ".xml");

                //context 객채에 캐쉬 생성
                context.Cache.Insert(_cacheName, _ds);

                //임시 파일 생성
                xtw = new XmlTextWriter(cacheFileLocation, null);
                _ds.WriteXml(xtw, XmlWriteMode.DiffGram);
            }
            catch (Exception ex)
            {
                Exceptions.AppException.WriteException("CacheHelper.cs[SetCacheData] : " + ex.Message);
            }
            finally
            {
                if (xtw != null)
                {
                    xtw.Close();
                }
            }
        }

        /// <summary>
        /// M : Cache 지우기
        /// </summary>
        /// <param name="_cacheName">캐쉬명</param>
        public static void DelCacheData(string _cacheName)
        {
            try
            {
                //서버의 물리적 파일 위치 만들기
                HttpContext context = HttpContext.Current;
                cacheFileLocation = context.Server.MapPath(CACHEPATH + _cacheName + ".xml");

                //캐쉬 삭제
                context.Cache.Remove(_cacheName);

                //임시 파일 삭제
                File.Delete(cacheFileLocation);
            }
            catch (Exception ex)
            {
                Exceptions.AppException.WriteException("CacheHelper.cs[DelCacheData] : " + ex.Message);
            }
        }

        #endregion Method
    }
}