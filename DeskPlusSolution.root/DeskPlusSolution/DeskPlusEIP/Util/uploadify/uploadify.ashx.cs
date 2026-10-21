using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zeospace.DeskPlus.Base.Web.Util.uploadify
{
    /// <summary>
    /// uploadify의 요약 설명입니다.
    /// </summary>
    public class uploadify : IHttpHandler
    {

        /// <summary>
        ///   <see cref="T:System.Web.IHttpHandler"/> 인터페이스를 구현하는 사용자 지정 HttpHandler를 사용하여 HTTP 웹 요청을 처리할 수 있도록 합니다.
        /// </summary>
        /// <param name="context">Request, Response, Session, Server 등과 같이 HTTP 요청을 처리하는 데 사용되는 내장 서버 개체에 대한 참조를 제공하는 <see cref="T:System.Web.HttpContext"/> 개체입니다.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Expires = -1;
            try
            {
                HttpPostedFile postedFile = context.Request.Files["Filedata"];

                string savepath = "";
                string tempPath = "";

                tempPath = context.Request["folder"];
                savepath = context.Server.MapPath(tempPath);

                string filename = postedFile.FileName;
                if (!System.IO.Directory.Exists(savepath))
                {
                    System.IO.Directory.CreateDirectory(savepath);
                }

                postedFile.SaveAs(savepath + @"\" + filename);
                context.Response.Write(tempPath + "/" + filename);

                /*
                200 : request가 성공적으로 완료되었음.
                204 : request가 실행되었으나 클라이언트에게 보낼 데이터가 없음.
                300 : 요구된 request가 여러 위치에 존재하는 자원을 필요로 하므로
                        request는 위에대한 정보를 보낸다. 클라이언트는 가장 적당한 
                        위치를 선택하여야 함.
                302 : request가 요구한 데이터를 발견하였으나 실제 다른 url에 존재함.
                400 : request의 문법이 잘못되었음.
                403 : request는 금지된 자원을 요구하였음.
                404 : 서버는 요구된 url을 찾을 수 없음.
                500 : 서버에 내부적으로 오류가 발생하여 더 이상을 진행할 수 없음.
                501 : 요청된 request는 합법적이나 서버는 요구된 method를 지원하지 않음.
                503 : 서버가 바쁘기 때문에 서비스를 할 수 없음.
                */

                context.Response.StatusCode = 200; //성공처리 (기본값 200)
            }
            catch (Exception ex)
            {
                context.Response.Write("Error: " + ex.Message);
            }
      
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}