<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reply.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Admin.Content.Reply" %>

<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><asp:Label ID="lbCategory" runat="server" Text=""></asp:Label></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">                    
                    <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
                    <li><cc3:ClientSmartButton ID="cbtnCancel" runat="server"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_content">

                <div class="row">
                    <div class="col-12 col-lg-9 col-xl-8 content-area-body-left">


                        <table cellspacing="0" class="table_typeB">
                            <colgroup>
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th>
                                        <%=GetResource("Title")%>
                                    </th>
                                    <td>
                                        <cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server"
                                            Required="true" ScriptSubmitFunName="Add.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="last-child">
                                        <%=GetResource("Gubun")%>
                                    </th>
                                    <td class="last-child">
                                        <select name="ddlCatHeader" id="ddlCatHeader" runat="server">
                                        </select>
                                        <select name="ddlFontColor" id="ddlFontColor" runat="server">
                                        </select>
                                        <select name="ddlFontSize" id="ddlFontSize" runat="server">
                                        </select>
                                        <input id="cbxBold" type="checkbox" name="cbxBold" runat="server" /><label for="cbxBold"><%=GetResource("Bold") %></label>
                                        <input id="cbOptionModify" type="checkbox" name="cbOptionModify" runat="server" /><label
                                            for="cbOptionModify"><%=GetResource("ModifyEveryone")%></label>
                                        <input id="cbOptionTop" type="checkbox" name="cbOptionTop" runat="server" /><label
                                            for="cbOptionTop"><%=GetResource("RegisterOptionTop")%></label>
                                        <select name="ddlDay" id="ddlDay" style="display: none" runat="server">
                                            <option value="">---</option>
                                            <option value="5">5</option>
                                            <option value="10">10</option>
                                            <option value="15">15</option>
                                            <option value="20">20</option>
                                            <option value="25">25</option>
                                            <option value="30">30</option>
                                            <option value="35">35</option>
                                            <option value="40">40</option>
                                            <option value="45">45</option>
                                            <option value="50">50</option>
                                            <option value="55">55</option>
                                            <option value="60">60</option>
                                            <option value="65">65</option>
                                            <option value="70">70</option>
                                            <option value="75">75</option>
                                            <option value="80">80</option>
                                            <option value="85">85</option>
                                            <option value="90">90</option>
                                            <option value="95">95</option>
                                            <option value="100">100</option>
                                        </select>
                                        <asp:CheckBox ID="cbOptionAnonymity" runat="server" />
                                    </td>
                                </tr>
                                <tr id="trAlarmUser">
                                    <th class="last-child" rowspan="2"><%=GetResource("AlarmUser")%></th>
                                    <td >
                                        <asp:HiddenField ID="hdAlarmUser" runat="Server"></asp:HiddenField>
                                        <div id="InputAlarmUser" style="float:left;height: 27px; width: 85%; padding-left: 2px; padding-right:2px;padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>
                                        <ul class="btn_right">                        
                                            <cc3:ClientSmartButton ID="cbtnWhole" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                            <cc3:ClientSmartButton ID="cbtnAlarmUser" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                        </ul>
                                    </td>                    
                                </tr>                
                                <tr id="trAlarmUserOption">
                                    <td >                     
                                        <span id="spnAlarmUser">   
                                        <input id="cbxAlarmUser" type="checkbox" name="cbxAlarmUser" runat="server"/><label for="cbxAlarmUser"><%=GetResource("List_SubGroup")%></label>            
                                        </span>
                                    </td>             
                                </tr>
                            </tbody>
                        </table>

                        <div id="tabs" class="layout_subbox">
                            <ul id="tab" class="tab" style="display:none">
                                <li><a id="ActiveX"><span>ActiveX</span> </a></li>
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
                            <!-- 첨부파일 -->
                            <li class="title">
                                <%=GetResource("AttachName")%>
                                <div class="float-right">                            
                                <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                                <a href="#" id="cbtnLinkURL" title="<%=GetResource("URLAddBtn")%>"><i class="fas fa-link text-secondary"></i></a>
                                 </div>
                            </li>
                            <li><div id="placeholder_file" style="clear: both;"></div></li>
                            <li id="liLinkURL"><ul id="placeholder_URLfile" class='table_typeT ul_typeT'></ul></li> 
                            <!-- 첨부파일 리스트 -->
                        </ul>
                  </div><!-- end col content-area-body-right -->
             
			    </div><!-- end row  -->
		    </div><!-- end layout_content  -->
        
	    </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->

    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"  runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <!-- ==================================================================================== -->
        <asp:TextBox ID="hiddenLinkURLName" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <asp:TextBox ID="hiddenLinkURL" Style="display: none" Width="0"   runat="server"></asp:TextBox>
    </div>
    </form>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>"; //트리 최상단 카테고리
        var CategoryID = "<%=CategoryID %>"; //선택한 트리 카테고리
        var SearchTerm = "<%=SearchTerm %>"; //날자 분류
        var SearchTermValue = "<%=SearchTermValue %>"; //날자값
        var SearchGroup = "<%=SearchGroup %>"; //분류
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort
        var OptionListType = "<%=OptionListType %>";
        var GoPage = "<%=GoPage %>";
        var TrySaveCnt = 0; 
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_InputTitle = "<%= GetResource("Add_InputTitle") %>";
        var R_Add_SelectOptionTop = "<%= GetResource("Add_SelectOptionTop") %>";
        var R_Add_InputContent = "<%= GetResource("Add_InputContent") %>";
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_TempSaveAlert = "<%=GetResource("TempSaveAlert") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_NotImageFormat = "<%=GetResource("NotImageFormat") %>";
        var R_ScreenShot = "<%=GetResource("ScreenShot") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";

        var BoaComID = "<%=BoaComID %>"; 
        var OptionAlarm = "<%=OptionAlarm %>";  
        var OptionUseDept = "<%=OptionUseDept %>";   
        var allUser = "<%=allUser %>";   
        var R_AleadyRegisted = "<%= GetResource("AleadyRegisted") %>";
        var R_Del = "<%= GetResource("Del") %>";  
        var R_PopupSelect = "<%= GetResource("PopupSelect") %>";    
        var Whole = "<%= GetResource("Whole") %>";

        var OptionLinkURL = "<%=OptionLinkURL %>";      
        var RowMax = 0;
        var RowSeq = 0;
        var RowMaxFile = 0; 
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Admin/Content/Reply.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Board/Js/Admin/Content/Reply.js");
    </script>
</body>
</html>
