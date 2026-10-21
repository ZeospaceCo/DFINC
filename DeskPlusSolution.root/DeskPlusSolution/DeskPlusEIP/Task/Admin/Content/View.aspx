<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="View.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Admin.Content.View" %>
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
                    <cc3:ClientSmartButton ID="cbtnDel" runat="server" BeginEndTag="li" class="btn btn-danger" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />                    
                </ul>
            </div>
            <!-- END HEADER -->
            <div id="layout_content">
            <div class="row">
            <div class="col-12 content-area-body"><!-- width 100%(col-12) -->            
                <div class="conmain">
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

                <!-- FOOTER -->
                <div class="footerbtn" style="display:none;">
                    <ul class="btn_left">
                        <cc3:ClientSmartButton ID="cbtnfileBackUp_F" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnReadList_F" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnForwardList_F" runat="server" BeginEndTag="li" />
                        <cc3:ClientSmartButton ID="cbtnPrint_F" runat="server" BeginEndTag="li" />
                    </ul>
                    <ul class="btn_right">
                        <cc3:ClientSmartButton ID="cbtnDel_F" runat="server" BeginEndTag="li" BeginEndTagClass="btn_red" />
                        <cc3:ClientSmartButton ID="cbtnList_F" runat="server" BeginEndTag="li" />                
                    </ul>
                </div>
                <!-- FOOTER -->  
            </div>
            </div>
            </div>
        </div>
    </div>
    <div style="display: none;">
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
        var IsGubun = "<%=IsGubun %>";
        var ListCount = "<%=ListCount %>"; //보여줄게시물수        
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var Status = "<%=Status %>";
        var IsType = "<%=IsType %>";
        var IsBase = "<%=IsBase %>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var FullDomainPath = "<%=FullDomainPath %>";
        var R_ApplyDelete = "<%= GetResource("ApplyDelete") %>";       
        var R_StandBy = "<%=GetResource("StandBy") %>"; //대기
        var R_ProgressStr ="<%=GetResource("ProgressStr") %>"; //진행
        var R_Complete ="<%=GetResource("Complete") %>"; //완료
        var R_Delay = "<%=GetResource("Delay") %>"; //지연         
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Admin/Content/View.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Admin/Content/View.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
    </script>
</body>
</html>

