<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.EIP.Web.Admin.Company.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_EIP.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_EIP.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("CompanyManagement")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnLic" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnActive" runat="server" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnModLicense" runat="server" class="btn btn-secondary" BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnAdd" runat="server" class="btn btn-secondary"  BeginEndTag="li" />
                <cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"  BeginEndTag="li" Visible="false" />
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
            <li><%=GetResource("TotalUsers")%> : <%=UserCnt%> , <%=GetResource("License")%> : <span id="licenseCnt""></span></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->
            <!-- 테이블 시작 //-->
            
            <div id="placeholder" style="clear: both;"></div>
            
            <!-- 테이블 끝 -->

            <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                <h3><span id="pageInfo"></span></h3>
	            <span id="pager"></span>
                <asp:DropDownList ID="ddlListCount" runat="server" CssClass="aRight form-control">                                    
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- 페이징 끝 //-->

            </div><!-- end col content-area-body -->
            </div><!-- end row  -->
        </div>
    </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>     
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CompanyID = "<%= CompanyID %>";       
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var TotalItemCount = "<%=TotalCount %>"; //게시물수        
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수
        var Order = "<%=Order %>"; //정렬  
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var PageUrl = "<%= Request.Url.ToString() %>";
        var R_CompanyName = "<%=GetResource("CompanyName") %>";
        var R_DelegatorName = "<%=GetResource("DelegatorName") %>";
        var R_Telephone = "<%=GetResource("Telephone") %>";
        var R_Fax = "<%=GetResource("Fax") %>";
        var R_UserCount = "<%=GetResource("UserCount") %>";
        var R_Active = "<%=GetResource("Active") %>";
        var R_Yes = "<%=GetResource("Yes") %>";
        var R_No = "<%=GetResource("No") %>";        
        var R_SUCCESS =  "<%=GetResource("SUCCESS") %>";
        var R_ProcessingError =  "<%=GetResource("ProcessingError") %>";
        var R_LicenseCount=  "<%=GetResource("LicenseCount") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_ExcessLicenseCnt = "<%=GetResource("ExcessLicenseCnt") %>";
    </script>    
    <script type="text/javascript" src="../../Js/Admin/Company/List.js"></script>
    <script type="text/javascript">
        //JsCall("../../Js/Admin/Company/List.js");
    </script>
</body>
</html>
