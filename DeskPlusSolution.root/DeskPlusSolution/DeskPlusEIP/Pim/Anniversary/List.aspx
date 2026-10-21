<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlus.Pim.Web.Anniversary.List" %>
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
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("AnnivControl")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <cc3:ClientSmartButton ID="cbtnMySchedule" runat="server"  BeginEndTag="li"></cc3:ClientSmartButton>
                <cc3:ClientSmartButton ID="cbtnExcel" runat="server"  BeginEndTag="li"></cc3:ClientSmartButton>

				<cc3:ClientSmartButton ID="cbtnAdd" runat="server"  BeginEndTag="li" Class="btn btn-secondary"></cc3:ClientSmartButton>
            </ul>
        </div>

        <div id="layout_srh">
            <ul class="btn_searchbox">
                <li><asp:DropDownList ID="ddlCategory" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li><asp:DropDownList ID="ddlSearchType" runat="server" CssClass="searchselect form-control"></asp:DropDownList></li>
                <li> 
                    <input type="text" id="dummy" style="display: none;" />
                    <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
					<div id="startEndDate">
                    <input type="text" id="startDate" size="12" class="clsDate searchtext form-control" readonly="readonly" />-<input type="text" id="endDate" size="12" class="clsDate searchtext form-control" readonly="readonly" />                              
                    </div>
				</li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn btn-secondary"></cc3:ClientSmartButton></li>
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
		                    <span id="pager"></span>	
                            <select class="aRight form-control" id="selSize">
                                <option value="5">5 </option>
	                            <option selected="selected" value="10">10</option>
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
    <script src="../../Html/Js/DeskPlusEIPComponent.js" type="text/javascript"></script>    
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/        
        var ImagePath = "<%=BaseImgPath%>";
        var AttachFileInfoList = null;
        var CompanyID = "<%=CompanyID %>";
        var type = 'J,M';
        var opentype = '2';
        var Searchkey = "<%=Searchkey %>"; ; //검색분류
        var SearchValue = "<%=SearchValue %>"; ; //검색어
        var TotalItemCount = 0;
        var PageSize = 5;  //페이지크기
        var PageSizePageNo = 1; //현재페이지
        var RecordPerPage = 10;
        var CurrentPage = "<%=CurrentPage %>"; 
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var Order = "<%=Order %>";
        var Starttime = "<%=Starttime %>";
        var Endtime = "<%=Endtime %>";
        var InputSearchDay = "<%=GetResource("InputSearchDay") %>";
        var col1 = "<%=GetResource("KIND") %>"; //구분
        var col2 = "<%=GetResource("Title") %>"; //제목
        var col3 = "<%=GetResource("Period") %>"; //기간
        var col4 = "<%=GetResource("WRITER") %>"; //작성자
        var PreLang = "<%=PreLang%>"; 
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var TextEmpty = "<%=GetResource("TextEmpty") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js" ></script>
    <script type="text/javascript" src="/DeskPlusEIP/Pim/Js/Anniversary/List.js" ></script>           
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Pim/Js/Anniversary/List.js");
    </script>
</body>
</html>

