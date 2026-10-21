<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CirMemo.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Folder.Dept.CirMemo" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
</head>
<body>

<form id="form1" runat="server">

<div class="layer_popup">
    <div class="inside">
        <div class="popup_header">
            <h1><%=GetResource("Circulation")%></h1>
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnShortReply" runat="server" BeginEndTag="li" class="btn btn-secondary" />
                <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
            </ul>
        </div>
        <div class="dotLine"></div>
        <div class="popup_layout_style3" style="min-height:420px;">

            <div class="app_target">

            <div class="left">
                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
            </div>
            <div class="mid">                
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton>
            </div>
            <div class="right">

                

                <div style="overflow:auto;max-height:235px">
                <table class="table_typeA" id="contentTable">
                    <caption><%=GetResource("CirList")%></caption>
                    <colgroup>
                        <col />
                        <col width="60px"/>
			        </colgroup>
			        <tbody>
				    <tr>
                        <th class="center" id="r_name"><%=GetResource("TargetUser")%></th>
                        <th class="center" id="r_del"><%=GetResource("Btn_Del")%></th>
                    </tr>                
                    </tbody>
                </table>
                </div>

                <div class="layout_subbox">
                    <h2><%=GetResource("CirOpinion")%></h2>
                </div>
                <div class="memo"><textarea name="textarea" id="textarea"  class="textarea" style="width:100%;"></textarea></div>
            </div>

            </div>

        </div> 
    </div>
</div>
<div style="display: none">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <input name="tvSelectedID" type="text" id="tvSelectedID" />
    <input name="tvSelectedValue" type="text" id="tvSelectedValue" />

    <textarea id="htaFormHTML" style="DISPLAY: none" name="htaFormHTML" rows="2" cols="40" runat="server"></textarea>
    <textarea id="htaFormTEXT" style="DISPLAY: none" name="htaFormTEXT" rows="2" cols="40" runat="server"></textarea>
    <asp:textbox id="tbOpinion" Runat="server" Width="0"></asp:textbox>

    <asp:textbox id="tbEtc1" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc2" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc3" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc4" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc5" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc6" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc7" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc8" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc9" Runat="server" Width="0"></asp:textbox>
    <asp:textbox id="tbEtc10" name="tbEtc10" Runat="server" Width="0"></asp:textbox>

    <input id="userList" name="userList" Runat="server" type="text"/>
    <input id="documentId" name="documentId" Runat="server" type="text"/>
</div>
</form>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ChildDocID = "<%=ChildDocID %>";
        var MenuName = "<%=MenuName %>";
        var UserList = "";
        var AuthArrayList = "";
        var RowMax = 1;

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";

        var R_Name = "<%=GetResource("Name")%>";   //"이름";
        var R_Del = "<%=GetResource("Btn_Del")%>";    //"삭제";
        var R_Processing = "<%=GetResource("MSG_SUCCESS")%>"; //"처리되었습니다.";
        var R_NoContent = "<%=GetResource("NoContent")%>";  //"내용이 없습니다.";
        var R_Add_ReplyInputContent = "<%=GetResource("NeedContents")%>";  //"내용을 입력해 주세요.";
        var R_SettingAuthority = "<%=GetResource("SelUser")%>";  //"대상자를 선택해 주시기 바랍니다.";
        var R_MSG_CirOpinion = "<%=GetResource("MSG_CirOpinion")%>";  //"회람의견을 입력해 주세요";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Folder/Dept/CirMemo.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Folder/Dept/CirMemo.js");
    </script>
</body>
</html>