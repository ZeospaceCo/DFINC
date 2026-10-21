<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewTotal.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.ViewTotal" %>

<!DOCTYPE html>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/Func_RM.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/Func_RM.css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div class="layer_popup view">

        <div class="layout_full">
            <div class="popup_header">
                <h1><%=GetResource("ReserveResource") %></h1>

                <ul class="btn_right">
                    <cc3:ClientSmartButton ID="cbtnPrint" BeginEndTag="li" runat="server" />
                    <cc3:ClientSmartButton ID="cbtnList" BeginEndTag="li" runat="server" />
                </ul>
            </div>
            <div class="dotLine pb-0 mb-0"></div>
            <div id="layout_content">
                <div class="row">
                    <div class="col-12 col-lg-8 content-area-body-left" ><!-- width 12,9,8/12 -->
                        <div id="toPrint">
                            <!--기본정보-->
                            <table class="table_typeB">
                                <caption>
                                    View</caption>
                                <colgroup>
                                    <col width="80px" />
                                    <col width="350px" />
                                    <col width="80px" />
                                    <col />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th><%=GetResource("Subject") %></th>
                                        <td colspan="3"><asp:Label title="" ID="lbSubject" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("Period") %></th>
                                        <td><asp:Label title="" ID="lbDate" runat="server"></asp:Label></td>
                                        <th><%=GetResource("Subscriber")%></th>
                                        <td><asp:Label title="" ID="lbWriteName" runat="server"></asp:Label></td>                                        
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("ModifyDate")%></th>
                                        <td colspan="3"><asp:Label title="" ID="lbModifyDate" runat="server"></asp:Label></td>
                                        <!--<th><%=GetResource("Gubun")%></th>
                                        <td><span id="Pattern" runat="server"></span></td>-->
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("SecuritySetting")%></th>
                                        <td><asp:Label title="" ID="lbGuard" runat="server"></asp:Label></td>
                                        <th><%=GetResource("UsableStr")%></th>
                                        <td><asp:Label title="" ID="lbUsable" runat="server"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <th><%=GetResource("Repeat")%></th>
                                        <td colspan="3"><asp:Label title="" ID="lbRepeat" runat="server"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                            <!--기본정보-->
            
                            <!--본문정보-->
                            <div class="content_box"  style="overflow:auto;">
                            <table cellspacing="0" style="min-height:300px;width:100%;">
                                <tr>
                                    <td valign="top">
                                        <asp:Label title="" ID="lbContent" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <!--본문정보-->
                         </div>
                    </div><!-- end col content-area-body-left -->            
                    <div class="col-12 col-lg-4 content-area-body-right"  ><!-- width 12,3,4/12 -->
                        <ul>
                            <!-- 자원 선택 -->
                            <li class="title">
                                    <%=GetResource("ReserveResource")%>
                            </li>
                            <li>
                                <ul class="table_typeT ul_typeT">   
                                    <li>
                                        <span ID="spTitle" runat="server"></span>
                                    </li>
                                </ul>
                            </li>   
                            <!-- 자원 선택 -->
                          </ul>  
                     </div>
                 </div>
            </div>
        </div>

    </div>
    </form>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>        
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.print.js"></script>
    <script type="text/javascript">        
        var ReservationID = "<%=ReservationID%>";
        var ParentReservationID = "<%=ParentReservationID%>";       
        var R_FileName = "<%=GetResource("FileName") %>";
        var R_FileType = "<%=GetResource("FileType") %>";
        var R_FileLength = "<%=GetResource("FileLength") %>";
        var R_Download = "<%=GetResource("Download") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";                
        var R_Name = "<%=GetResource("Name") %>";
        var R_Contents = "<%=GetResource("Contents") %>";
        var R_WriteDate = "<%=GetResource("WriteDate") %>";
        var R_Mod  = "<%=GetResource("Mod") %>";
        var R_Del  = "<%=GetResource("Del") %>";    
        var FullDomainPath = "<%=FullDomainPath %>";    
        var R_AttendeeType1 = "<%=GetResource("AttendeeType1") %>"; //부서
        var R_AttendeeType2 = "<%=GetResource("AttendeeType2") %>"; //사용자
    </script>
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/ViewTotal.js"></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Content/ViewTotal.js");
    </script>
</body>
</html>

