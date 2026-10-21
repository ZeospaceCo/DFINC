using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.Web.Common
{
    public partial class GetDeptUserScript : Base.BasePageAccount
    {
        private int CompanyID;
        private int DeptID;
        private string Access = string.Empty;
        private string UseCheckBoxGroup = string.Empty;
        private string UseCheckBoxMember = string.Empty;
        private string UseIcon = string.Empty;
        private string UseActiveDept = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드

            this.ResourceName = "Zeospace.DeskPlus.Account.Web.Resource.GlobalString.string";					//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Account.Web.Common.GetDeptUserScript).Assembly;						//어셈블리 선언
            this.GetResourceManager();

            #endregion 다국어 리소스 로드

            ComponentArt.Web.UI.TreeView TreeView1 = new ComponentArt.Web.UI.TreeView();

            CompanyID = this.GetDecryptParam("cID", "0").ToNumber<int>();
            DeptID = this.GetDecryptParam("pID", "0").ToNumber<int>();
            Access = GetDecryptParam("access", "");
            UseCheckBoxGroup = GetDecryptParam("useCheckBoxGroup", "");
            UseCheckBoxMember = GetDecryptParam("useCheckBoxMember", "");
            UseIcon = GetDecryptParam("useIcon", "");
            UseActiveDept = GetDecryptParam("useActiveDept", "");

            if (CompanyID > 0 && DeptID > 0)
            {
                DataSet ds = new DataSet();

                if (Access == "3") //가상조직도
                {
                    EIP.DAL.UserRole role = new EIP.DAL.UserRole();
                    ds = role.GetUserRoleTree(CompanyID, DeptID);
                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(CompanyID, dbRow, false);
                        if (node != null) TreeView1.Nodes.Add(node);
                    }
                }
                else //일반조직도
                {
                    bool useActiveDept = (UseActiveDept == "1") ? true : false;
                    Zeospace.DeskPlus.Account.DAL.UserDept userDept = new Zeospace.DeskPlus.Account.DAL.UserDept();
                    ds = userDept.GetDeptUserTree(CompanyID, DeptID, useActiveDept);

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode node = CreateNode(CompanyID, dbRow, false);
                        if (node != null) TreeView1.Nodes.Add(node);
                    }
                }

                Response.Write(TreeView1.GetXml());
            }
        }

        private ComponentArt.Web.UI.TreeViewNode CreateNode(int _companyID, DataRow _dr, bool _expanded)
        {
            //2013-03-21 : deskplus 계정은 조직도에서 보이지 않음 ( shimpark)
            if (_dr["Type"].ToString().Equals("M") && _dr["OwnerID"].ToString() == "1") return null;

            ComponentArt.Web.UI.TreeViewNode node = new ComponentArt.Web.UI.TreeViewNode();

            string baseDept = "(" + this.GetResource("Module_DeptUserTreeSearch_AdditionalJob") + ")"; //겸

            #region 다국어 이름 설정

            string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 이름 설정

            #region 다국어 직급 설정

            string titleName = this.GetLangName(_dr["TitleBase"].ToString(), _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 직급 설정

            #region 다국어 직책 설정

            string dutyName = this.GetLangName(_dr["DutyBase"].ToString(), _dr["DutyKo"].ToString(), _dr["DutyEn"].ToString(), _dr["DutyJa"].ToString(),
                _dr["DutyFr"].ToString(), _dr["DutyZhCn"].ToString(), _dr["DutyZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 직책 설정

            #region 다국어 부서 설정

            string deptName = this.GetLangName(_dr["DeptBase"].ToString(), _dr["DeptKo"].ToString(), _dr["DeptEn"].ToString(), _dr["DeptJa"].ToString(),
                _dr["DeptFr"].ToString(), _dr["DeptZhCn"].ToString(), _dr["DeptZhTw"].ToString(), PreLang).ToReverseXSS();

            #endregion 다국어 부서 설정

            string titleNames = "";

            if (dutyName.Trim() == "-" || dutyName == "")
            {
                titleNames = (titleName.Length > 0) ? "(" + titleName + ")" : "";
            }
            else
            {
                titleNames = "(" + dutyName + ")";
            }

            if (_dr["BaseDept"].ToString() == "0" && _dr["Type"].ToString() != "G")
            {
                node.Text = name + baseDept + titleNames;
            }
            else
            {
                //영문사람일 경우
                if (_dr["Type"].ToString() == "M")
                {
                    node.Text = name + titleNames;
                }
                else
                {
                    node.Text = name;
                }
            }

            node.ID = _dr["ID"].ToString();					//목록 고유값
            node.Expanded = _expanded;							//확장여부
            node.ShowCheckBox = false;
            node.Attributes["CompanyID"] = _companyID.ToString();

            if (_dr["Type"].ToString().Equals("G"))
            {
                if (UseIcon.Equals("1")) node.ImageUrl = "group.gif";
            }

            if (_dr["Type"].ToString().Equals("M"))
            {
                node.ImageUrl = "member.gif";
            }

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                if (UseCheckBoxGroup != null && UseCheckBoxGroup == "1")
                {
                    node.ShowCheckBox = true;
                }
            }
            else
            {
                if (UseCheckBoxMember != null && UseCheckBoxMember == "1")
                {
                    node.ShowCheckBox = true;
                }
            }

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                node.Text = node.Text;
                node.Style.Add("font-style", "bold");
            }

            // type(G) | deptID | deptName | parentDeptID | parentDeptName | titleID | titleName | loginID | mail | officephone | fax
            // type(M) | userID | userName | deptID | deptName | titleID | titleName | loginID | mail | phone | officePhone

            StringBuilder sb = new StringBuilder();

            sb.Append(_dr["Type"].ToString());
            sb.Append("|");

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                sb.Append(_dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));

                if (_dr["ChildCnt"].ToString().ToNumber<int>() > 0)
                {
                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("guid", System.Guid.NewGuid().ToString());
                    paraDic.Add("access", Access.ToString());
                    paraDic.Add("useCheckBoxGroup", UseCheckBoxGroup);
                    paraDic.Add("useCheckBoxMember", UseCheckBoxMember);
                    paraDic.Add("useIcon", UseIcon);
                    paraDic.Add("useActiveDept", UseActiveDept);
                    paraDic.Add("cID", CompanyID.ToString());
                    paraDic.Add("pID", _dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));

                    string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                    node.ContentCallbackUrl = "/DeskplusEIP/Account/Common/GetDeptUserScript.aspx?p=" + encParams;
                }
            }
            else
            {
                sb.Append(_dr["OwnerID"].ToString());
            }

            sb.Append("|");
            sb.Append(name);
            sb.Append("|");

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                sb.Append(_dr["ParentOwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));			//하위메뉴값
            }
            else if (_dr["Type"].ToString() == "M")
            {
                sb.Append(_dr["ParentOwnerID"].ToString().Replace("G", ""));					//하위메뉴값
            }

            sb.Append("|");
            sb.Append(_dr["TitleID"].ToString());
            sb.Append("|");
            sb.Append(titleName);
            sb.Append("|");
            sb.Append(_dr["LoginID"].ToString());
            sb.Append("|");
            sb.Append(_dr["Mail"].ToString());
            sb.Append("|");
            sb.Append(_dr["Phone"].ToString());
            sb.Append("|");
            sb.Append(_dr["OfficePhone"].ToString());
            sb.Append("|");
            sb.Append(dutyName);
            sb.Append("|");
            sb.Append(deptName);
            sb.Append("|");
            sb.Append(_dr["DeptID"]);
            sb.Append("|");
            sb.Append(_dr["EmployeeNumber"]); //2015.10.14
            sb.Append("|");
            sb.Append(_companyID.ToString());
            node.Value = sb.ToString();

            return node;
        }
    }
}