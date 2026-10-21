<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Mod.aspx.cs" Inherits="Zeospace.DeskPlus.Board.Web.Content.Mod" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Board.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Board.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
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
                    <cc3:ClientSmartButton ID="cbtnInfo" runat="server" BeginEndTag="li" />

                    <cc3:ClientSmartButton ID="cbtnSwfFileAdd2" runat="server" BeginEndTag="li" Visible="false" />                                                                                               
                    <cc3:ClientSmartButton ID="cbtnConfirm" runat="server" BeginEndTag="li" Class="btn btn-secondary" />
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>

         <div id="layout_content">
            <div class="row">
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left">
                  <table cellspacing="0" class="table_typeB">
		            <colgroup>
                        <col width="80px" />
                        <col width="450px" />
                        <col />
		            </colgroup>
		            <tbody>
                        <tr>
                            <th><%=GetResource("Title")%></th>
                            <td colspan="2"><cc3:ClientSmartTextBox ID="ctbName" InputMode="None" MaxLength="250" runat="server" Required="true" ScriptSubmitFunName="Mod.Add()" CssStyle="width:98% !important;"></cc3:ClientSmartTextBox></td>
                        </tr>
                        <tr>
                            <th>
                                <%=GetResource("Gubun")%>
                            </th>
                            <td colspan="2">
                                <select name="ddlCatHeader" id="ddlCatHeader" runat="server" class="searchselect form-control"></select>
                                <select name="ddlFontColor" id="ddlFontColor" runat="server" class="searchselect form-control"></select>
                                <select name="ddlFontSize" id="ddlFontSize" runat="server" class="searchselect form-control"></select>
                                <input id="cbxBold" type="checkbox" name="cbxBold" runat="server" /><label for="cbxBold"><%=GetResource("Bold") %></label>
                                <input id="cbOptionModify" type="checkbox" name="cbOptionModify" runat="server" /><label for="cbOptionModify"><%=GetResource("ModifyEveryone")%></label>
                                <input id="cbOptionTop" type="checkbox" name="cbOptionTop" runat="server" /><label for="cbOptionTop"><%=GetResource("RegisterOptionTop")%></label>
                                <select name="ddlDay" id="ddlDay" style="display: none" runat="server" class="searchselect form-control">
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
                                <asp:CheckBox ID="cbCheckDownLoad" runat ="server" />
                            </td>
                        </tr>
                        <tr id="tdDate" style="display:none">
                            <th><%=GetResource("PopUpSetting")%></th>
                            <td colspan="2">
                                <input id="cbPopUp" type="checkbox" name="cbPopUp" runat="server" visible="false" /><label id="lbPopUp" for="cbPopUp" class="lbText" visible="false" runat="server"><%=GetResource("UsePopup")%></label>
                                <input id="cbMainPopUp" type="checkbox" name="cbMainPopUp" runat="server" visible="false" disabled /><label id="lbMainPopUp" for="cbPopUp" class="lbText" visible="false" runat="server"><%=GetResource("UseMainPopup")%></label>
                                <span id="setDate" style="display:none">* <%=GetResource("SetDate")%></span> <input type="text" id="startDate" runat="server" style="display:none" size="12" class="clsDate searchtext form-control" readonly="readonly" /><span id="seperate"></span><input type="text" runat="server" id="endDate" readonly="readonly" style="display:none" size="12" class="clsDate searchtext form-control" />
                                <span id="MainPopOption" style="display:none">(
                                    WIDTH :  <input type="text" id="popupWidth" runat="server"  maxlength="4" style="width:30px;" /> px / HEIGHT : <input type="text" id="popupHeight" runat="server"  maxlength="4" style="width:30px;" /> px)
                                    <input id="Radio1" type="radio"  name="rdopixel" runat="server" value="400" style="margin-top: -1px; vertical-align: middle;"/>&nbsp;400*600 &nbsp; &nbsp; <input id="Radio2" type="radio" runat="server" name="rdopixel" value="520" style="margin-top: -1px; vertical-align: middle;"  />&nbsp;520*600 &nbsp; &nbsp; <input id="Radio3" type="radio" runat="server" name="rdopixel" value="800" style="margin-top: -1px; vertical-align: middle;" />&nbsp;800*600&nbsp;)<a class=btn id="cbtnView"><%=GetResource("Preview")%></a>
                                </span>
                            </td>
                        </tr>
                        <tr id="trAlarmUser">
                            <th class="last-child" rowspan="2"><%=GetResource("AlarmUser")%></th>
                            <td colspan="2">
                                <asp:HiddenField ID="hdAlarmUser" runat="Server"></asp:HiddenField>
                                <div id="InputAlarmUser" style="float:left;height: 27px; width: 85%; padding-left: 2px; padding-right:2px;padding-top: 2px;  text-overflow: ellipsis;display:table-cell;vertical-align:middle; overflow-y: auto; vertical-align:middle" contenteditable="false"></div>
                                <ul class="btn_right">                        
                                    <cc3:ClientSmartButton ID="cbtnWhole" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                    <cc3:ClientSmartButton ID="cbtnAlarmUser" runat="server" BeginEndTag="li"></cc3:ClientSmartButton>
                                </ul>
                            </td>                    
                        </tr>                
                        <tr id="trAlarmUserOption">
                            <td colspan="2">                     
                                <span id="spnAlarmUser">   
                                <input id="cbxAlarmUser" type="checkbox" name="cbxAlarmUser" runat="server"/><label for="cbxAlarmUser"><%=GetResource("List_SubGroup")%></label>            
                                </span>
                                <input id="cbxUseAlarm" type="checkbox" name="cbxUseAlarm" runat="server"/><label for="cbxUseAlarm"><%=GetResource("UseAlarm")%></label>                                        
                            </td>             
                        </tr>
                        <tr id="trAlarmUserOption2">
                            <th class="last-child" ><%=GetResource("AlarmUser")%></th>
                            <td colspan="2">                     
                                <input id="cbxUseAlarm2" type="checkbox" name="cbxUseAlarm" runat="server"/><label for="cbxUseAlarm2"><%=GetResource("UseAlarm")%></label>                                        
                            </td>             
                        </tr>
                    </tbody>
                    </table>
                  <div id="tabs" class="layout_subbox">
                    <ul id="tab" class="tab" style="display:none">
                        <li><a id="ActvieX"><span>ActiveX</span> </a></li>
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
                        </li>
                        <li><div id="placeholder_fileList" style="clear: both;"></div></li>
                        <li id="liLinkURLList"><ul id="placeholder_URLfileList" class='table_typeT ul_typeT'></ul></li>                        
                     </ul>
                     <ul>
                        <!-- 첨부파일 -->
                        <li class="title">
                              <%=GetResource("Additional")%> <%=GetResource("AttachName")%>
                              <div class="float-right">   
                                 <a href="#" id="cbtnSwfFileAdd" title="<%=GetResource("AddFile")%>"><i class="fas fa-plus-circle text-secondary"></i></a>
                                 <a href="#" id="cbtnLinkURL" title="<%=GetResource("URLAddBtn")%>"><i class="fas fa-link text-secondary"></i></a>      
                              </div>
                        </li>
                        <li><div id="placeholder_file" style="clear: both;"></div></li>
                        <li id="liLinkURL"><ul id="placeholder_URLfile" class='table_typeT ul_typeT'></ul></li>   
                     </ul>                  
              </div><!-- end col content-area-body-right -->
                    
			</div><!-- end row  -->
		</div><!-- end layout_content  -->
        
	 </div><!-- end layout_full  -->
    </div><!-- end layout_body  -->
    <!-- ================================== Hidden Input ===================================== -->
    <div style="display: none">
        <input type="hidden" id="contents" name="contents" value="<%: Contents %>" />
        <asp:TextBox ID="tbHiddenSelectedContent" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" TextMode="MultiLine" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Width="0" runat="server"></asp:TextBox>

        <asp:LinkButton ID="lbtnWrite" runat="server" OnClick="lbtnWrite_Click"></asp:LinkButton>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"></textarea>

        <!--  파일 첨부시 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>

        <asp:TextBox ID="hiddenLinkURLName" Style="display: none" Width="0"   runat="server"></asp:TextBox>
        <asp:TextBox ID="hiddenLinkURL" Style="display: none" Width="0"   runat="server"></asp:TextBox>
    </div>
    <!-- ==================================================================================== -->
    </form>
    <form name="PreviewForm" action="/DeskPlusEIP/Board/Content/Preview.aspx" method="post">
        <input type="hidden" name="MimeValue"/>
        <input type="hidden" name="Gubun"/>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script> 
    <script type="text/javascript">
        var Dcparm = "<%= Dcparm %>";
        var MenuCatID = "<%=MenuCatID %>";
        var CatID = "<%=CatID %>"; //트리 최상단 카테고리
        var CategoryID = "<%=CategoryID %>"; //선택한 트리 카테고리
        var ListID = "<%=ListID %>"; //리스트아이디
        var SearchGroup = "<%=SearchGroup %>"; //분류
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var Order = "<%=Order %>"; //sort    
        var OptionListType = "<%=OptionListType %>";
        var OptionAlarm = "<%=OptionAlarm %>";
        
        var ScreenShot = "<%=ScreenShot %>";
        var GoPage = "<%=GoPage %>";
        var OptionPopUp = "<%=OptionPopUp %>";  
        var MainPopUpCount = 0;
        var PGubun = "<%=PGubun%>";
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var R_Add_InputTitle = "<%= GetResource("Add_InputTitle") %>";
        var R_Add_SelectOptionTop = "<%= GetResource("Add_SelectOptionTop") %>";
        var R_Add_InputContent = "<%= GetResource("Add_InputContent") %>";
        var R_DelAttach = "<%= GetResource("DelAttach") %>";
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDown = "<%=GetResource("FileDownload") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_NotImageFormat = "<%=GetResource("NotImageFormat") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_ScreenShot = "<%=GetResource("ScreenShot") %>";
        var R_StartDateEndDate = "<%=GetResource("StartDateEndDate") %>";
        var R_ApplyRegDate = "<%=GetResource("ApplyRegDate") %>";
        var R_UsingMainPopup = "<%=GetResource("UsingMainPopup") %>";
        var R_WritePopupSize = "<%=GetResource("WritePopupSize") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID   
        var FileSizeLimit = "<%=AttachLimitSize %>"; //upload 사이즈 제한 kb
        var FileUploadLimit = 0; //업로드 개수
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var NonActiveX = "<%=NonActiveX%>";
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var PreLangReal = "<%=PreLangReal %>"; //ja

        var BoaComID = "<%=BoaComID %>";
        var AlarmUser = "<%=AlarmUser %>";
        var IsLowRank = "<%=IsLowRank %>";        
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/Mod.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Board/Js/Content/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/Board/Js/Content/Mod.js");
        //JsCall("/DeskPlusEIP/Board/Js/Content/Common.js");
    </script>
    <script type="text/javascript">
        setTimeout(function () {
            Mod.GetAttachFileList();
            Mod.GetSpoolFileList();
            Mod.GetLinkURLList();
        }, 200);
    </script>
</body>
</html>
