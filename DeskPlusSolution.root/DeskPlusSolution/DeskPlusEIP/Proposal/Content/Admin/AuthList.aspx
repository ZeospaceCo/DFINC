<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthList.aspx.cs" Inherits="Zeospace.DeskPlus.Proposal.Web.Content.Admin.AuthList" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Proposal.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Proposal.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/AutoComplete.css" />
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script> 
    <script type="text/javascript">
        var CompanyID = "<%=CurrentUserInfo.CompanyID %>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var AuthArrayList = "<%=AuthArrayList %>";
        var RowMax = "<%=MaxRowMax %>";
    </script>     
    <script type="text/javascript" src="/DeskPlusEIP/Proposal/Js/Content/Admin/AuthList.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Proposal/Js/Content/Admin/AuthList.js");
    </script>    
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
    
        <div id="layout_header">
            <h1>1차심사위원 관리</h1>
            <ul class="btn_right">
                <li><div id="loading"></div></li>
                <cc3:ClientSmartButton ID="CbtnMod" runat="server" BeginEndTag="li" Text="저장" Class="btn btn-secondary" />
            </ul>
        </div>    

        <div id="layout_content">
             <div class="row">
                <div class="col-12">
                    <div id="divAuth">
                        <!-- 권한설정 start-->    
                        <div class="acc_auth_left">
                             <div class="layout_scroll" style="min-height:400px;height:400px;">
                                <zeoTree:UCUserDeptTreeScript ID="DeptUserTreeScript1" runat="server" UseActiveDept="true" UseCheckBoxGroup="false" UseCheckBoxMember="true" UseIcon="true" />
                            </div>
                        </div>
                        <div class="acc_auth_middle">
                            <ul>
                                <li><cc3:ClientSmartButton ID="CbtnAdd" runat="server" Text="추가" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                            </ul>
                        </div>
                        <div class="acc_auth_right">                    
                            <table class="table_typeB" id="contentTable">
                            <caption>1차심사위원리스트</caption>
                            <colgroup>
                                <col />
                                <col width="80px"/>
			                </colgroup>
			                <tbody>
				            <tr>
                                <th>명칭</th>
                                <th class="center">삭제</th>
                            </tr>
                            <%=AuthTable%>                    
                            </tbody>
                            </table>
                        </div>
                        <!-- 권한설정 end-->    
                    </div>
                 </div>
              </div>
        </div>    
    </div>
</div>
    <div style="display:none;">
        <input name="tvSelectedID" type="text" id="tvSelectedID" />
        <input name="tvSelectedValue" type="text" id="tvSelectedValue" />
    </div> 
    </form>
    <!-- ############# start of modal ############## -->
    <div id="zeoModalContact">
        <!-- 드래그공간(br Tag 처리) //-->        
        <div id="zeoModalDrag" class="modal-drag"><p></p></div>
        <form id="zeoModalForm" name="zeoModalForm" target="zeoModalIfrm" method="post"></form>
        <iframe id="zeoModalIfrm" name="zeoModalIfrm" frameborder="0" scrolling="no" src="about:blank"></iframe>
    </div>
    <div id="zeoModalBG"></div>
    <!-- ############# end of modal ############## -->
</body>
</html>
