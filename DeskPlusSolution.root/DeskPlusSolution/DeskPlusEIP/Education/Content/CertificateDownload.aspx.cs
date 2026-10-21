using System;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Zeospace.DeskPlus.Education.Web.Content
{
    public class CertificateDownload : Base.BasePageEducation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 관리자 목록에서는 대상 사용자 ID를 전달하고, 내 수료증은 로그인한 사용자로 조회합니다.
            int userID;
            if (!Int32.TryParse(GetDecryptParam("userID", CurrentUserInfo.UserID.ToString()), out userID) || userID <= 0)
            {
                NotFound();
                return;
            }
            string fileGUID = GetDecryptParam("fileGUID", "");
            BE.TOAttachInfo certificate = new DAL.TOAttach().Get(CurrentUserInfo.CompanyID, ListID, userID, fileGUID);
            if (certificate == null)
            {
                NotFound();
                return;
            }
            string filePath = Server.MapPath(certificate.FilePath + certificate.FileGUID);
            if (!File.Exists(filePath))
            {
                NotFound();
                return;
            }
            string downloadName = Path.GetFileName(certificate.FileName);
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.AddHeader("X-Content-Type-Options", "nosniff");
            string encodedName = Uri.EscapeDataString(downloadName);
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + encodedName + "\"; filename*=UTF-8''" + encodedName);
            Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString());
            Response.TransmitFile(filePath);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            Response.ContentType = "text/plain; charset=utf-8";
            Response.Write("수료증을 찾을 수 없습니다.");
            Context.ApplicationInstance.CompleteRequest();
        }

        // 다운로드 응답 뒤에 ASPX의 공백이나 HTML을 덧붙이지 않습니다.
        protected override void Render(HtmlTextWriter writer)
        {
        }
    }
}
