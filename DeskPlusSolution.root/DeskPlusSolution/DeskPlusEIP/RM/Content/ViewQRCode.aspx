<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewQRCode.aspx.cs" Inherits="Zeospace.DeskPlus.RM.Web.Content.ViewQRCode" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8"><title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; minimum-scale=0.5; user-scalable=yes" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C/Css/Func_RM.css"/><link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Design/C1/Css/Func_RM.css"/>
</head>
<body>
    <form id="form1" runat="server">
<div class="layer_popup view">
    
   <div class="layout_full">
        <div class="popup_header">
            <h1><%=GetResource("ResourcesView") %></h1>
        </div>
        <div class="dotLine pb-0 mb-0"></div>
        <div id="layout_content">
             <div id="isExist" runat="server">
                <div class="row">
                    <div class="col-12 col-lg-8 content-area-body-left" ><!-- width 12,9,8/12 -->
          
                        <!-- 자원 예약 내용 -->
                        <table class="table_typeB">
                            <caption></caption>
                            <colgroup>
                                <col width="100px" />
                                <col />
                                <col width="45px" />
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th><%=GetResource("Image")%></th>
                                    <td colspan="3">
                                        <div class="imageSlider">  
                                            <div class="left" id="imgLeft" runat="server"><a href="#"><img src="/DeskPlusEIP/Design/C/Img/Main/ptl_laytype3_left.gif" alt="" title="" /></a></div>
                                            <div class="imageList">    
                                                 <div id="phImage" style="clear: both;" runat="server"></div>                    
                                            </div>
                                            <div class="right" id="imgRight" runat="server"><a href="#"><img src="/DeskPlusEIP/Design/C/Img/Main/ptl_laytype3_right.gif" alt="" title="" /></a></div>                                
                                        </div>                            
                                    </td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("ResourcesTitle")%></th>
                                    <td colspan="3"><asp:Literal ID="ltName" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th><%=GetResource("Code")%></th>
                                    <td><asp:Literal ID="ltCode" runat="server"></asp:Literal></td>
                                    <th><%=GetResource("Qty")%></th>
                                    <td><asp:Literal ID="ltReCount" runat="server"></asp:Literal></td>   
                            
                                </tr>
                                <tr>
                                    <th><%=GetResource("AuthorityNeed")%></th>
                                    <td  colspan="3"><asp:Literal ID="ltAuth" runat="server"></asp:Literal></td>                          
                                </tr>
                                <tr>
                                    <th><%=GetResource("Place")%></th>
                                    <td colspan="3"><asp:Literal ID="ltPlace" runat="server"></asp:Literal></td>
                                </tr>
                                <tr>
                                    <th style="min-height:100px;"><%=GetResource("Content")%></th>
                                    <td colspan="3"><asp:Literal ID="ltDesc" runat="server"></asp:Literal></td>
                                </tr>
                            </tbody>
                        </table>
                        <!-- 자원 예약 내용 -->
                      
                    </div><!-- end col content-area-body-left -->            
                    <div class="col-12 col-lg-4 content-area-body-right"  ><!-- width 12,3,4/12 -->
                       <ul>
                            <!-- 자원 담당자 리스트 -->
                            <li class="title">
                                 <%=GetResource("ResourcesUser")%>
                            </li>

                            <li>
                                <ul class="table_typeT ul_typeT">   
                                    <span ID="spTitle" runat="server"></span>
                                </ul>
                            </li>    
                       </ul>
                    </div>
                </div>               
             </div>
             <div id="noExist" style="min-height:300px;font-size:14px; font-weight:bold;" runat="server">
               <img src="/DeskPlusEIP/Design/C/Img/Icon/ico_sysError.gif" align="absmiddle"/><%=GetResource("NoQRCode") %>
             </div>            
        </div>
   </div>
 </div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.rolling.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js" ></script>    
    <script type="text/javascript" src="/DeskPlusEIP/RM/Js/Content/ViewQRCode.js" ></script>
    <script type="text/javascript">
//        JsCall("/DeskPlusEIP/RM/Js/Content/ViewQRCode.js");
    </script>
</body>
</html>
