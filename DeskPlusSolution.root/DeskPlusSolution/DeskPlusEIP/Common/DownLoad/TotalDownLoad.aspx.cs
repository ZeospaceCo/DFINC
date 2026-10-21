using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;


namespace Zeospace.DeskPlus.Common.Web.DownLoad
{
    public partial class TotalDownLoad : Base.BasePageCommon
    {
        #region Declaration (컨트롤 선언)
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Common.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Common.Web.DownLoad.TotalDownLoad).Assembly;	//어셈블리 선언
            #endregion

            SetLanguage();

            #region HEADER 정보 로드
            FileList = ((string)(Request.Params["fileList"] ?? "") == "") ? "" : (string)(Request.Params["fileList"] ?? "");

            string decryptString = "";

            if (FileList.Length > 0)
            {
                foreach (string fullpath in FileList.Split('|'))
                {
                    string filename = fullpath.Split(':')[0];
                    string filepath = DecryptString(fullpath.Split(':')[1]);


                    if (filepath.ToUpper().IndexOf("/BOARD/") > 0)  //filePath에 board 가 포함되어 있으면 게시판 첨부파일
                    {
                        int listid = filepath.Split('/')[6].ToNumber<int>();

                        Zeospace.DeskPlus.Board.DAL.ListEtc etc = new Zeospace.DeskPlus.Board.DAL.ListEtc();
                        List<Board.BE.ListEtcInfo> al = etc.GetListID(CurrentUserInfo.CompanyID, listid);

                        if (al.Count > 0)
                        {
                            foreach (Board.BE.ListEtcInfo info in al)
                            {
                                if (info.Etc8.ToString() == "1") //특정사용자들만 첨부파일이 가능하도록 체크가 되어있다면
                                {
                                    Application.Lock();
                                    Application.Remove("SysCode" + CurrentUserInfo.CompanyID);
                                    Application.UnLock();
                                    if (!CurrentUserInfo.IsInRole("AllowBoardDownUser") && !CheckIP()) //게시판 첨부다운 허용role이 없고, IP대역도 다르다면 다운불가 메시지 출력
                                    {
                                        Response.Write("<script type='text/javascript'>");
                                        Response.Write("window.alert('" + resMng.GetString("DownLoadArea") + "');self.opener = self;self.close();");
                                        Response.Write("<" + "/script>");
                                        Response.End();
                                    }
                                }
                            }
                        }
                    }

                    //filePath=/DeskPlusEIP/FileData/1/Board/201202/228/efefab0e-e59e-4347-b09c-79b4858bb72e.txt&fileName=게시판모바일sp추가부분.txt
                    filepath = UtilHelper.GetParmforParameter(filepath, "filePath", "");

                    decryptString += filename + ":" + filepath + "|";
                }
            }

            FileList = decryptString.TrimEnd('|');
            #endregion

            if (!IsPostBack)
            {
                #region 파라미터 처리 부분
                #endregion

                #region 리소스 처리
                this.GetResourceManager();
                if (!this.resMng.Equals(null))
                {
                    //CbtnClose.Text = resMng.GetString("키명");
                    CbtnSubmit.Text = resMng.GetString("Alert_Ok");
                    CbtnClose.Text = resMng.GetString("Close");
                }
                #endregion

                #region 이미지 처리
                #endregion

                #region 초기화
                #endregion
            }
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// IP체크
        /// </summary>
        /// <returns></returns>
        private bool CheckIP()
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.IPList allowIPList = new IPList();
            System.Data.DataView dv = AppSettingHelper.GetSysCode(CurrentUserInfo.CompanyID, "AllowBoardDownLoadIP");

            string ip = "";
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["ActiveCode"].ToString() == "1")
                {
                    ip = dv[i]["InterfaceCode"].ToString();
                    string[] ipArr = ip.Split('-');

                    if (ipArr.Length == 1)
                    {
                        allowIPList.Add(ipArr[0]);
                    }
                    else
                    {
                        allowIPList.AddRange(ipArr[0], ipArr[1]);
                    }
                }
            }

            if (allowIPList.CheckNumber(Request.UserHostAddress))
            {
                return true;      // 접속 IP가 사내 IP 대역에 있음. 
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 복고화
        /// </summary>
        /// <param name="_enc"></param>
        /// <returns></returns>
        private string DecryptString(string _enc)
        {
            string param = Server.UrlDecode(_enc);//디코딩
            param = Utility.DecryptString(param);
            param = Server.UrlDecode(param);//디코딩
            return param;
        }

        #endregion

        #region Event Handler (이벤트 핸들러)
         #endregion

        #region Property (프로퍼티)
        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-09-12
        /// 작성목적 : 파일 리스트
        /// </summary>
        public string FileList
        {
            get
            {
                return GetViewStateString("FileList", "");
            }
            set
            {
                ViewState["FileList"] = value;
            }
        }
        #endregion
    }
}
