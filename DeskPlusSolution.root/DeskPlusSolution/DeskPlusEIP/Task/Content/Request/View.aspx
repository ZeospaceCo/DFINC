<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Content.Request.View" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"> 
    <title>
        <%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Task.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Task.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" />
    <link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body> 
    <form id="form1" runat="server">
    <div id="layout_body">
        <div class="layout_full">
            <!-- START HEADER -->
            <div id="layout_header">
                <h1><%=GetResource("Detail")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnfileBackUp" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnReadList" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnForwardList" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnForward" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnOpen" runat="server" BeginEndTag="li" class="btn btn-secondary" />    
                    <cc3:ClientSmartButton ID="cbtnCompCancel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnComplete" runat="server" BeginEndTag="li" class="btn btn-secondary "  />                    
                    <cc3:ClientSmartButton ID="cbtnReCall" runat="server" BeginEndTag="li" class="btn btn-secondary "  />                    
                    <cc3:ClientSmartButton ID="cbtnMod" runat="server" BeginEndTag="li" class="btn btn-secondary "  />                    
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                    
                </ul>
            </div>
            <!-- END HEADER -->           
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->                
                    <div class="conmain">
                        <!-- a href="#" onclick="Common.StatusInfo()" class="btn btn-sm btn-light text-secondary float-left pl-2 pr-2" id="imgStatusInfo" ><i class="fas fa-question  mr-1"></i></a--> 
                        <div class="help">
                            <span style="border-color:#75ace9;"><%=GetResource("StandBy") %></span>
                            <span style="border-color:#8bc63e;"><%=GetResource("ProgressStr") %></span>
                            <span style="border-color:#343a40;"><%=GetResource("Complete") %></span>
                            <span style="border-color:#9e9080;"><%=GetResource("Delay") %></span>
                        </div>
                        <div class="etcbtn">
                            <a href="#" class="btn float-left" id="cbtnDetailOpen" style="display:none"><i class="fas fa-plus mr-1"></i><%=GetResource("DetailOpen")%></a>
                            <a href="#" class="btn float-left" id="cbtnDetailClose" ><i class="fas fa-minus mr-1"></i><%=GetResource("DetailClose")%></a>                
                            <a href="#" class="btn float-left" id="cbtnDown"><i class="fas fa-chevron-down mr-1"></i><%=GetResource("DownView")%></a>                                            
						</div>
                    </div> 
                    <div id="toPrint">
                    <!-- START - REQUEST TEMPATE 1 -->
                    <table cellspacing="0" class="tab_typeCR">
                        <colgroup>
                            <col width="10%" />
                            <col width="40%"/>
                            <col width="10%" />
                            <col width="40%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th><%=GetResource("Title")%></th>
                                <td colspan="3"><span class="brd_tb"><asp:Literal ID="ltlSubject" runat="server"></asp:Literal></span></td>
                            </tr>    
                            <tr>
                                <th><%=GetResource("Requester")%></th>
                                <td><asp:Literal ID="ltlRequester" runat="server"></asp:Literal></td>
                                <th><%=GetResource("State")%></th>
                                <td><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
                            </tr>                          
                            <tr>
                                <th><%=GetResource("Receiver")%></th>
                                <td><asp:Literal ID="ltlReceive" runat="server"></asp:Literal></td>
                                <th><%=GetResource("Referencer")%></th>
                                <td><asp:Literal ID="ltlReference" runat="server"></asp:Literal></td>
                            </tr>     
                            <tr class="detail">
                                <th><%=GetResource("RequestDate")%></th>
                                <td><asp:Literal ID="ltlRequestDate" runat="server"></asp:Literal></td>
                                <th><%=GetResource("CompleteDate")%></th>
                                <td><asp:Literal ID="ltlCompleteRequestDate" runat="server"></asp:Literal></td>
                            </tr>     
                            <tr class="detail">
                                <th><asp:Literal ID="ltlHeaderStartDate" runat="server"></asp:Literal></th>
                                <td style="height:20px"><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></td>
                                <th><asp:Literal ID="ltlHeaderEndDate" runat="server"></asp:Literal></th>
                                <td><asp:Literal ID="ltlEndDate" runat="server"></asp:Literal></td>
                            </tr>         
                            <tr class="detail">
                                <th><%=GetResource("Group")%></th>
                                <td><asp:Literal ID="ltlItem" runat="server"></asp:Literal> -> <asp:Literal ID="ltlItemSub" runat="server"></asp:Literal></td>
                                <th><%=GetResource("IsClose")%></th>
                                <td><asp:Literal ID="ltlIsClose" runat="server"></asp:Literal></td>
                            </tr>     
                            <tr class="detail">
                                <th><%=GetResource("IsGubun")%></th>
                                <td><asp:Literal ID="ltlIsGubun" runat="server"></asp:Literal></td>
                                <th><%=GetResource("IsSource")%></th>
                                <td><asp:Literal ID="ltlIsSource" runat="server"></asp:Literal></td>                                
                            </tr>             
                            <tr class="detail">
                                <th><%=GetResource("DocNumber")%></th>
                                <td><asp:Literal ID="ltlTaskNumber" runat="server"></asp:Literal></td>
                                <th><%=GetResource("History")%></th>
                                <td><asp:Literal ID="ltlHistroy" runat="server"></asp:Literal></td>
                            </tr>  
                            <tr class="detailEtc" <%=IsBase %>>
                                <th><%=GetResource("AttachName")%></th>
                                <td><asp:Literal ID="ltlAttach" runat="server"></asp:Literal></td>
                                <th><%=GetResource("Base")%></th>
                                <td><asp:Literal ID="ltlBase" runat="server"></asp:Literal></td>
                            </tr>         
                            <tr>
                                <td colspan="4" valign='top'>
                                <div style="overflow:auto;">
                                    <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                                </div>             
                                </td>
                            </tr>                
                        </tbody>
                    </table>
                    <!-- END - HEADER REQUEST 1 -->
                                  
                    <asp:Panel ID="pnContent" runat="server" Width="100%" Height="100%"></asp:Panel>
                    </div><!-- end toPrint -->   
                    <div class="pb-5 mb-1" id="btn_Answer_Area"></div>                        
 
                    <!-- 답변하기 글쓰기 영역 -->
                    <div id="Answer_Area" style="display:none">        
                    <!-- 작성페이지 -->
                    <div class="layout_subbox p-1">
                        <h1><%=GetResource("Reply")%></h1>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnReply" runat="server" BeginEndTag="li"  class="btn btn-secondary" />                
                            <cc3:ClientSmartButton ID="cbtnClose" runat="server" BeginEndTag="li" />
                        </ul>
                    </div>
                    <div id="tabs" class="layout_subbox write pt-0">
                        <ul id="tab" class="tab">
                            <li><a id="ActvieX" class="tab_on"><span>ActiveX</span> </a></li>
                            <li><a id="NonActiveX"><span>NonActiveX</span> </a></li>
                        </ul>
                        <div class="divEditor">
                            <script type="text/javascript" src="/DeskPlusEIP/Html/Js/IEActiveX/<%=PreLang%>/activexTagfree.js"></script>
                        </div>
                    </div>
                    <ul>
                        <!-- 첨부파일 -->
                        <li class="attchT">
                            <%=GetResource("AttachName")%>
                            <div class="float-right"> <a href="#" id="cbtnSwfFileAdd"><i class="fas fa-plus-circle text-secondary" title="<%=GetResource("AddFile")%>" style="font-size: 1.25em;"></i></a></div>
                        </li>
                        <!-- 첨부파일 리스트 -->            
                        <ul class="itemlist"> <li id="placeholder_file" style="border-bottom: 1px solid rgba(0,0,0,0.1);padding:0px;"></li> </ul>
            
                        <!-- 근거문서 -->
                        <li class="attchT">
                            <%=GetResource("BaseDoc")%>
                               <div class="float-right"> <a href="#" id="cbtnAddApp"><i class="fas fa-plus-circle text-secondary"  title="<%=GetResource("AddDoc")%>" style="font-size: 1.25em;"></i></a> </div>
                        </li>
                        <li><ul class="itemlist" id="contentTable" style="border-bottom: 1px solid rgba(0,0,0,0.1);min-height:21px;"></ul></li>
                    </ul>

                    <!-- 업무협업 근거문서 리스트 -->
                    <div style="display:none">
                    <div class="layout_subbox">
                        <h2><%=GetResource("BaseTask")%></h2>
                        <ul class="btn_right">
                            <li><cc3:ClientSmartButton ID="cbtnAddTask" runat="server"></cc3:ClientSmartButton></li>
                        </ul>
                    </div>               
            
                    <table class="table_typeT" id="contentTaskTable">
                        <caption><%=GetResource("BaseTask")%></caption>
                        <colgroup>                        
                            <col width="250px" />
                            <col />
                            <col width="60px" />
                            <col width="50px" />
                            <col width="60px" />
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
                    </div>

                    <!-- FOOTER -->
                    <div class="footerbtn"  style="display:none;">
                        <ul class="btn_left">
                            <cc3:ClientSmartButton ID="cbtnfileBackUp_F" runat="server" BeginEndTag="li" />
                            <cc3:ClientSmartButton ID="cbtnReadList_F" runat="server" BeginEndTag="li" />
                            <cc3:ClientSmartButton ID="cbtnForwardList_F" runat="server" BeginEndTag="li" />
                            <cc3:ClientSmartButton ID="cbtnPrint_F" runat="server" BeginEndTag="li" />
                        </ul>
                        <ul class="btn_right">
                            <cc3:ClientSmartButton ID="cbtnOpen_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />    
                            <cc3:ClientSmartButton ID="cbtnCompCancel_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_red" />
                            <cc3:ClientSmartButton ID="cbtnComplete_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />
                            <cc3:ClientSmartButton ID="cbtnForward_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />
                            <cc3:ClientSmartButton ID="cbtnReCall_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />                    
                            <cc3:ClientSmartButton ID="cbtnMod_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_blue" />    
                            <cc3:ClientSmartButton ID="cbtnDel_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_red" />
                            <cc3:ClientSmartButton ID="cbtnList_F" runat="server" BeginEndTag="li" />                
                        </ul>
                    </div> 
             </div><!-- end col content-area-body-right -->
             </div><!-- end row  -->
             </div><!-- end layout_content  -->
    
        </div><!-- end layout_full  -->
   </div> <!-- end layout_body  -->
    <div style="display: none;">
        <!-- 에디터에 필요한 필드 //-->
        <asp:TextBox ID="tbHiddenSelectedContent" Style="display: none" runat="server" Width="0px"></asp:TextBox>
        <asp:TextBox ID="tbHiddenMime_contents" Style="display: none" Width="0" TextMode="MultiLine"  runat="server"></asp:TextBox>
        <asp:TextBox ID="tbHiddenText_contents" Style="display: none" Width="0" runat="server"></asp:TextBox>
        <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server"   style="display: none" rows="1" cols="20"></textarea>
        <asp:TextBox ID="tbSelItemSubID" Width="0" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbBaseApps" runat="server"></asp:TextBox>
        <asp:TextBox ID="tbBaseTasks" runat="server"></asp:TextBox>
        <input name="tbAddAppID" type="text" id="tbAddAppID" value="" size="160" runat="server" />
        <input name="tbAddTaskID" type="text" id="tbAddTaskID" value="" size="160" runat="server" />
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton>
        <iframe id="ifrm" name="ifrm" src="about:blank"></iframe>
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/
        var CurrentPage = "<%=CurrentPage %>"; //현재페이지
        var SearchStatus = "<%=SearchStatus %>"; //상태값     
        var SearchDateValue = "<%=SearchDateValue %>"; //검색날짜값        
        var SearchField = "<%=SearchField %>"; //검색키워드
        var SearchValue = "<%=SearchValue %>"; //검색어      
        var ItemID = "<%=ItemID %>"; //대분류
        var ItemSubID = "<%=ItemSubID %>"; //소분류
        var IsType  = "<%=IsType %>";
        var ReLoad = "<%=ReLoad%>";
        var IsGubun = "<%=IsGubun %>";
        var ListCount = "<%=ListCount %>"; //보여줄게시물수        
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var Status = "<%=Status %>";
        var ImpoUserYN = "<%=ImpoUserYN %>";
        var IsBase = "<%=IsBase %>";
        //전자결재 근거문서용  
        var BaseDocIDs = "<%=BaseDocIDs %>";        
        var BaseDocCnt = 0; //전제결재 근거문서 새창에서 사용함.
        //업무협업 근거문서용
        var BaseTasks = "";
        var BaseTaskCnt = 0; //근거문서 새창에서 사용함.
        var BtnCheck = "N";     
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_Del = "<%= GetResource("Del") %>";
        var R_FileName = "<%= GetResource("FileName") %>";
        var R_FileType = "<%= GetResource("FileType") %>";
        var R_FileLen = "<%= GetResource("FileLen") %>";
        var R_FileDel = "<%= GetResource("Del") %>";
        var R_NoAttachData = "<%= GetResource("NoAttachData") %>";
        var R_NoContent = "<%= GetResource("NoContent") %>";
        var R_NoBaseListData = "<%= GetResource("NoBaseListData") %>";
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";
        var R_DelReply = "<%= GetResource("DelReply") %>";
        var R_InputTitle = "<%= GetResource("InputTitle") %>";        
        var R_DoubtStcipt = "<%=GetResource("DoubtStcipt") %>";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";
        var R_MSG_FAIL = "<%=GetResource("MSG_FAIL") %>";
        var ConfirmEditorTab = "<%=GetResource("ConfirmEditorTab") %>";
        var R_MsgProcessing  = "<%=GetResource("MsgProcessing") %>";  
        var R_MsgCompCancel  = "<%=GetResource("MsgCompCancel") %>";  
        var SwfUploadGUID = "<%=SwfUploadGUID %>"; //flash upload GUID           
        var FileType = "*.*";
        var FileTypeDesc = "All Files (*.*)";
        var ImagePath = "<%=BaseImgPath %>";
        var NonActiveX = "<%=NonActiveX%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_StandBy = "<%=GetResource("StandBy") %>"; //대기
        var R_ProgressStr ="<%=GetResource("ProgressStr") %>"; //진행
        var R_Complete ="<%=GetResource("Complete") %>"; //완료
        var R_Delay = "<%=GetResource("Delay") %>"; //지연
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLang%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Content/Request/View.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLang + ".js");
        //JsCall("/DeskPlusEIP/Task/Js/Content/Request/View.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
    </script>
</body>
</html>

