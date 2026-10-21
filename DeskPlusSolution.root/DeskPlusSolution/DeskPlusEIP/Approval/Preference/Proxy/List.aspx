<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Preference.Proxy.List" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("SetProxy") %></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <li id="modifyInfo" style="display:none"><a href="#" id="ModList" class="btn"><span ><%=GetResource("ModifyList")%></span></a></li>
                    <li><a href="#" id="Add" class="btn btn-secondary"><span><%=GetResource("AddProxy") %></span></a></li>
                    <li><a href="#" id="Del" class="btn btn-danger"><span><%=GetResource("DelProxy") %></span></a></li>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    
                    <div class="col-12 content-area-body">
                        <!-- 테이블 시작 //-->
                        <div id="placeholder" style="clear: both;"></div>
                        <!-- 테이블 끝 //-->

                        <!-- 페이징 시작 //-->
                        <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                            <h3><span id="pageInfo"></span></h3>
	                        <span id="pager"></span>
                            <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                <asp:ListItem Text="10 " Value="10" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <!-- 페이징 끝 //-->
                
                        <div class="app_subtxt"><%=GetResource("ProxyText6")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText3")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText4")%></div>
                        <div class="app_subtxt"><%=GetResource("ProxyText5")%></div>
                    </div><!-- end col-12 content-area-body -->
                
                </div><!-- end row  -->
            </div><!-- end layout_content  -->
        
        </div><!-- end layout_full  -->
    </div><!-- end ifm_layout_body  -->
    <div style="display: none;">
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>
        <asp:TextBox ID="tbxHiddenValue" runat="Server" Width="0"></asp:TextBox>
        <asp:TextBox ID="tbxHiddenDeptValue" runat="Server" Width="0"></asp:TextBox>
    </div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수                   
        var ProxyCnt = "<%=ProxyCnt %>"; //대결시작 카운트(IsProxy = 'Y' 갯수)

        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>"; 
        var R_ConfirmDeleteProxyPerson = "<%=GetResource("ConfirmDeleteProxyPerson") %>";
        var R_CheckDeleteProxyPerson = "<%=GetResource("CheckDeleteProxyPerson") %>";

        var R_Photo = "<%=GetResource("Photo") %>";
        var R_UserName = "<%=GetResource("Name") %>";
        var R_Mail = "<%=GetResource("Mail") %>";
        var R_Mobile = "<%=GetResource("Mobile") %>";
        var R_Cphone = "<%=GetResource("Cphone") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_State = "<%=GetResource("State") %>";
        var R_NoData = "<%=GetResource("NoDataProxy") %>"; 
        var R_Msg_Resource04 = "<%=GetResource("Msg_Resource04") %>"; 
        var R_Msg_Resource05 = "<%=GetResource("Msg_Resource05") %>"; 
        var R_Msg_Resource06 = "<%=GetResource("Msg_Resource06") %>";
        var R_Msg_Resource07  = "<%=GetResource("Msg_Resource07") %>";
        var R_Msg_Resource08  = "<%=GetResource("Msg_Resource08") %>";
        var R_Msg_Resource09  = "<%=GetResource("Msg_Resource09") %>";
        var R_StateInfo = "<%=GetResource("EnforceStateInfo") %>"; 
        var R_IsModfy = "<%=GetResource("IsModfy") %>"

        var R_ProxyDate = "<%=GetResource("AbsencePeriod")%>";
        var R_ProxyType = "<%=GetResource("TypeAbsence")%>";
        var R_AllDay = "<%=GetResource("AllDay")%>";

        var PageUrl = "<%= Request.Url.ToString() %>";
        var AttachFileInfoList = null;
        var WindowObjectReference = null; //새창용
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja   
       
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Preference/Proxy/List.js?v=1"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Preference/Proxy/List.js");
    </script>

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
    <form id="frmTotalDownLoad" name="frmTotalDownLoad" method="post" target="openWin">
    <input type="hidden" name="fileList" id="fileList" />
    <iframe id="ifrmAttachFile" frameborder="0" width="0" height="0" scrolling="no" src="about:blank">
    </iframe>
    </form>
</body>
</html>
