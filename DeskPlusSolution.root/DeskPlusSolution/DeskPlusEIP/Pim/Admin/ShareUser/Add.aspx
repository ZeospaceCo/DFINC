<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.ShareUser.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />  
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup">
        <div class="inside">
            <div class="popup_header">
                <h1><%=GetResource("PIMShareManager")%></h1>
                <ul class="btn_right">
                    <li><cc3:ClientSmartButton ID="CbtnConfirm" runat="server" Class="btn btn-secondary" /></li>
                    <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" Class="btn btn-danger" /></li>
                    <li><cc3:ClientSmartButton ID="CbtnClose" runat="server"   /></li>
                </ul>
            </div>
            <div class="dotLine"></div>
            <div class="popup_layout_style3">
  
            <table class="table_typeB">
			<caption><%=GetResource("AuthManage")%></caption>
			<colgroup>
                <col width="80px" />
                <col />
                <col width="100px" />
            </colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("pimconfirm")%></th>
                    <td><input type="text" style="width:95% !important;" id="txtUser" readonly="readonly" disabled="disabled" /></td>
                    <td>
                    <ul class="btn_smallL"><li><a href="#" id="CbtnAddTarget" class="btn btn-primary"><%=GetResource("shareobjsel")%></a></li></ul>
                    </td>
                </tr>
            </tbody>
            </table>

            
            <div class="layout_subbox">
                <h2><%=GetResource("PimShare")%></h2>
            </div>

		        <table cellspacing="0" border="0" style="width:100%;"> 
                <tr> 
          	        <td valign="top" width="270px" style="text-align:center;"> 
          	            <div class="layout_scroll"  style="width:270px;min-height:417px;height:417px;overflow:hidden">
                            <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server"
                             UseCheckBox="true" UseHiddenRootCheckBox="true" UseIcon="true" UseCheckBoxMember="true" UseActiveDept="true" SetUseEtcFolder="1,2,3" UseCheckBoxGroup="true" />
                         </div>
                    </td>
          	        <td class="center" valign="top" style="width:100px;">
                        <cc3:ClientSmartButton ID="CbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
                    </td>
          	        <td valign="top">
                        <div class="layout_scroll"  style="min-height:417px;height:417px;">
                        <div id="placeholder" style="clear: both;"></div>
                        </div>
                        
          	        </td>            
                </tr> 
		        </table>    
		        
            </div>
        </div>
    </div> 

        
    <div style="display: none">
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
        <asp:linkbutton id="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:linkbutton>
        <asp:TextBox ID="tbAuthValue" runat="server" Width="0"></asp:TextBox>
    </div>    
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/ShareUser/Add.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript">
        var CatID = "<%=CatID %>";
        var CategoryID = "<%=CategoryID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var UserList = "";
        var CompanyID = "<%=CompanyID %>";
        var UserID = 0;
        var AlertScriptShort = "<%=GetResource("LimitSpecialCharacter") %>" + "\n(" + LimitSpeChar + ")";
        var AlertScriptLong = "<%=GetResource("AlertScriptContnet") %>";
        var PreLang = "<%=PreLang%>"; 

        function SetUserinfo(arg) {
          
            if (arg.length > 0) {
                var arVal = arg.split('|');

                $("#txtUser").val(arVal[2]);
                UserID = arVal[1];
            
                Add.GetShareList();
            }
        }

        var ApplyDelete = '<%=GetResource("ApplyDelete") %>'; 	//삭제하시겠습니까     
        var AddUserSelect = '<%=GetResource("AddUserSelect") %>'; 	//공유자를 추가할 유저를 선택하세요         
        var allDelete = '<%=GetResource("allDelete") %>'; 	//
        var NoneSelUser = '<%=GetResource("NoneSelUser") %>';
        var t_object = '<%=GetResource("ObjePerson") %>';
        var t_del = '<%=GetResource("Del") %>';
        var UserSelect = '<%=GetResource("UserSelect") %>';
        var WaitMent = '<%=GetResource("waitment")%>';
        var BtnCheck = 'N';
    </script> 
</body>
</html>
