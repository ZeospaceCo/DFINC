using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.Web.Package.SMS.Common
{
    public partial class GetAddressTree : Base.BasePagePortlet
    {
        #region Declaration (컨트롤 선언)
        private int CompanyID;
        private int UserID;
        private string PID;        
        private string UseActiveDept;        
        private string UseCheckBoxGroup;
        private string UseCheckBoxMember;
        private string UseIcon;

        #endregion


        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, System.EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.SMS.Common.GetAddressTree).Assembly;	//어셈블리 선언
            #endregion

            #region 바인딩
            
            LoadParam();

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            if (PID != "0")
            {
                try
                {                    
                    CompanyID = CurrentUserInfo.CompanyID;	//회사고유값
                    UserID = CurrentUserInfo.UserID;		//사용자아이디                    

                    DataSet ds = new DataSet();

                    Zeospace.DeskPlus.Address.DAL.Private bll = new Address.DAL.Private();
                    ds = bll.GetTree(CompanyID, PID.ToString(), UserID, PreLang);

                    if (ds.Tables["GetAddressTree"] != null)
                    {
                        foreach (DataRow dbRow in ds.Tables["GetAddressTree"].Rows)
                        {
                            if (UseActiveDept == "1")//사용자 및 부서의 활성화
                            {
                                if (dbRow["ActiveDept"].ToString() == "1")
                                {
                                    ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                                    TreeView1.Nodes.Add(node);
                                }
                            }
                            else
                            {
                                ComponentArt.Web.UI.TreeViewNode node = CreateNode(dbRow, false);
                                TreeView1.Nodes.Add(node);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("lbtnWrite_Click", ex);
                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                    throw;
                }

                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/xml";
                Response.Write(TreeView1.GetXml());
                Response.End();
            }

            #endregion
        }
        #endregion

        #region Method (함수)

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            PID = GetDecryptParam("PID", "");
            UseActiveDept = GetDecryptParam("UseActiveDept", "");            
            UseCheckBoxGroup = GetDecryptParam("UseCheckBoxGroup", "");
            UseCheckBoxMember = GetDecryptParam("UseCheckBoxMember", "");
            UseIcon = GetDecryptParam("UseIcon", "");
        }

        /// <summary>
        /// 메서드명   : 
        /// 작성목적   : 이름 이외의 별도 추가 이름들(직급,부서,회사명)
        /// 작성자     : 심재운
        /// 최초작성일 : 2008-06-12
        /// </summary>
        /// <param name="_dr">The _DR.</param>
        /// <returns></returns>
        private string NodeName(DataRow _dr)
        {
            
            string type = _dr["Type"].ToString();
            string baseDept = _dr["BaseDept"].ToString();            
            string etcName = "";            
            string dsText = "";
            string baseDeptText = GetResource("Both");   // (겸)

            string name = this.GetLangName(
                _dr["NameBase"].ToString(),
                _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang);

            string titleName = this.GetLangName(
                _dr["TitleBase"].ToString(),
                _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
               _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang);

            #region 다국어 회사 설정
            string comName = this.GetLangName(
                _dr["CompanyBase"].ToString(),
                _dr["CompanyKo"].ToString(), _dr["CompanyEn"].ToString(), _dr["CompanyJa"].ToString(),
                _dr["CompanyFr"].ToString(), _dr["CompanyZhCn"].ToString(), _dr["CompanyZhTw"].ToString(), PreLang);
            #endregion 

            if (titleName.Length > 0 || comName.Length > 0)
            {
                if (titleName.Length > 0 && comName.Length > 0)
                {
                    etcName = "(" + titleName + "/" + comName + ")";
                }
                else
                {
                    etcName = (titleName.Length > 0) ? "(" + titleName + ")" : "";
                    etcName = (comName.Length > 0) ? "(" + comName + ")" : etcName;
                }
            }

            //부서일 경우 / 사람일 경우 겸직 표시함
            if (baseDept == "0" && type.IndexOf("M") != -1)
            {
                dsText = name + baseDeptText + etcName;	//목록명
            }
            else
            {
                dsText = (type.IndexOf("M") != -1) ? name + etcName : name;
            }

            return dsText;
        }

        /// <summary>
        /// 하위트리 만들기
        /// </summary>
        /// <param name="_dr"></param>
        /// <param name="_expanded"></param>
        /// <returns></returns>
        private ComponentArt.Web.UI.TreeViewNode CreateNode(DataRow _dr, bool _expanded)
        {
            try
            {                
                ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();
                node.Text = NodeName(_dr).ToReverseXSS();

                //node.Text	= _dr["Name"].ToString();				//목록명
                node.ID = _dr["OwnerID"].ToString();			//목록 고유값
                node.Expanded = _expanded;							//확장여부
                

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("UseActiveDept", UseActiveDept);
                paraDic.Add("UseIcon", UseIcon);
                paraDic.Add("UseCheckBoxMember", UseCheckBoxMember);
                paraDic.Add("UseCheckBoxGroup", UseCheckBoxGroup);                    

                // 그룹일경우
                if (_dr["Type"].ToString().IndexOf("M") == -1) 
                {
                    node.ShowCheckBox = false;

                    if (UseCheckBoxGroup == "1")
                    {
                        // 그룹에 대한 체크박스가 허용이 되어도 직급/직책에 대해서는 체크박스를 막는다. 
                        node.ShowCheckBox = (_dr["OwnerID"].ToString() == "TG1" || _dr["OwnerID"].ToString() == "TG2") ? false : true;
                    }
                }
                else
                {
                    node.ShowCheckBox = (UseCheckBoxMember == "1") ? true : false;
                }

                if (UseIcon != "" && UseIcon == "1")
                {
                    if (_dr["ETC"].ToString().IndexOf("FG") != -1)
                    {
                        node.ImageUrl = BaseImgPath + "/Component/tree/favorites.gif";
                    }
                    else
                    {
                        node.ImageUrl = (_dr["Type"].ToString().IndexOf("M") == -1) ? BaseImgPath + "/Component/tree/folder.gif" : BaseImgPath + "/Component/tree/Member.gif";
                    }
                }
                else
                {
                    if (_dr["Type"].ToString().IndexOf("G") != -1)
                    {
                        node.Text = node.Text;
                        node.Style.Add("font-style", "bold");
                    }
                }
               
                //부서명의 노드 일경우
                if (_dr["Type"].ToString().IndexOf("M") == -1)
                {
                    int childCnt = _dr["childCnt"].ToString().ToNumber<int>();
                    if (childCnt > 0)
                    {
                        string pId = (_dr["Type"].ToString() == "R") ? _dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), "") : _dr["OwnerID"].ToString();

                        paraDic.Add("PID", pId);                        
                        string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));

                        node.ContentCallbackUrl = "/DeskPlusEIP/Portlet/Package/SMS/Common/GetAddressTree.aspx?guid=" + System.Guid.NewGuid().ToString() + "&p=" + encParams;
                    }
                }

                node.Attributes["CategoryID"] = _dr["CategoryID"].ToString();
                node.Attributes["ParentOwnerID"] = _dr["ParentOwnerID"].ToString().Replace(_dr["Type"].ToString(), "");//하위메뉴값
                node.Attributes["Orders"] = _dr["Orders"].ToString();
                node.Attributes["Type"] = _dr["Type"].ToString();
                node.Attributes["Etc"] = _dr["Etc"].ToString();

                return node;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("CreateNode", ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.ShowMessageBox(Page, "fail", GetResource("ProcessingError"), "");
                throw;
            }

        }



        #endregion


        #region Property (프로퍼티)


        

        #endregion


        #region Event Handler (이벤트 핸들러)
        #endregion
    }
}