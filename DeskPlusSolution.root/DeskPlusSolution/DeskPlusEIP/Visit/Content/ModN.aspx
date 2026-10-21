<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Visit.Web.Content.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Visit.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Visit.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body" class="write">
    <div class="layout_full">
        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("VisitReservation")%><%=GetResource("Mod")%></h1>
            <!-- 버튼 //-->
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
            <div class="row">
             <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->   

                <table cellspacing="0" class="table_typeB">
		        <colgroup>
                    <col width="80px" />
                    <col />
		        </colgroup>
		        <tbody>
                <tr>
                    <th><%=GetResource("Subject")%></th>
                    <td><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="150" runat="server" Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                </tr>
                <tr>
		            <th><%=GetResource("Date")%></th>
		            <td id="tdDate">
                        <div id="startEndDate">
                            <input type="text" id="startDate" size="10"  runat="server" readonly /> 
				            <asp:DropDownList ID="ddlstartTime" runat="server" CssClass="searchselect form-control" >
				            </asp:DropDownList>
				            ~
				            <input type="text" id="endDate" size="10"  runat="server" readonly /> 
				            <asp:DropDownList ID="ddlendTime" runat="server" CssClass="searchselect form-control" >
				            </asp:DropDownList>
				            </div>
                    </td>
                </tr>
                <tr>
			        <th><%=GetResource("Repeat")%></th>
			        <td>
                        <span class="aLeft">
                        <input  type="text" class="text required" value="" id="txtrepeat" runat="server" readonly style="width:400px !important;"/>&nbsp;
                        </span>
                        <ul class="btn_smallL">
                        <li><cc3:ClientSmartButton ID="cbtnRepeat" runat="server"></cc3:ClientSmartButton></li>
                        <li><cc3:ClientSmartButton ID="cbtnRepCancel" runat="server"></cc3:ClientSmartButton></li>
                        </ul>
                    </td>
		        </tr>
                </tbody>
                </table>

                <div id="datePick_dummy"></div>

                <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab" style="display:none">
                        <li><a id="ActiveX" ><span>ActiveX</span> </a></li>
                        <li><a id="NonActiveX" class="tab_on"><span>NonActiveX</span> </a></li>
                    </ul>
                    <div class="divNonEditor">
                        <div id="divXFE" style="height: 5px;"> 
                            <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>                       
                            <script type="text/javascript">
                                xfe = new XFE({
                                    basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                                    width: '100%',
                                    height: '550px',
                                    ignoreMinHeight: true
                                });
                                xfe.render('divXFE');
                            </script>
                        </div>
                    </div>
                </div>   

            </div><!-- end col content-area-body-right -->
			<div class="col-12 col-lg-3 col-xl-4 content-area-body-right">
                <ul>
                    <!-- 기본공유자 -->
                    <li class="title">
                         <%=GetResource("Share")%>
                    </li>
                    <li><div id="phShare" style="clear: both;"></div></li>    
                    <!-- 기본공유자 -->

                    <!-- 공유자 선택 -->
                    <li class="title">
                         <%=GetResource("ShareSetting")%>
                         <a id="cbtnAddShare" title="<%=GetResource("ShareAdd")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                    </li>
                    <li><ul id="phShareList" class="table_typeT ul_typeT"></ul></li>    
                    <!-- 공유자 선택 -->
                                         
                    <!-- 자원 선택 -->
                    <li class="title">
                         <%=GetResource("ReserveResource")%>
                         <a id="cbtnAddResource" title="<%=GetResource("SelectResource")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                    </li>
                    <li><ul id="phRMList" class="table_typeT ul_typeT"></ul></li>    
                    <!-- 자원 선택 -->    
                    
                    <!-- 첨부 파일 -->
                    <li class="title">
                         <%=GetResource("Attach")%>
                    </li>
                    <li><ul id="phFile" class="table_typeT ul_typeT"></ul></li>    
                    <!-- 첨부 파일 -->        
   
                    <!-- 추가할 첨부 파일 -->
                    <li class="title">
                         <%=GetResource("AddAttach")%>
                         <a id="cbtnAddFile" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                    </li>
                    <li><div id="phSpoolFile" style="clear: both;"></div></li>    
                    <!-- 추가할 첨부 파일 -->
                </ul>    
            </div>
        </div>        
    </div>
    </div>
    <div style="display:none;">
        <input type="hidden" id="hdContent" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbRepeat" Style="display: none"   runat="server" Width="0px"></asp:TextBox>   
        <asp:TextBox ID="hdRepeatSel" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeatChk" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdRepeatType" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdSelectType" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdIntervalCnt" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption1" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption2" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption3" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption4" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption5" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption6" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdOption7" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdMaxReptCnt" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="hdEndDate" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenResource" Style="display: none"   runat="server" Width="0px"></asp:TextBox>    
        <asp:TextBox ID="tbHiddenShare" Style="display: none"   runat="server" Width="0px"></asp:TextBox>  
        <asp:LinkButton ID="lbtnConfirm" runat="server" OnClick="lbtnConfirm_Click"></asp:LinkButton> 
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"  Style="display: none" rows="1" cols="20"></textarea>                 
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>      
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" >
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var RowMax = 0;        
        var RowShareMax = 0;    
        var ReserveID = "<%=ReserveID%>";
        var ReserveGroupID = "<%=ReserveGroupID%>";   
        var TmpDate = "<%=TmpDate%>";
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var TagGubun = "<%=TagGubun%>";
        var BtnCheck = "N";      
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja     
        var BeforeStartDate = "<%=BeforeStartDate%>";
        var GroupStartDate = "<%=GroupStartDate%>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_ResourcesTitle = "<%=GetResource("ResourcesTitle") %>";
        var R_Qty = "<%=GetResource("Qty") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_InputSubject = "<%=GetResource("InputSubject") %>";
        var R_ApplyStartDate = "<%=GetResource("ApplyStartDate") %>";
        var R_StartDateEndDiffer = "<%=GetResource("StartDateEndDiffer") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_StartDateEndDate	= "<%=GetResource("StartDateEndDate") %>";
        var R_UsableResource = "<%=GetResource("UsableResource") %>";
        var R_NoRepeatDay = "<%=GetResource("NoRepeatDay") %>";
        var R_QtyZero = "<%=GetResource("QtyZero") %>";
        var R_ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var R_Processing = "<%=GetResource("Processing") %>";
        var R_Title = "<%=GetResource("Title") %>";
        var R_Gubun  = "<%=GetResource("Gubun")  %>";
        var R_ErrorDelFile = "<%=GetResource("ErrorDelFile") %>";
        var R_DelAttach = "<%=GetResource("DelAttach") %>";
        var R_EditorScript = "<%=GetResource("EditorScript") %>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Visit/Js/Content/Mod.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Visit/Js/Content/Mod.js");
    </script>
</body>
</html>
