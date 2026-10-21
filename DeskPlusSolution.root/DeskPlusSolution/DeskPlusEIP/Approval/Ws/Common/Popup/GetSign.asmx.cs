using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Common.Popup
{
    /// <summary>
    /// GetSign의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetSign : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-06-20
        /// 작성목적 : Signature 생성
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void Draw(string p)
        {
            try
            {
                IList<Point> points = new JavaScriptSerializer().Deserialize<IList<Point>>(p);

                Image signImage = (Image)Session["SignImage"];
                lock (signImage)
                {
                    using (Graphics g = Graphics.FromImage(signImage))
                    using (Pen pen = new Pen(Color.Black, 5))
                    {
                        if (points.Count > 1)
                        {
                            int startX = points[0].X;
                            int startY = points[0].Y;

                            for (int i = 1; i < points.Count; i++)
                            {
                                g.DrawLine(pen, startX, startY, points[i].X, points[i].Y);
                                startX = points[i].X;
                                startY = points[i].Y;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Draw", ex);
            }
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-06-20
        /// 작성목적 : Signature 다시하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public void Clear()
        {
            Bitmap bmp = new Bitmap(200, 200);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bmp.Width, bmp.Height));
                g.Flush();
            }

            Session["SignImage"] = bmp;
        }

        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-06-20
        /// 작성목적 : Signature 이미지 저장
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string Save()
        {
            try
            {
                System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)Session["SignImage"];

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string signImagePath = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                string signPath = baseFileDirectory + "{0}/Users/{1}/Picture";
                string signFileName = "Sign_" + Guid.NewGuid().ToString() + ".png";
                signImagePath = string.Format(signImagePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, signFileName);
                signPath = string.Format(signPath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

                if (!Directory.Exists(FileHelper.GetServerMapPath(signPath)))
                {
                    Directory.CreateDirectory(FileHelper.GetServerMapPath(signPath));
                }

                bmp.Save(Server.MapPath(signImagePath), ImageFormat.Png);

                Zeospace.DeskPlus.Account.DAL.User dal = new Zeospace.DeskPlus.Account.DAL.User();

                if (dal.ModSeal(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, signFileName))
                {
                    return signImagePath;
                }
                else
                {
                    return "!NULL";
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Save", ex);
                return "!NULL";
            }
        }
    }

    public class Point
    {
        public int X;
        public int Y;
    };
}
