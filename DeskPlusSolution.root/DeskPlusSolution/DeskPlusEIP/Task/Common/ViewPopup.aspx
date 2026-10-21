<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPopup.aspx.cs" Inherits="Zeospace.DeskPlus.Task.Web.Common.ViewPopup" %>
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
            <div id="layout_header">
                <h1><%=GetResource("Detail")%></h1>
                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" runat="server" BeginEndTag="li" />
                    <cc3:ClientSmartButton ID="cbtnList" runat="server" BeginEndTag="li" />
                   <li><a href="javascript:self.close()" title="<%=GetResource("Close")%>"  class="btn"><span><%=GetResource("Close")%></span></a></li>                   
                </ul>
            </div>
           <div id="layout_content">
           <div class="row">
           <div class="col-12 content-area-body"><!-- width 100%(col-12) -->            
                <div class="conmain">
                    <!-- START - REQUEST TEMPATE 1 -->
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
                 </div> 
                
           </div>
           </div>

           <!-- FOOTER -->
           <div class="footerbtn" style="display:none;">
                <ul class="btn_right">                                    
                    <cc3:ClientSmartButton ID="cbtnPrint_F" runat="server" BeginEndTag="li" />                
                    <cc3:ClientSmartButton ID="cbtnList_F" runat="server" BeginEndTag="li" />       
                    <li><a href="javascript:self.close()" title=""><span><%=GetResource("Close")%></span></a></li>         
                </ul>
            </div>
           <!-- FOOTER -->  
           </div>
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
        var ListID = "<%=ListID %>";
        var GroupID = "<%=GroupID %>";
        var IsBase = "<%=IsBase %>";
        var ImagePath = "<%=BaseImgPath %>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja 
        var ClientQueryString = "<%=Page.ClientQueryString %>";
        var FullDomainPath = "<%=FullDomainPath %>";
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/ViewPopup.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Task/Js/Common/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Task/Js/Common/ViewPopup.js");
        //JsCall("/DeskPlusEIP/Task/Js/Common/Common.js");
    </script>
</body>
</html>

