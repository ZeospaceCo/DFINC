<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComplete.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Common.AddComplete" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="layout_body" class="write">
        <div class="layout_full">
            <div id="layout_header">
                <!-- 페이지타이틀 //-->
                <h1><%=GetResource("Completes")%></h1>
                <!-- 버튼 //-->
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnAdd" runat="server" BeginEndTag="li" class="btn btn-secondary"/>                                        
                    <cc3:ClientSmartButton ID="cbtnCancel" runat="server" BeginEndTag="li" />
                </ul>
            </div>
            <div id="layout_content">
             <div class="row"> 
                <div class="col-12 col-lg-9 col-xl-8 content-area-body-left"><!-- width 100%(col-12) -->
                    <div class="layout_Tbox">
                    <table cellspacing="0" class="table_typeB">
                        <colgroup>
                            <col width="80px" />
                            <col />
                            <col width="80px" />
                            <col />
                        </colgroup>
                        <tbody>      
                            <tr id="tdDate">
                                <th><%=GetResource("Start")%></th>
                                <td><cc3:ClientSmartTextBox ID="ctbStartDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox></td>
                                <th><%=GetResource("End")%></th>
                                <td><cc3:ClientSmartTextBox ID="ctbEndDate" ReadOnly InputMode="None" MaxLength="10" runat="server" Required="true" CssStyle="width:100px !important;"></cc3:ClientSmartTextBox></td>
                            </tr>                        
                        </tbody>
                    </table>
                    </div>
                    <div id="tabs" class="layout_subbox">
                        <ul id="tab" class="tab" style="margin-left:0px">
                            <li><a id="ActvieX" class="tab_on"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divEditor">
                            <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                        </div>
                    </div>
                </div><!-- end col content-area-body-right -->
                <div class="col-12 col-lg-3 col-xl-4 content-area-body-right"  >
                    <ul>
                    <!-- 첨부파일 -->
                      <li class="title">
                        <%=GetResource("AttachName")%>                    
                        <a href="#" id="cbtnSwfFileAdd"><i class="fas fa-plus-circle text-secondary" title="<%=GetResource("AddFile")%>"></i></a>                   
                      </li>
                                      
                      <li><div id="placeholder_file" style="clear: both;"></div></li>

                     <!-- 근거문서 -->
                     <li class="title">
                        <%=GetResource("BaseDoc")%>                   
                        <a href="#" id="cbtnAddApp"><i class="fas fa-plus-circle text-secondary"  title="<%=GetResource("AddDoc")%>"></i></a>                   
                     </li>
                     <li>
                        <ul class="itemlist" id="contentTable">
                            <li id="noContent"></li>
                        </ul>
                     </li>
                  </ul>
                
                <!-- 업무협업 근거문서 리스트 -->
                <div style="display:none">
                <div class="layout_subbox">
                    <h2><%=GetResource("BaseTask")%></h2>
                    <ul class="btn_right">
                        <li><cc3:ClientSmartButton ID="cbtnAddTask" runat="server"></cc3:ClientSmartButton></li>
                    </ul>
                </div>                
                
                <table class="table_typeA" id="contentTaskTable">
                    <caption><%=GetResource("BaseTask")%></caption>
                    <colgroup>                        
                        <col width="200px" />
                        <col />
                        <col width="60px" />
                        <col width="60px" />
                        <col width="70px" />
                    </colgroup>
                    <thead>
                        <tr>
                            <th class="center"><%=GetResource("DocNumber")%></th>
                            <th class="center"><%=GetResource("Title")%></th>
                            <th class="center"><%=GetResource("DocView")%></th>
                            <th class="center"><%=GetResource("Del")%></th>
                            <th class="center"><%=GetResource("ChangeOrder")%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr id ="noTaskContent">
                            <td colspan="5" class="center"><span class='NoBaseTaskListData' /></td>
                        </tr>
                    </tbody>
                </table>
                
                </div>
                <!-- 업무협업 근거문서 리스트 -->
               </div><!-- end col  -->    
             </div><!-- end row  -->    
           </div><!-- end layout_content  -->
		</div><!-- end layout_full  -->
	</div><!-- end layout_body  -->
    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->    
        <input type="hidden" id="hdContent" name="contents" value="<%: Contents %>" />    
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"  runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>        
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbSelItemSubID" Width="0" runat="server"></asp:TextBox>
        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"></asp:LinkButton>        
        <asp:TextBox ID="tbBaseApps" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbBaseTasks" runat="server"></asp:TextBox>
        <input name="tbAddAppID" type="text" id="tbAddAppID" value="" size="160" runat="server" />
        <input name="tbAddTaskID" type="text" id="tbAddTaskID" value="" size="160" runat="server" />

        <!--  파일 첨부시 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSpoolPath" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenSaveFileList" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbScreenShot" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <iframe id="ifrmDelFile" style="display: none" name="ifrmDelFile" src="about:blank" frameborder="0"></iframe>
        <!-- ==================================================================================== -->
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
    <script type='text/javascript' src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
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
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchStatus = "<%=SearchStatus %>"; //상태값     
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어          
        var ListCount = "<%=ListCount %>"; //보여줄게시물수        
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var ItemID = "<%=ItemID %>"; //대분류
        var ItemSubID = "<%=ItemSubID %>"; //소분류
        var IsType = "<%=IsType %>";
        var ImpoUserYN = "<%=ImpoUserYN %>";
        var IsGubun = "<%=IsGubun %>";
        var IsPage = "<%=IsPage %>";   
        var RecentPageNo = "<%=RecentPageNo%>";  
        var ProcessPageNo = "<%=ProcessPageNo%>";
        var DelayPageNo = "<%=DelayPageNo%>";        
        var ComPageNo = "<%=ComPageNo%>";
        var CCPageNo = "<%=CCPageNo%>";
        var WriteCommentPageNo  = "<%=WriteCommentPageNo%>";
        var ImpoPageNo = "<%=ImpoPageNo%>";
        var Year = "<%=Year%>";     
        //전자결재 근거문서용  
        var BaseDocIDs = "<%=BaseDocIDs %>";        
        var BaseDocCnt = 0; //전제결재 근거문서 새창에서 사용함.
        //업무협업 근거문서용
        var BaseTasks = "";
        var BaseTaskCnt = 0; //근거문서 새창에서 사용함.
        var BtnCheck = "N";   
        var R_ApplyDelete ="<%=GetResource("ApplyDelete") %>";
        var R_AleadyRegisted = "<%=GetResource("AleadyRegisted") %>";
        var R_Del = "<%=GetResource("Del") %>";
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLen = "<%=GetResource("FileLen") %>";
        var R_FileDel = "<%=GetResource("Del") %>";
        var R_NoAttachData = "<%=GetResource("NoAttachData") %>";
        var R_NoBaseListData = "<%=GetResource("NoBaseListData") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var R_MovePage = "<%=GetResource("MovePage") %>";
        var R_SelectFirst = "<%=GetResource("SelectFirst") %>";
        var R_SelectSecond = "<%=GetResource("SelectSecond") %>";
        var R_InputStartDate = "<%= GetResource("InputStartDate") %>";
        var R_InputEndDate = "<%= GetResource("InputEndDate") %>";
        var R_StartDateEndDate = "<%= GetResource("StartDateEndDate") %>";   
        var R_InputReceive = "<%=GetResource("InputReceive") %>";
        var R_InputTitle = "<%=GetResource("InputTitle") %>";
        var R_Title = "<%=GetResource("Title")%>";
        var R_State = "<%=GetResource("State")%>";
        var R_FirtstGroup = "<%=GetResource("First")%>"+"<%=GetResource("Group")%>";
        var R_SecondGroup = "<%=GetResource("Second")%>"+"<%=GetResource("Group")%>";
        var R_RequestDate = "<%=GetResource("RequestDate")%>";
        var R_StartDate = "<%=GetResource("StartDate")%>";
        var R_EndDate = "<%=GetResource("EndDate")%>"; 
        var R_Delete = "<%=GetResource("Delete")%>";
        var R_NoContent= "<%=GetResource("NoContent") %>";
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_MsgProcessing  = "<%=GetResource("MsgProcessing") %>";
        var R_DocNumber  = "<%=GetResource("DocNumber") %>";
        var R_SubCategory = "<%=GetResource("SubCategory") %>";
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal %>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/AddComplete.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/AddComplete.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
    </script>
</body>
</html>
