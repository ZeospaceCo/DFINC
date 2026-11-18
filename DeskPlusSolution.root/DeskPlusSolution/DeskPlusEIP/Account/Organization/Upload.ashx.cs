using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions;

namespace Zeospace.DeskPlus.Account.Web.Organization
{
    public class Upload : Base.BaseWSAccount, IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.TrySkipIisCustomErrors = true;
            try
            {
                if (!String.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                {
                    response.StatusCode = 405;
                    response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = "엑셀 파일은 업로드 버튼으로 등록해 주세요." }));
                    return;
                }
                OrganizationSheetLayout.DemandEdit(CurrentUserInfo, context, context.Request.Form["token"]);
                int revision;
                if (!Int32.TryParse(context.Request.Form["revision"], out revision) || revision < 0) throw new ArgumentException("조직도 버전을 확인할 수 없습니다. 새로고침 후 다시 업로드해 주세요.");
                var file = context.Request.Files["workbook"];
                if (file == null || file.ContentLength <= 0) throw new ArgumentException("업로드할 엑셀 파일을 선택해 주세요.");
                if (file.ContentLength > OrganizationSheetExcelImporter.MaxUploadBytes) throw new ArgumentException("엑셀 파일은 10MB 이내로 업로드해 주세요.");
                var document = OrganizationSheetExcelImporter.Import(file.InputStream, Path.GetFileName(file.FileName));
                string json = OrganizationSheetLayout.Serializer().Serialize(document);
                var record = new DAL.OrganizationSheet().Save(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, revision, json);
                response.Write(OrganizationSheetLayout.SafeJson(new
                {
                    ok = true,
                    state = new OrganizationSheetState { document = document, revision = record.Revision, storageReady = true, isTemplate = false,
                        modifiedAt = DateTime.SpecifyKind(record.ModifyDate, DateTimeKind.Utc).ToString("o") }
                }));
            }
            catch (HttpException ex)
            {
                response.StatusCode = ex.GetHttpCode();
                response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = ex.Message }));
            }
            catch (DBConcurrencyException)
            {
                response.StatusCode = 409;
                response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = "다른 관리자가 먼저 조직도를 갱신했습니다. 새로고침 후 다시 업로드해 주세요." }));
            }
            catch (ArgumentException ex)
            {
                response.StatusCode = 400;
                response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = ex.Message }));
            }
            catch (SqlException ex)
            {
                AppException.WriteException("OrganizationSheet.Upload : " + ex);
                response.StatusCode = 503;
                response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = ex.Number == 208 ? "조직도 저장소가 설치되지 않았습니다. 서버 설치를 완료한 후 업로드해 주세요." : "조직도를 저장하지 못했습니다. 기존 조직도는 유지됩니다. 잠시 후 다시 시도해 주세요." }));
            }
            catch (Exception ex)
            {
                AppException.WriteException("OrganizationSheet.Upload : " + ex);
                response.StatusCode = 500;
                response.Write(OrganizationSheetLayout.SafeJson(new { ok = false, message = "엑셀을 처리하지 못했습니다. 기존 조직도는 유지됩니다. 파일을 확인한 후 다시 업로드해 주세요." }));
            }
        }
    }
}
