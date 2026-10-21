<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="Zeospace.DeskPlusMobile.Web.Config.List" %>

<%@ Register Src="../Portal/Menu.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="../Portal/Footer.ascx" TagName="Footer" TagPrefix="uc2" %>
<!DOCTYPE html>
<html lang="ko">
<head>
    <title></title>
    <meta charset="uft8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0, user-scalable=no, target-densitydpi=medium-dpi" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/base/Css/DStyle/all.css" />
    <link rel="stylesheet" type="text/css" href="/DeskPlusMobile/Content/ko/Base/Css/<%=SkinType%>/navimobile.css"/>
</head>
<body class="mobile navihide scrolldown">
    <form id="form1" runat="server">
    <div id="divprg" align="center" class="loading">
    </div>
    <div data-role="page" id="PimsList">
        <uc1:Menu ID="Menu1" runat="server" />
        <div class="bodycontainer">
            <div class="introcontentblock bottomcontent">
                <ul class="mobliecontent ">
                    <li class="title text-primary">
                        <span id="listTitle">
                            <i class="fas fa-cog dropbtn "></i><%=GetResource("Config")%>
                        </span>
                        <div class="float-right date text-dark"><%=Today%>&nbsp;&nbsp;</div>
                    </li>
                    <li class="contentblock msgNotice" id="msgNotice" style="display:none;" onclick="NoticeHide();">
                    </li>
                    <li class="contentblock">
                        <div class="container-fluid buttongroup"><!-- 버튼 그룹 -->
                            <div class="float-left">
                                <button type="button" class="btn btn-lg btn-primary" id="btsave">
                                    <span class="btnb"><%=GetResource("Save")%></span>
                                </button>
                            </div>
                        </div>
                    </li>
                </ul>
                <ul class="mobliecontent ">
                    <li class="contentblock">
                        <div class="container-fluid">
                            <div id="divPims" runat="server">
                                <ul class="mli">
                                    <li>
                                        <span class="mct">
                                            <span class="lif">
                                                <%=GetResource("PimsSetting")%>
                                            </span>
                                        </span>
                                        <span style="display:block;">
                                            <input type="radio" id="rdPims1" name="rdPims" class="rd" /><label for=""><%=GetResource("PimsSetting1")%></label>
                                            <br />
                                            <input type="radio" id="rdPims2" name="rdPims" class="rd" /><label for=""><%=GetResource("PimsSetting2")%></label>
                                        </span>
                                    </li>
                                    <li id="liCbPims2" style="display: none">
                                        <div class="title">
                                            <span class="mct">
                                                <span class="istSt">
                                                    <%=GetResource("PimsSetting3")%>
                                                    <input type="radio" id="rdPimsUser1" name="rdPimsUser" class="rd" /><label for="">ON</label>
                                                    <input type="radio" id="rdPimsUser2" name="rdPimsUser" class="rd" /><label for="">OFF</label>
                                                </span>
                                            </span>
                                            <span class="mct">
                                                <span class="istSt">
                                                    <%=GetResource("PimsSetting4")%>
                                                    <input type="radio" id="rdPimsDept1" name="rdPimsDept" class="rd" /><label for="">ON</label>
                                                    <input type="radio" id="rdPimsDept2" name="rdPimsDept" class="rd" /><label for="">OFF</label>
                                                </span>
                                            </span>
                                            <span class="mct">
                                                <span class="istSt">
                                                    <%=GetResource("PimsSetting5")%>
                                                    <input type="radio" id="rdPimsShare1" name="rdPimsShare" class="rd" /><label for="">ON</label>
                                                    <input type="radio" id="rdPimsShare2" name="rdPimsShare" class="rd" /><label for="">OFF</label>
                                                </span>
                                            </span>
                                            <span class="mct">
                                                <span class="istSt">
                                                    <%=GetResource("PimsSetting6")%>
                                                    <input type="radio" id="rdPimsAll1" name="rdPimsAll" class="rd" /><label for="">ON</label>
                                                    <input type="radio" id="rdPimsAll2" name="rdPimsAll" class="rd" /><label for="">OFF</label>
                                                </span>
                                            </span>
                                        </div>
                                        
                                    </li>
                                </ul>
                            </div>
                            <div id="divSkin" runat="server">
                                <ul class="mli">
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("MobileDesign")%></span>
                                            <br />
                                            <br />
                                            <ul class="colorbox">
                                                <li class="colorchip">
                                                    <span style="background: #3d6efc;">
                                                        <a href="#" onclick="List.SetSkin('C1');"></a>
                                                    </span>
                                                    <span style="background: #722ef5;">
                                                        <a href="#" onclick="List.SetSkin('C2');"></a>
                                                    </span>
                                                    <span style="background: #429e74;">
                                                        <a href="#" onclick="List.SetSkin('C3');"></a>
                                                    </span>
                                                    <span style="background: #9e4268;">
                                                        <a href="#" onclick="List.SetSkin('C4');"></a>
                                                    </span>
                                                    <span style="background: #6a9e42">
                                                        <a href="#" onclick="List.SetSkin('C5');"></a>
                                                    </span>
                                                    <span style="background: #9e5342">
                                                        <a href="#" onclick="List.SetSkin('C6');"></a>
                                                    </span>
                                                </li>
                                                <li class="colordir">
                                                    <img src="/DeskPlusMobile/Content/ko/Base/Img/bu_lk.gif" />
                                                </li>
                                                <li class="colorsample">
                                                    <img id="ColorSample" src="/DeskPlusMobile/Content/ko/Base/Img/ColorSample/<%=SkinType%>.png" />
                                                </li>
                                            </ul>
                                        </span>
                                    </li>
                                </ul>
                            </div>
                            <div id="divApprove" runat="server">
                                <ul class="mli">
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("SettingApprovePwd")%> </span>
                                            <br />
                                            <input type="radio" id="rblUsePwd_0" name="rblUsePwd" class="rd" /><label for=""><%=GetResource("IsActiveTitN")%></label>
                                            <input type="radio" id="rblUsePwd_1" name="rblUsePwd" class="rd" /><label for=""><%=GetResource("IsActiveTitY")%></label>
                                        </span>
                                    </li>
                                </ul>
                            </div>
                            <div>
                                <ul class="mli">
                                    <li style="padding:5px 0px;">
                                        <span class="mct">
                                            <span class="lif">Push <%=GetResource("SetAlarmTimeUse")%>(Push - <%=GetResource("MsgApp")%>) <br /><input type="checkbox" id="cbAlarmTimeUse" runat="server" class="rd" /> <%=GetResource("IsActiveTitY")%> ( <%=GetResource("BasedOnTime")%>) </span>
                                            <span id="divAlarmTime" runat="server" style="display:none;">
                                                <br />
                                                <br />
                                                <span class="lif"><%=GetResource("AlarmStartTime")%>:
                                                    <asp:DropDownList class="form-control" id="alarmStartHour" runat="server" style="width:70px; display:inline-block;">
                                                        <asp:ListItem value="01">01</asp:ListItem>
                                                        <asp:ListItem value="02">02</asp:ListItem>
                                                        <asp:ListItem value="03">03</asp:ListItem>
                                                        <asp:ListItem value="04">04</asp:ListItem>
                                                        <asp:ListItem value="05">05</asp:ListItem>
                                                        <asp:ListItem value="06">06</asp:ListItem>
                                                        <asp:ListItem value="07">07</asp:ListItem>
                                                        <asp:ListItem value="08">08</asp:ListItem>
                                                        <asp:ListItem value="09" selected="selected">09</asp:ListItem>
                                                        <asp:ListItem value="10">10</asp:ListItem>
                                                        <asp:ListItem value="11">11</asp:ListItem>
                                                        <asp:ListItem value="12">12</asp:ListItem>
                                                        <asp:ListItem value="13">13</asp:ListItem>
                                                        <asp:ListItem value="14">14</asp:ListItem>
                                                        <asp:ListItem value="15">15</asp:ListItem>
                                                        <asp:ListItem value="16">16</asp:ListItem>
                                                        <asp:ListItem value="17">17</asp:ListItem>
                                                        <asp:ListItem value="18">18</asp:ListItem>
                                                        <asp:ListItem value="19">19</asp:ListItem>
                                                        <asp:ListItem value="20">20</asp:ListItem>
                                                        <asp:ListItem value="21">21</asp:ListItem>
                                                        <asp:ListItem value="22">22</asp:ListItem>
                                                        <asp:ListItem value="23">23</asp:ListItem>
                                                        <asp:ListItem value="24">24</asp:ListItem>
                                                    </asp:DropDownList> : 
                                                    <asp:DropDownList class="form-control" id="alarmStartMin" runat="server" style="width:70px; display:inline-block;">
                                                        <asp:ListItem value="00" selected="selected">00</asp:ListItem>
                                                        <asp:ListItem value="30">30</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <br />
                                                <br />
                                                <span class="lif"><%=GetResource("AlarmEndTime")%>:
                                                    <asp:DropDownList class="form-control" id="alarmEndHour" runat="server" style="width:70px; display:inline-block;">
                                                        <asp:ListItem value="01">01</asp:ListItem>
                                                        <asp:ListItem value="02">02</asp:ListItem>
                                                        <asp:ListItem value="03">03</asp:ListItem>
                                                        <asp:ListItem value="04">04</asp:ListItem>
                                                        <asp:ListItem value="05">05</asp:ListItem>
                                                        <asp:ListItem value="06">06</asp:ListItem>
                                                        <asp:ListItem value="07">07</asp:ListItem>
                                                        <asp:ListItem value="08">08</asp:ListItem>
                                                        <asp:ListItem value="09">09</asp:ListItem>
                                                        <asp:ListItem value="10">10</asp:ListItem>
                                                        <asp:ListItem value="11">11</asp:ListItem>
                                                        <asp:ListItem value="12">12</asp:ListItem>
                                                        <asp:ListItem value="13">13</asp:ListItem>
                                                        <asp:ListItem value="14">14</asp:ListItem>
                                                        <asp:ListItem value="15">15</asp:ListItem>
                                                        <asp:ListItem value="16">16</asp:ListItem>
                                                        <asp:ListItem value="17">17</asp:ListItem>
                                                        <asp:ListItem value="18" selected="selected">18</asp:ListItem>
                                                        <asp:ListItem value="19">19</asp:ListItem>
                                                        <asp:ListItem value="20">20</asp:ListItem>
                                                        <asp:ListItem value="21">21</asp:ListItem>
                                                        <asp:ListItem value="22">22</asp:ListItem>
                                                        <asp:ListItem value="23">23</asp:ListItem>
                                                        <asp:ListItem value="24">24</asp:ListItem>
                                                    </asp:DropDownList> : 
                                                    <asp:DropDownList class="form-control" id="alarmEndMin" runat="server" style="width:70px; display:inline-block;">
                                                        <asp:ListItem value="00" selected="selected">00</asp:ListItem>
                                                        <asp:ListItem value="30">30</asp:ListItem>
                                                    </asp:DropDownList>
                                                </span>
                                                <br />&nbsp;
                                            </span>
                                        </span>
                                    </li>
                                </ul>
                            </div>
                            <div>
                                <ul class="mli">
                                    <li>
                                        <span class="mct">
                                            <span class="lif"><%=GetResource("NotifiView")%> & Push <%=GetResource("AlarmSetting")%>(Push - <%=GetResource("MsgApp")%>) </span><br />
                                            <span class="lif">(<%=GetResource("MsgNotifi")%>)</span>
                                            <span id="sMail" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Mail")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlMailList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlMailAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sApproval" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Approval")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlApprovalList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlApprovalAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sBoard" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Board")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlBoardList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlBoardAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sPim" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Pims")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlPimsList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlPimsAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sTask" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Task")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlTaskList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlTaskAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span>
                                                <br />
                                                <br />
                                                <%=GetResource("Work")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlWorkList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlWorkAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sCommunity" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("Community")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlCommunityList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlCommunityAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sResource" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("ResourceManager")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlResourceList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlResourceAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span id="sSNS" runat="server">
                                                <br />
                                                <br />
                                                <%=GetResource("SNS")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlSNSList" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <asp:DropDownList ID="ddlSNSAlarm" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>                                            
                                            <span><br /><br /><%=GetResource("AlarmNotice")%> : </span>
                                            <asp:DropDownList ID="ddlNoticeAlarm" runat="server" Style="display: inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                            <br />&nbsp;
                                        </span>
                                    </li>
                                </ul>                               
                                <ul class="mli">
                                    <li style="padding:5px 0px;">
                                        <span class="mct">
                                            <span class="lif">
                                                <%=GetResource("MobileAuth")%>
                                                (<%=GetResource("MsgApp")%>)
                                            </span>
                                            <span> 
                                                <br />
                                                <%=GetResource("LoginType")%> : 
                                            </span>
                                            <asp:DropDownList ID="ddlLoginType" runat="server" style="display:inline-block; width:auto;" CssClass="form-control"></asp:DropDownList>
                                        </span>
                                    </li>
                                    <li class="contentblock">
                                        <div class="buttongroup full-width" id="logout" runat="server">
                                            <a class="btn btn-outline-light full-width" href="../Portal/SetLogOut.aspx"><%=GetResource("LogOut")%></a>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/jquery.common.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/bootstrap.min.js"></script>
    <script type="text/javascript" src="/DeskPlusMobile/Html/Js/Design/navimobile.js"></script>
    <script type="text/javascript">
        var UUID = "<%=UUID%>";
        var SkinType = "<%=SkinType%>";
        var PimsCheck = "<%=pimsCheck%>";
        var R_Save1 = "<%=GetResource("Save1") %>";
        var R_Menu = "<%=GetResource("Menu")%>";
        var R_AlarmTimeCheck = "<%=GetResource("AlarmTimeCheck")%>";
    </script>
    <script type="text/javascript" src="/DeskPlusMobile/Js/Config/List.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusMobile/Js/Config/List.js");
    </script>
</body>
</html>
