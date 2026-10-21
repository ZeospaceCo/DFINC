using System;
using System.IO;
using System.Web;

namespace Zeospace.DeskPlus.Base.Web.Data.Editor.XFE2.upload
{
    public partial class upload : Base.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strUploadedPath, strContentType, strAbsolutePath, strExtension, strFileName;
            string strDirOriPath = (Session["XFEUpdatePath"] != null) ? Session["XFEUpdatePath"].ToString() : "/DeskPlusEIP/Data/Editor/XFE2/upload";
            string strDir = "";
            int index;
            bool validation;
            HttpPostedFile file;
            string strDialogType = "";

            strContentType = Request.Form["content_type"];        //이전 페이지에서 image인지 flash인지 받아옴
            strDialogType = Request.Form["dialog_type"];

            validation = false;

            if (strDirOriPath.Length > 0) strDir = Server.MapPath(strDirOriPath);

            if (!Directory.Exists(strDir))//이 경로가 있는지 확인하고
            {
                Directory.CreateDirectory(strDir);//없으면 해당 경로를 만든다
            }

            file = Request.Files["FILE_PATH"];//input type="file" mutlipart~형식으로 넘어온 파일정보를 받아옴
            strFileName = file.FileName;
            /*
             *파일이름은 asdf.gif 무조건 요 형식이므로 . 으로 split하면 .으로 쪼개진 배열이 나온다 헌데
             *이 배열의 마지막값이 확장자 이므로
             *쪼갠배열의 길이 - 1 을 배열의 인덱스로 주어(이렇게 하면 배열의 마지막 값이니깐)
             *파일의 확장자 명을 구한다
             */
            strExtension = strFileName.Split('.')[strFileName.Split('.').Length - 1];

            //유효성검사시작
            //ContentType이 플래쉬이면서 확장자가 swf인 파일 이거나 image이면서 확장자가 jpg || jpeg || gif || png 일경우
            if ((string.Compare(strContentType, "flash") == 0) || (string.Compare(strContentType, "image") == 0))
            {
                if ((string.Compare(strContentType, "flash") == 0))//flash 이면서
                {
                    if ((string.Compare(strExtension, "swf", true) == 0))//swf라면
                    {
                        validation = true;
                    }
                }
                if ((string.Compare(strContentType, "image") == 0))//image 이면서
                {
                    if ((string.Compare(strExtension, "jpg", true) == 0))//jpg라면
                    {
                        validation = true;
                    }
                    if ((string.Compare(strExtension, "jpeg", true) == 0))//jpeg라면
                    {
                        validation = true;
                    }
                    if ((string.Compare(strExtension, "gif", true) == 0))//gif라면
                    {
                        validation = true;
                    }
                    if ((string.Compare(strExtension, "png", true) == 0))//png라면
                    {
                        validation = true;
                    }
                }
            }
            if (validation == true)
            {
                if ((string.Compare(strContentType, "flash") == 0))
                    strFileName = "FLA_" + DateTime.Now.ToString("HHmmss");
                else
                    strFileName = "IMG_" + DateTime.Now.ToString("HHmmss");

                strUploadedPath = @"\" + strFileName;   //상대경로에 오늘의 날짜\파일이름.확장자로 저장
                strAbsolutePath = strDir;
                strDir = strAbsolutePath + strUploadedPath + "." + strExtension;  //절대경로에 상대경로를 이어붙여 저장

                //동일파일(같은시각에 파일을 올릴경우 동일이름의 파일이 생기게됨)을 막기위해 파일이름끝에_index를 붙여주는 작업
                index = 1;
                while (File.Exists(strDir))
                {
                    strFileName = DateTime.Now.ToString("HHmmss") + "_" + index;
                    strDir = strAbsolutePath + @"\" + strFileName + "." + strExtension;
                    index++;
                }
                file.SaveAs(strDir);//경로/파일명 으로 저장
                //html에 패스를 넘겨주기 위한 작업
                strFileName = strFileName + "." + strExtension; //[파일명.확장자]

                if (BaseHttpPort == "80" || BaseHttpPort == "")
                    strUploadedPath = BaseHttp + FullDomain + strDirOriPath + "/" + strFileName;
                else strUploadedPath = BaseHttp + FullDomain + ":" + BaseHttpPort + strDirOriPath + "/" + strFileName; //년월일/[파일명.확장자] 폴더 경로

                //html에 값을 넘겨줌
                divContentType.Value = strContentType;
                divImagePath.Value = strUploadedPath;
                dialogType.Value = strDialogType;
            }
            else
            {
                //유효성검사 에러메세지 출력
            }
        }
    }
}