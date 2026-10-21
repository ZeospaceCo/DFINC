using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text;
using ComponentArt.Web.UI;

namespace Zeospace.DeskPlus.Approval.Web.Common.FormFlow
{
    public partial class GetDeptUserScript : Base.BasePageApproval
    {
        int CompanyID;
        int DeptID;
        string Access = string.Empty;
        string UseCheckBoxGroup = string.Empty;
        string UseCheckBoxMember = string.Empty;
        string UseIcon = string.Empty;
        string UseActiveDept = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";					//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Common.FormFlow.GetDeptUserScript).Assembly;						//어셈블리 선언
            this.GetResourceManager();
            #endregion

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
                    Approval.DAL.Account userDept = new DAL.Account();
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

            //string titleName = "";

            StringBuilder nameSb = new StringBuilder();
            string baseDept = "(" + this.GetResource("OrganAddtionJob") + ")"; //겸

            #region 다국어 이름 설정
            string name = this.GetLangName(_dr["NameBase"].ToString(), _dr["NameKo"].ToString(), _dr["NameEn"].ToString(), _dr["NameJa"].ToString(),
                _dr["NameFr"].ToString(), _dr["NameZhCn"].ToString(), _dr["NameZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 직급 설정
            string titleName = this.GetLangName(_dr["TitleBase"].ToString(), _dr["TitleKo"].ToString(), _dr["TitleEn"].ToString(), _dr["TitleJa"].ToString(),
                _dr["TitleFr"].ToString(), _dr["TitleZhCn"].ToString(), _dr["TitleZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 직책 설정
            string dutyName = this.GetLangName(_dr["DutyBase"].ToString(), _dr["DutyKo"].ToString(), _dr["DutyEn"].ToString(), _dr["DutyJa"].ToString(),
                _dr["DutyFr"].ToString(), _dr["DutyZhCn"].ToString(), _dr["DutyZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            #region 다국어 부서 설정
            string deptName = this.GetLangName(_dr["DeptBase"].ToString(), _dr["DeptKo"].ToString(), _dr["DeptEn"].ToString(), _dr["DeptJa"].ToString(),
                _dr["DeptFr"].ToString(), _dr["DeptZhCn"].ToString(), _dr["DeptZhTw"].ToString(), PreLang).ToReverseXSS();
            #endregion

            string titleNames = (titleName.Length > 0) ? "(" + titleName + ")" : "";

            //겸(0) + 사람일 경우
            if (_dr["BaseDept"].ToString() == "0" && _dr["Type"].ToString() != "G")
            {
                nameSb.Append(name + baseDept + titleNames);	//목록명					
            }
            else
            {
                //겸(x) + 사람일 경우
                if (_dr["Type"].ToString() == "M")
                {
                    nameSb.Append(name + titleNames);
                }
                else //겸(x) + 부서일 경우
                {
                    nameSb.Append(name);
                }
            }

            node.Text = nameSb.ToString();

            //node.Text	= _dr["Name"].ToString();					//목록명
            node.ID = _dr["ID"].ToString();					//목록 고유값
            node.Expanded = _expanded;							//확장여부
            node.ShowCheckBox = false;

            if (_dr["Type"].ToString().Equals("G"))
            {
                if (UseIcon.Equals("1")) node.ImageUrl = "group.gif";
            }

            if (_dr["Type"].ToString().Equals("M"))
            {
                node.ImageUrl = "member.gif";
            }

            //G : 조직도 그룹, R : 가상조직도 그룹
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

            // [0]type(G) | [1]deptID | [2]deptName | [3]parentDeptID | [4]parentDeptName | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]officephone | [12]fax
            // [0]type(M) | [1]userID | [2]userName | [3]deptID       | [4]deptName       | [5]titleID | [6]titleName | [7]dutyID | [8]dutyName | [9]loginID | [10]mail | [11]phone       | [12]officePhone

            //string nodeData = "";
            StringBuilder sb = new StringBuilder();
            sb.Append(_dr["Type"].ToString());              // [0]type(G/M)
            sb.Append("|");

            if (_dr["Type"].ToString() == "G" || _dr["Type"].ToString() == "R")
            {
                sb.Append(_dr["OwnerID"].ToString().Replace(_dr["Type"].ToString(), ""));			// [1]deptID/userID

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
                    node.ContentCallbackUrl = "/DeskplusEIP/Approval/Common/FormFlow/GetDeptUserScript.aspx?p=" + encParams;
                }
            }
            else
            {
                sb.Append(_dr["OwnerID"].ToString());       // [1]deptID/userID
            }
            sb.Append("|");
            sb.Append(name);                   // [2]deptName/userName
            sb.Append("|");
            sb.Append(_dr["ParentOwnerID"].ToString().Replace("G", "")); // [3]parentDeptID/deptID      - 하위메뉴값   
            sb.Append("|");
            sb.Append(deptName);                                         // [4]parentDeptName/deptName
            sb.Append("|");
            sb.Append(_dr["TitleID"].ToString());           // [5]titleID/titleID
            sb.Append("|");
            sb.Append(titleName);                           // [6]titleName/titleName
            sb.Append("|");
            sb.Append(_dr["DutyID"].ToString());            // [7]dutyID/dutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [8]dutyName/dutyName
            sb.Append("|");
            sb.Append(_dr["LoginID"].ToString());           // [9]loginID/loginID
            sb.Append("|");
            sb.Append(_dr["Mail"].ToString());              // [10]mail/mail
            sb.Append("|");
            sb.Append(_dr["Phone"].ToString());             // [11]officephone/phone
            sb.Append("|");
            sb.Append(_dr["OfficePhone"].ToString());       // [12]fax/officePhone

            #region 수발자
            name = this.GetLangName(_dr["CPNameBase"].ToString(), _dr["CPNameKo"].ToString(), _dr["CPNameEn"].ToString(), _dr["CPNameJa"].ToString(),
                _dr["CPNameFr"].ToString(), _dr["CPNameZhCn"].ToString(), _dr["CPNameZhTw"].ToString(), PreLang);

            titleName = this.GetLangName(_dr["CPTitleBase"].ToString(), _dr["CPTitleKo"].ToString(), _dr["CPTitleEn"].ToString(), _dr["CPTitleJa"].ToString(),
                                    _dr["CPTitleFr"].ToString(), _dr["CPTitleZhCn"].ToString(), _dr["CPTitleZhTw"].ToString(), PreLang);

            dutyName = this.GetLangName(_dr["CPDutyBase"].ToString(), _dr["CPDutyKo"].ToString(), _dr["CPDutyEn"].ToString(), _dr["CPDutyJa"].ToString(),
                                    _dr["CPDutyFr"].ToString(), _dr["CPDutyZhCn"].ToString(), _dr["CPDutyZhTw"].ToString(), PreLang);

            deptName = this.GetLangName(_dr["CPDeptBase"].ToString(), _dr["CPDeptKo"].ToString(), _dr["CPDeptEn"].ToString(), _dr["CPDeptJa"].ToString(),
                _dr["CPDeptFr"].ToString(), _dr["CPDeptZhCn"].ToString(), _dr["CPDeptZhTw"].ToString(), PreLang);

            sb.Append("|");
            sb.Append(_dr["CPUserID"].ToString());          // [13]cpuserID 
            sb.Append("|");
            sb.Append(name);                                // [14]cpuserName
            sb.Append("|");
            sb.Append(_dr["CPDeptID"].ToString());          // [15]cpdeptID
            sb.Append("|");
            sb.Append(deptName);                            // [16]cpdeptName
            sb.Append("|");
            sb.Append(_dr["CPTitleID"].ToString());         // [17]cptitleID
            sb.Append("|");
            sb.Append(titleName);                           // [18]cptitleName 
            sb.Append("|");
            sb.Append(_dr["CPDutyID"].ToString());          // [19]cpdutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [20]cpdutyName
            sb.Append("|");
            sb.Append(_dr["CPLoginID"].ToString());         // [21]cploginID
            #endregion

            #region 결재자
            name = this.GetLangName(_dr["AppNameBase"].ToString(), _dr["AppNameKo"].ToString(), _dr["AppNameEn"].ToString(), _dr["AppNameJa"].ToString(),
                _dr["AppNameFr"].ToString(), _dr["AppNameZhCn"].ToString(), _dr["AppNameZhTw"].ToString(), PreLang);

            titleName = this.GetLangName(_dr["AppTitleBase"].ToString(), _dr["AppTitleKo"].ToString(), _dr["AppTitleEn"].ToString(), _dr["AppTitleJa"].ToString(),
                                    _dr["AppTitleFr"].ToString(), _dr["AppTitleZhCn"].ToString(), _dr["AppTitleZhTw"].ToString(), PreLang);

            dutyName = this.GetLangName(_dr["AppDutyBase"].ToString(), _dr["AppDutyKo"].ToString(), _dr["AppDutyEn"].ToString(), _dr["AppDutyJa"].ToString(),
                                    _dr["AppDutyFr"].ToString(), _dr["AppDutyZhCn"].ToString(), _dr["AppDutyZhTw"].ToString(), PreLang);

            deptName = this.GetLangName(_dr["AppDeptBase"].ToString(), _dr["AppDeptKo"].ToString(), _dr["AppDeptEn"].ToString(), _dr["AppDeptJa"].ToString(),
                                    _dr["AppDeptFr"].ToString(), _dr["AppDeptZhCn"].ToString(), _dr["AppDeptZhTw"].ToString(), PreLang);

            sb.Append("|");
            sb.Append(_dr["AppUserID"].ToString());         // [22]appuserID
            sb.Append("|");
            sb.Append(name);                                // [23]appuserName
            sb.Append("|");
            sb.Append(_dr["AppDeptID"].ToString());         // [24]appdeptID
            sb.Append("|");
            sb.Append(deptName);                            // [25]appdeptName
            sb.Append("|");
            sb.Append(_dr["AppTitleID"].ToString());        // [26]apptitleID
            sb.Append("|");
            sb.Append(titleName);                           // [27]apptitleName
            sb.Append("|");
            sb.Append(_dr["AppDutyID"].ToString());         // [28]appdutyID
            sb.Append("|");
            sb.Append(dutyName);                            // [29]appdutyName
            sb.Append("|");
            sb.Append(_dr["AppLoginID"].ToString());        // [30]apploginID
            #endregion 

            node.Value = sb.ToString();

            return node;
        }
    }
}