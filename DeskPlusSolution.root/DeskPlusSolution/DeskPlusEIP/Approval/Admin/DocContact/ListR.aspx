<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListR.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.DocContact.ListR" %>

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
                <h1><%=GetResource("ReceiveSender")%></h1>
                <!-- 버튼 //-->
                <span class="app_caption">* <%=GetResource("ScriptAcc1")%></span>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnActive" BeginEndTag="li" runat="server" class="btn btn-secondary"></cc3:ClientSmartButton>
                    <cc3:ClientSmartButton ID="cbtnDel" BeginEndTag="li" class="btn btn-danger" runat="server"></cc3:ClientSmartButton>
                </ul>
            </div>
            <div id="layout_content">
                <div class="row">
                    <div class="col col-lg-12 content-area-body-left" style="min-height:680px"><!-- width 100%(col-12) -->
                        <div class="app_fdr">
						    <table width="100%" cellspacing="0" cellpadding="0">
						    <tr>
						    <td width="240px" valign="top" style="vertical-align:top">
                                <div class="left">
                                    <div class="cont">
                                        <zeoDeptTree:UCDeptTreeScript ID="UCDeptTreeScript1" runat="server" UseIcon="true"
                                            UseCheckBox="false" UseActiveDept="true" />
                                    </div>
                                </div>
						    </td>
						    <td valign="top">
						        <!-- 테이블 시작 //-->
                                <div id="placeholder" style="clear: both;"></div>
                                <!-- 테이블 끝 //-->

                                <!-- 페이징 시작 //-->
                                <div id="Pager1" name="Pager1" class="paging_num_s1">
                                    <h3>
                                        <span id="pageInfo"></span>
                                    </h3>
                                    <span id="pager"></span>
                                    <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">
                                        <asp:ListItem Text="5 " Value="5"></asp:ListItem>
                                        <asp:ListItem Text="10 " Value="10" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="20 " Value="20"></asp:ListItem>
                                        <asp:ListItem Text="30 " Value="30"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <!-- 페이징 끝 //-->
                                <span class="app_subtxt">* <%=GetResource("ScriptAcc2")%></span>
						    </td>
						    </tr>
						    </table>
                        </div>
                    </div><!-- end col content-area-body-left -->
                </div><!-- end row  -->
            </div>
        </div>
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

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var DeptID = "0";
        var Type = "R";

        var CurrentPage = "<%=CurrentPage %>";  //현재페이지
        var TotalItemCount = "<%=TotalItemCount %>"; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수                            
        var ActiveCnt = "0"; //지정시작 카운트(IsActive = '1' 갯수)

        var R_Photo = "<%=GetResource("Photo") %>";
        var R_Name = "<%=GetResource("Name") %>";
        var R_DeptName = "<%=GetResource("DeptName") %>";
        var R_IsActive = "<%=GetResource("IsActive") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_Clear = "<%=GetResource("Clear") %>";
        var R_SetUp = "<%=GetResource("SetUp") %>";

        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PageUrl = "<%= Request.Url.ToString() %>";

    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/DocContact/ListR.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/DocContact/ListR.js");
    </script>

</body>
</html>