<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSearch.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Schedule.ListSearch" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
    <meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Pim.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Pim.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/ContextMenu.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/ContextMenu.css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="layout_body">
    <div class="layout_full">
    
          <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1>
            <%=GetResource("PimSearchEnquiry")%>            
            </h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="CbtnSchedule" runat="server"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="CbtnExcel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li>
                  <asp:DropDownList ID="ddlSearchKind" runat="server" CssClass="searchselect form-control"> </asp:DropDownList>
                  <span id="spanDate" style="display:none;">
                  <input type="text" id="startDate" size="12" readonly="readonly" class="clsDate searchtext form-control" />-<input type="text" id="endDate" size="12" readonly="readonly" class="clsDate searchtext form-control" />
                    &nbsp;
                  </span>
                </li>
                <li><asp:DropDownList ID="ddlPerson" Visible="false" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><asp:DropDownList ID="ddlValue" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li>
                   <input type="text" id="dummy" style="display: none;" />
                   <input name="ctbSearch" type="text" id="ctbSearch" runat="server" class="searchtext form-control" placeholder="검색어 입력" />
                </li>
                <li><a href=# class="searchbtn btn-secondary" onclick="ListSearch.SearchButton()"><%= GetResource("SEARCHTEXT") %></a></li>
            </ul>
        </div>

        <div id="layout_content">

            <div class="row">
				<div class="col-12 content-area-body"><!-- width 100%(col-12) -->
                    <!-- 테이블 시작 //-->
                    <div id="placeholder" style="clear: both;"></div>            
                    <!-- 테이블 끝 //-->

                    <!-- 페이징 시작 //-->
                    <div class="paging_num_s1"> 
                        <h3><span id="pageInfo"></span></h3>
	                    <div id="Pager1"  class="board_num">    
                        <span id="pager"></span>
                        <select class="aRight form-control" id="selPageDistance" >
                        <option value="5">5 </option>
	                    <option selected="selected" value="10">10</option>
	                    <option value="20">20</option>
	                    <option value="30">30</option>
                        </select>
                        </div>  
                    </div>
                    <!-- 페이징 끝 //-->
                </div>
            </div>
        </div>        
    </div>
    </div>

    <div style="display:none;">
    <asp:LinkButton ID="lbtnexcel" runat="server" OnClick="lbtnexcel_Click"></asp:LinkButton>        
    <input type="text" id="hdexcel" runat="server" />
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick-ko.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js" ></script>     
    <script type="text/javascript">
        var myperson = '<%= GetResource("myviewschedule") %>';
        var AllSchedule = '<%= GetResource("pim4") %>';
        var Distance = '<%=Distance %>';
        var inquryPerson = '<%=inquryPerson %>';
        var DatePeriod = '<%=DatePeriod %>';
        var SearchKind = '<%=SearchKind %>';
        var SerachValue = '<%=SerachValue %>';
        var StartDate = '<%=StartDate %>';
        var EndDate = '<%=EndDate %>';
        var UserId = '<%=UserID %>';
        var CompanyID = '<%=CompanyID%>';
        var TotalItemCount = 0;          
        var PageSize = 10;  //페이지크기
        var PageSizePageNo = 0; //현재페이지
        var RecordPerPage = 10;
        var CurrentPage = 1;
        var R_Previous = '<%=GetResource("Previous") %>';
        var R_Next = '<%=GetResource("Next") %>';
        var Order = "";
        var InputSearchDay = '<%=GetResource("InputSearchDay") %>';
        var col1 = '<%=GetResource("Name") %>';
        var col2 = '<%=GetResource("KIND") %>';
        var col3 = '<%=GetResource("DATE2") %>';
        var col4 = '<%=GetResource("Title") %>';
        var col5 = '<%=GetResource("Time") %>';
        var TextEmpty = '<%=GetResource("TextEmpty") %>';
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";                        
        var PreLang = "<%=PreLang%>"; 
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        //목록버튼
        var PageScope = '<%=PageScope%>';
        var paraDate = '<%=Date%>';
        var paraOpentype = '<%=Opentype%>';       
        
        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";         
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Schedule/ListSearch.js" ></script> 
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Schedule/ListSearch.js");        
    </script>
</body>
</html>
