<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Admin.ShareUser.List" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Account.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Account.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />   
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("PIMShareManager")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnMySchedule" runat="server" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnAdd" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>

       <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><div id="loading"></div></li>
                <li>
                    
                </li>
                <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li>
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px"  InputMode="None" class="searchtext form-control" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" BeginEndTag="li" Class="searchbtn btn-secondary" /></li>         
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
            	<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->            
                    <div id="plShareUser" style="clear: both;"></div>            
                    <!-- 테이블 끝 //-->

                    <!-- 페이징 시작 //-->
                    <div id="Pager1" name="Pager1" class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <span id="pager"></span>   
                        <select class="aRight form-control" id="ddlListCount" >
                        <option selected="selected" value="5">5 </option>
	                    <option value="10">10</option>
	                    <option value="20">20</option>
	                    <option value="30">30</option>
                        </select>
                    </div>
                    <!-- 페이징 끝 //-->
                </div>
            </div>
        </div>
    </div>
    </div>
    <!-- ############# hidden value ############## -->
    <div style="display:none" >
        <input type="hidden" id="hdSel" />

    </div>
    <!-- #############  ############## -->
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>  
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang %>";
        var Name = "";
        var Kind = "G";
        var CompanyID = '<%= CompanyID %>';
        var PageSize = 5;
        var PageSizePageNo = 1;
        var SearchValue = "";
        var TotalItemCount = 0;
        var CurrentPage = 1;
        var R_Previous = '<%=GetResource("Previous") %>';
        var R_Next = '<%=GetResource("Next") %>';
        var RecordPerPage = 5;
        var SelectUser = "";
        var col1 = '<%=GetResource("DEPTNAME") %>';
        var col2 = '<%=GetResource("Pay") %>';
        var col3 = '<%=GetResource("Post") %>';
        var col4 = '<%=GetResource("Name") %>';
        var TextEmpty = '<%=GetResource("TextEmpty") %>';

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Admin/ShareUser/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Pim/Js/Admin/ShareUser/List.js");
    </script>
</body>
</html>
