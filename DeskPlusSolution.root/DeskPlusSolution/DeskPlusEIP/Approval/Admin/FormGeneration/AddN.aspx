<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Zeospace.DeskPlus.Approval.Web.Admin.FormGeneration.Add" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Approval.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Approval.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
</head>
<body scroll="auto">
    <form id="form1" runat="server">

<div id="app_wrap">
<div id="contentWrap">

    <!-- 트리 //-->
    <div id="left">
    <div id="layout_left" class="left-menu admin">

     	<ul class="">
		<li>

        <div id="layout_treetop">
            <span class="treetop_title"><%=GetResource("ManageForm")%></span>
        </div>
        <div id="layout_treeboxAcc" style="height:90%;">
            <div class="accordion">

                <div class="menu"><a href="#" onclick="" ><%=GetResource("ProcessInfo")%></a></div>
                <div class="app_menusub">
                    <ul class="app_reg_tag">
                        <li class="tit"><%=GetResource("Send")%></li>
                        <li><img id="ZD0" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD0.gif" alt="" title="<%=GetResource("ZD0")%>" class="appTag" /></li>
                        <li><img id="ZD1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD1.gif" alt="" title="<%=GetResource("ZD")%>1" class="appTag" /></li>
                        <li><img id="ZD2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD2.gif" alt="" title="<%=GetResource("ZD")%>2" class="appTag" /></li>
                        <li><img id="ZD3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD3.gif" alt="" title="<%=GetResource("ZD")%>3" class="appTag" /></li>
                        <li><img id="ZD4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD4.gif" alt="" title="<%=GetResource("ZD")%>4" class="appTag" /></li>
                        <li><img id="ZD5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD5.gif" alt="" title="<%=GetResource("ZD")%>5" class="appTag" /></li>
                        <li><img id="ZD6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD6.gif" alt="" title="<%=GetResource("ZD")%>6" class="appTag" /></li>
                        <li><img id="ZD7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD7.gif" alt="" title="<%=GetResource("ZD")%>7" class="appTag" /></li>
                        <li><img id="ZD8" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD8.gif" alt="" title="<%=GetResource("ZD")%>8" class="appTag" /></li>
                        <li><img id="ZD9"  src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD9.gif" alt="" title="<%=GetResource("ZD")%>9" class="appTag" /></li>
                        <li><img id="ZD10" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD10.gif" alt="" title="<%=GetResource("ZD")%>10" class="appTag" /></li>
                        <li><img id="ZD11" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD11.gif" alt="" title="<%=GetResource("ZD")%>11" class="appTag" /></li>
                        <li><img id="ZD12" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD12.gif" alt="" title="<%=GetResource("ZD")%>12" class="appTag" /></li>
                        <li><img id="ZD13" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZD13.gif" alt="" title="<%=GetResource("ZD")%>13" class="appTag" /></li>
                    </ul>
                    <ul class="app_reg_tag">
                        <li class="tit"><%=GetResource("ZH")%></li>
                        <li><img id="ZH" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH.gif" alt="" title="<%=GetResource("ZH")%>" onclick="Add.MultiTag('ZH')" class="appTagMulti" /></li>
                        <li><img id="ZH1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH1.gif" alt="" title="<%=GetResource("ZH")%>1" class="appTag" /></li>
                        <li><img id="ZH2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH2.gif" alt="" title="<%=GetResource("ZH")%>2" class="appTag" /></li>
                        <li><img id="ZH3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH3.gif" alt="" title="<%=GetResource("ZH")%>3" class="appTag" /></li>
                        <li><img id="ZH4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH4.gif" alt="" title="<%=GetResource("ZH")%>4" class="appTag" /></li>
                        <li><img id="ZH5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH5.gif" alt="" title="<%=GetResource("ZH")%>5" class="appTag" /></li>
                        <li><img id="ZH6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH6.gif" alt="" title="<%=GetResource("ZH")%>6" class="appTag" /></li>
                        <li><img id="ZH7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH7.gif" alt="" title="<%=GetResource("ZH")%>7" class="appTag" /></li>
                        <li><img id="ZH8" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH8.gif" alt="" title="<%=GetResource("ZH")%>8" class="appTag" /></li>
                        <li><img id="ZH9" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH9.gif" alt="" title="<%=GetResource("ZH")%>9" class="appTag" /></li>
                        <li><img id="ZH10" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH10.gif" alt="" title="<%=GetResource("ZH")%>10" class="appTag" /></li>
                        <li><img id="ZH11" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH11.gif" alt="" title="<%=GetResource("ZH")%>11" class="appTag" /></li>
                        <li><img id="ZH12" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH12.gif" alt="" title="<%=GetResource("ZH")%>12" class="appTag" /></li>
                        <li><img id="ZH13" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZH13.gif" alt="" title="<%=GetResource("ZH")%>13" class="appTag" /></li>
                    </ul>
                    <ul class="app_reg_tag" style="display:">
                        <li class="tit"><%=GetResource("ZR")%></li>
                        <li><img id="ZR1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR1.gif" alt="" title="<%=GetResource("ZR")%>1" class="appTag" /></li>
                        <li><img id="ZR2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR2.gif" alt="" title="<%=GetResource("ZR")%>2" class="appTag" /></li>
                        <li><img id="ZR3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR3.gif" alt="" title="<%=GetResource("ZR")%>3" class="appTag" /></li>
                        <li><img id="ZR4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR4.gif" alt="" title="<%=GetResource("ZR")%>4" class="appTag" /></li>
                        <li><img id="ZR5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR5.gif" alt="" title="<%=GetResource("ZR")%>5" class="appTag" /></li>
                        <li><img id="ZR6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR6.gif" alt="" title="<%=GetResource("ZR")%>6" class="appTag" /></li>
                        <li><img id="ZR7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR7.gif" alt="" title="<%=GetResource("ZR")%>7" class="appTag" /></li>
                        <li><img id="ZR8" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR8.gif" alt="" title="<%=GetResource("ZR")%>8" class="appTag" /></li>
                        <li><img id="ZR9" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR9.gif" alt="" title="<%=GetResource("ZR")%>9" class="appTag" /></li>
                        <li><img id="ZR10" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR10.gif" alt="" title="<%=GetResource("ZR")%>10" class="appTag" /></li>
                        <li><img id="ZR11" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR11.gif" alt="" title="<%=GetResource("ZR")%>11" class="appTag" /></li>
                        <li><img id="ZR12" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR12.gif" alt="" title="<%=GetResource("ZR")%>12" class="appTag" /></li>
                        <li><img id="ZR13" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR13.gif" alt="" title="<%=GetResource("ZR")%>13" class="appTag" /></li>
                        <li><img id="ZR14" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZR14.gif" alt="" title="<%=GetResource("ZR")%>14" class="appTag" /></li>
                    </ul>
                    <ul class="app_reg_tag">
                        <li class="tit"><%=GetResource("Receive")%></li>
                        <li><img id="ZQ" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQDpt.gif" alt="" title="<%=GetResource("ZQ")%>" onclick="Add.MultiTag('ZQ')" class="appTagMulti" /></li>
                        <li><img id="ZP" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZPDpt.gif" alt="" title="<%=GetResource("ZP")%>" onclick="Add.MultiTag('ZP')" class="appTagMulti" /></li>
                    </ul>
                    <ul class="app_reg_tag">
                        <li class="tit"><%=GetResource("Receive")%></li>
                        <li><img id="ZQ01" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ0.gif" alt="" title="<%=GetResource("ZQ")%>" class="appTag" /></li>
                        <li><img id="ZQ01_ZD1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ1.gif" alt="" title="<%=GetResource("ZQ")%>1" class="appTag" /></li>
                        <li><img id="ZQ01_ZD2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ2.gif" alt="" title="<%=GetResource("ZQ")%>2" class="appTag" /></li>
                        <li><img id="ZQ01_ZD3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ3.gif" alt="" title="<%=GetResource("ZQ")%>3" class="appTag" /></li>
                        <li><img id="ZQ01_ZD4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ4.gif" alt="" title="<%=GetResource("ZQ")%>4" class="appTag" /></li>
                        <li><img id="ZQ01_ZD5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ5.gif" alt="" title="<%=GetResource("ZQ")%>5" class="appTag" /></li>
                        <li><img id="ZQ01_ZD6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ6.gif" alt="" title="<%=GetResource("ZQ")%>6" class="appTag" /></li>
                        <li><img id="ZQ01_ZD7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ7.gif" alt="" title="<%=GetResource("ZQ")%>7" class="appTag" /></li>
                        <li><img id="ZQ02" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ0.gif" alt="" title="<%=GetResource("ZQ")%>" class="appTag" /></li>
                        <li><img id="ZQ02_ZD1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ1.gif" alt="" title="<%=GetResource("ZQ")%>1" class="appTag" /></li>
                        <li><img id="ZQ02_ZD2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ2.gif" alt="" title="<%=GetResource("ZQ")%>2" class="appTag" /></li>
                        <li><img id="ZQ02_ZD3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ3.gif" alt="" title="<%=GetResource("ZQ")%>3" class="appTag" /></li>
                        <li><img id="ZQ02_ZD4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ4.gif" alt="" title="<%=GetResource("ZQ")%>4" class="appTag" /></li>
                        <li><img id="ZQ02_ZD5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ5.gif" alt="" title="<%=GetResource("ZQ")%>5" class="appTag" /></li>
                        <li><img id="ZQ02_ZD6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ6.gif" alt="" title="<%=GetResource("ZQ")%>6" class="appTag" /></li>
                        <li><img id="ZQ02_ZD7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZQ7.gif" alt="" title="<%=GetResource("ZQ")%>7" class="appTag" /></li>

                        <li><img id="ZP01" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP0.gif" alt="" title="<%=GetResource("ZQ")%>" class="appTag" /></li>
                        <li><img id="ZP01_ZD1" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP1.gif" alt="" title="<%=GetResource("ZQ")%>1" class="appTag" /></li>
                        <li><img id="ZP01_ZD2" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP2.gif" alt="" title="<%=GetResource("ZQ")%>2" class="appTag" /></li>
                        <li><img id="ZP01_ZD3" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP3.gif" alt="" title="<%=GetResource("ZQ")%>3" class="appTag" /></li>
                        <li><img id="ZP01_ZD4" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP4.gif" alt="" title="<%=GetResource("ZQ")%>4" class="appTag" /></li>
                        <li><img id="ZP01_ZD5" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP5.gif" alt="" title="<%=GetResource("ZQ")%>5" class="appTag" /></li>
                        <li><img id="ZP01_ZD6" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP6.gif" alt="" title="<%=GetResource("ZQ")%>6" class="appTag" /></li>
                        <li><img id="ZP01_ZD7" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZP7.gif" alt="" title="<%=GetResource("ZQ")%>7" class="appTag" /></li>
                    </ul>
                    <ul class="app_reg_tag">
                        <li class="tit"><%=GetResource("Carboncopy")%></li>
                        <li><img id="ZC" src="<%=BaseImgPath%>/Lang/<%=PreLang%>/app_reg_ZC.gif" alt="" title="<%=GetResource("Carboncopy")%>" onclick="Add.MultiTag('ZC')" class="appTagMulti" /></li>
                    </ul>

                    <div class="app_gap"></div>
                    <ul class="app_reg_edit">
                        <li class="tit"><%=GetResource("EditItem")%></li>
                        <li class="tagdel"><%=GetResource("Btn_Del")%></li>
                        <li class="area"></li>
                    </ul>
                </div>
                <div class="menu"><a href="#" onclick="" ><%=GetResource("FlowInfo")%></a></div>
                <div class="app_menusub">

                    <ul class="app_reg_flow">
                        <li id="FLOWZD" class="ZD" title="<%=GetResource("FlowZDLoc")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("FlowZD2")%></li>
                    </ul>
                    <ul class="app_reg_flow">
                        <li id="FLOWZH" class="ZH" title="<%=GetResource("FlowZHLoc")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("FlowZH2")%></li>
                    </ul>
                    <ul class="app_reg_flow">
                        <li id="FLOWZA" class="ZA" title="<%=GetResource("FlowZALoc")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("FlowZA2")%></li>
                    </ul>
                    <ul class="app_reg_flow" style="display:">
                        <li id="FLOWZR" class="ZR" title="<%=GetResource("FlowZRLoc")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("FlowZR")%></li>
                    </ul>

                    <div class="app_gap"></div>
                    <ul class="app_reg_edit">
                        <li class="tit"><%=GetResource("EditItem")%></li>
                        <li class="tagdel"><%=GetResource("Btn_Del")%></li>
                        <li class="area"></li>
                    </ul>
                    <div class="app_gap"></div>
                    <span class="app_subtxt"><%=GetResource("CanNotUseTogether")%></span>
                </div>
                <div class="menu"><a href="#" onclick="" ><%=GetResource("FormInfo")%></a></div>
                <div class="app_menusub">
                    <ul class="app_reg_flow">
                        <li class="tit"><%=GetResource("BasicInfo")%></li>
                        <li id="COMPANY" title="<%=GetResource("Company")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("Company")%></li>
                        <li id="NO" title="<%=GetResource("DocNumber")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("DocNumber")%></li>
                        <li id="DATE" title="<%=GetResource("DateOfWrite")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("DateOfWrite")%></li>
                        <li id="LIFE" title="<%=GetResource("KeepYear")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("KeepYear")%></li>
                        <li id="SUBJ" title="<%=GetResource("Subject")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("Subject")%></li>
                        <li id="UPDPT" title="<%=GetResource("ReportDept")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("ReportDept")%></li>
                        <li id="UPJC" title="<%=GetResource("ReportTitle")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("ReportTitle")%></li>
                        <li id="UPNM" title="<%=GetResource("ReportUser")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("ReportUser")%></li>
                        <li id="EMPNUM" title="<%=GetResource("ReportCompanyNum")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("ReportCompanyNum")%></li>
                    </ul>
                    <ul class="app_reg_flow">
                        <li class="tit"><%=GetResource("DocContents")%></li>
                        <li id="OPINION" title="<%=GetResource("Opinion")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("Opinion")%></li>
                        <li id="OPINIONVIEW" title="<%=GetResource("AttachOpinion")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("AttachOpinion")%></li>
                        <li id="BODY_etc" title="<%=GetResource("Input")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("Input")%></li>
                        <li id="BODY" title="<%=GetResource("TagDic_BODY")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("TagDic_BODY")%></li>
                        <li id="EDITOR" title="<%=GetResource("Editor")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("Editor")%></li>
                        <li id="PRINT" title="<%=GetResource("TagDic_Print")%>" onclick="Add.tag_click(this)" isdiv="true"><%=GetResource("Print")%></li>
                    </ul>
                    <ul class="app_reg_flow" style="display:none">
                        <li class="tit"><%=GetResource("EnforceDoc")%></li>
                        <li id="ENFORCEDEPTNAME" title="<%=GetResource("ReceiveDept")%>" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("ReceiveDept")%></li>
                        <li id="ENFORCENAME" title="<%=GetResource("Carboncopy")%>1" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("Carboncopy")%></li>
                        <li id="ENFORCERECEIVECARBON" title="<%=GetResource("Carboncopy")%>2" onclick="Add.tag_click(this)" isdiv="false"><%=GetResource("Carboncopy")%></li>
                    </ul>

                    <div class="app_gap"></div>
                    <ul class="app_reg_edit">
                        <li class="tit"><%=GetResource("EditItem")%></li>
                        <li class="tagdel"><%=GetResource("Btn_Del")%></li>
                        <li class="area"></li>
                    </ul>
                    <div class="app_gap"></div>
                    <ul class="app_reg_edit">
                        <li class="tit"><%=GetResource("ItemAtt")%></li>
                        <li class="tagset" title="<%=GetResource("ItemSetMsg")%>"><%=GetResource("ItemSet")%></li>
                        <li class="att">

                            <table border="0" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td><span class="bold"><%=GetResource("ItemOutline")%>:</span><select id="selAttObj" class="searchselect form-control"><option value="single"><%=GetResource("ItemSingle")%></option><option value="multi"><%=GetResource("ItemMulti")%></option></select></td>
                                </tr>
                                <tr>
                                    <td>
                                        <span class="bold" title="<%=GetResource("ItemWHSub")%>"><%=GetResource("ItemWH")%>:</span>
                                        <input type="text" id="objw" style="width:25px" />
                                        <span class="bold">/</span>
                                        <input type="text" id="objh" style="width:25px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding:5px 0px 0px 0px">
                                        <img id="alignLT" src="<%=BaseImgPath%>/Approval/app_alignLT.gif" onclick="Add.align_click('LT')" alt="" />
                                        <img id="alignCT" src="<%=BaseImgPath%>/Approval/app_alignCT.gif" onclick="Add.align_click('CT')" alt="" />
                                        <img id="alignRT" src="<%=BaseImgPath%>/Approval/app_alignRT.gif" onclick="Add.align_click('RT')" alt="" />
                                        <img id="alignLM" src="<%=BaseImgPath%>/Approval/app_alignLM.gif" onclick="Add.align_click('LM')" alt="" />
                                        <img id="alignCM" src="<%=BaseImgPath%>/Approval/app_alignCM.gif" onclick="Add.align_click('CM')" alt="" />
                                        <img id="alignRM" src="<%=BaseImgPath%>/Approval/app_alignRM.gif" onclick="Add.align_click('RM')" alt="" />
                                        <img id="alignLB" src="<%=BaseImgPath%>/Approval/app_alignLB.gif" onclick="Add.align_click('LB')" alt="" />
                                        <img id="alignCB" src="<%=BaseImgPath%>/Approval/app_alignCB.gif" onclick="Add.align_click('CB')" alt="" />
                                        <img id="alignRB" src="<%=BaseImgPath%>/Approval/app_alignRB.gif" onclick="Add.align_click('RB')" alt="" />
                                    </td>
                                </tr>

                            </tbody>
                            </table>
                            

                        </li>
                    </ul>

                    <div class="app_gap"></div>
                    <span class="app_subtxt">* <%=GetResource("Contents")%></span>
                    <div class="app_gap"></div>
                    <span class="app_subtxt"><%=GetResource("SetBasicInfo")%></span>
                </div>
                <div class="menu"><a href="#" onclick="" ><%=GetResource("SetETC")%></a></div>
                <div class="app_menusub">
                    <ul class="app_reg_def">
                        <li class="titL"><%=GetResource("IsActiveTit")%>:</li>
                        <li><asp:RadioButtonList ID="rblIsActive" runat="server" RepeatDirection="Horizontal" CssClass="table_typeH"></asp:RadioButtonList></li>
                    </ul>
                    
                </div>

            </div>
        </div>
        <div id="layout_treebuttom"></div>

			</li>
		</ul>
	</div>


    </div>
    </div>

    <!-- 내용 //-->
    <div id="content">
    <div id="layout_body" class="write" style="padding-left:10px">
    <div class="layout_full">

        <div id="layout_header">
            <!-- 버튼 //-->
            <ul class="btn_left">
            </ul>
            
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnPrint" runat="server" Visible="false" class="btn" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server"  class="btn btn-secondary" ></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnDel" runat="server" class="btn btn-danger"></cc3:ClientSmartButton></li>
                <li><cc3:ClientSmartButton ID="cbtnClose" runat="server"   class="btn" ></cc3:ClientSmartButton></li>
            </ul>
        </div>

        <div id="layout_content">
           <div class="row">
             <div class="col-12 content-area-body-left" >            

            <table class="table_typeC">
            <caption><%=GetResource("BasicDocInfo")%></caption>
            <colgroup>
                <col width="80px" />
                <col />
                <col width="80px" />
                <col />
   			</colgroup>
            <tbody>
                <tr>
                    <th><%=GetResource("FormName2")%></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbFormName" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                    </td>
                    <th><%=GetResource("ShortFormName")%></th>
                    <td>
                        <cc3:ClientSmartTextBox ID="ctbNameCode" InputMode="None" runat="server" Required="true" CssStyle="width:95% !important;"></cc3:ClientSmartTextBox>
                    </td>
                </tr>
            </tbody>
            </table><p>&nbsp;</p>
            
            <!-- 양식로드 //-->
            <div class="app_frmEditor">
                <div id="divXFE" style="height: 5px;">    
                <script type="text/javascript" src="/DeskPlusEIP/Data/Editor/XFE2/js/xfe_main.js"></script>  
                    <script type="text/javascript">
                        var form_click = function (_event) {    //alert(event);
                            //nowEl = _event.srcElement;          //alert(nowEl.id + "---" + nowEl.tagName + "---" + nowEl.outerHTML);
                            nowEl = _event.srcElement ? _event.srcElement : _event.target;  //ByFreeman FireFox 체크 20120713
                            Add.displayInfo();                  //편집항목Display
                        }

                        xfe = new XFE({
                            basePath: "<%=BaseHttp%>" + document.domain + "/DeskPlusEIP/Data/Editor/XFE2",
                            width: '100%',
                            height: '730px',
                            onMouseDown: form_click
                        });
                        xfe.render('divXFE');
                    </script>
                </div>
            </div>
            </div>
            </div>


        </div>

    </div>
    </div>
    </div>
        
</div>
</div>

<ul id="appTagPopup" class="app_ctxMenu">
    <li class="stamp"><a href="#STMPP"><%=GetResource("Stamp")%></a></li>
    <li class="info"><a href="#INFO0"><%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO3"><%=GetResource("Name")%></a></li>
    <li class="info"><a href="#INFO4"><%=GetResource("TitleName2")%></a></li>
    <li class="info"><a href="#INFO9"><%=GetResource("DutyName2")%></a></li>
    <li class="info"><a href="#INFO5"><%=GetResource("Dept")%></a></li>
    <li class="info"><a href="#INFO6"><%=GetResource("Name")%> <%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO1"><%=GetResource("TitleName2")%> <%=GetResource("Name")%></a></li>
    <li class="info"><a href="#INFO2"><%=GetResource("TitleName2")%> <%=GetResource("Name")%> <%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO7"><%=GetResource("Dept")%> <%=GetResource("TitleName2")%> <%=GetResource("Name")%> <%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO8"><%=GetResource("ApproveDate")%> <%=GetResource("ApproveTime")%></a></li>
    <li class="info"><a href="#INFO10"><%=GetResource("DutyName2")%> <%=GetResource("Name")%></a></li>
    <li class="info"><a href="#INFO11"><%=GetResource("DutyName2")%> <%=GetResource("Name")%> <%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO12"><%=GetResource("Dept")%> <%=GetResource("DutyName2")%> <%=GetResource("Name")%> <%=GetResource("ApproveDate")%></a></li>
    <li class="info"><a href="#INFO13"><%=GetResource("Name")%> <%=GetResource("ApproveDate")%> <%=GetResource("ApproveTime")%></a></li>
    <li class="info"><a href="#INFO14"><%=GetResource("ApproveDateMD")%></a></li>
    <li class="info"><a href="#INFO15"><%=GetResource("ApproveTime")%></a></li>
</ul>

<div style="display:none;">
    <asp:LinkButton ID="lbtnSubmit" runat="server" OnClick="lbtnSubmit_Click"></asp:LinkButton>
    <asp:TextBox ID="tbHiddenMime_contents" Style="display: none"   Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
    <textarea id="tbHiddenHTML_contents" name="tbHiddenHTML_contents" runat="server" rows="1" cols="20"></textarea>

    <asp:TextBox ID="tbHiddenSeq" Style="display: none" Width="0" TextMode="MultiLine" runat="server"></asp:TextBox>
</div>

    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all_1.8.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.contextMenu.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.hashtable.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        /**********************************************************************************************
        전역 변수들
        **********************************************************************************************/

        var formID          = "<%=FormID%>";
        var currentPage     = "<%=CurrentPage%>";
        var recordPerPage   = "<%=RecordPerPage%>";
        var searchField     = "<%=SearchField%>";
        var searchValue     = "<%=SearchValue%>";
        var searchTerm      = "<%=SearchTerm%>";
        var searchTermValue = "<%=SearchTermValue%>";
        var order           = "<%=Order%>";
        
        var nowEl = null;
        var editorEl = null;
        var tagDic = new jQuery.Hashtable();       
        var FullUrl                 = "<%=FullDomainPath%>";
        var NonActiveX              = "<%=NonActiveX%>";
        var ClientQueryString       = "<%=Page.ClientQueryString %>";
        var R_Msg_ConfirmEditorTab  = "<%=GetResource("Msg_ConfirmEditorTab") %>";
        var R_Msg_Del               = "<%=GetResource("Msg_Del") %>";
        var R_Msg_Close             = "<%=GetResource("Msg_Close") %>";
        var R_Msg_ErrCont           = "<%=GetResource("Msg_ErrCont") %>";

        var R_Msg_ErrInputName = "<%=GetResource("NeedFormName") %>"; // "양식명을 입력하세요.";
        var R_Msg_ErrInputNameCode = "<%=GetResource("Msg_ErrInputNameCode") %>"; // "양식코드를 입력하세요.";
        var R_Msg_Replace = "<%=GetResource("Msg_Change") %>";      //"변경하시겠습니까?";
        var R_Msg_ErrSelTarget = "<%=GetResource("MSG_SelTarget") %>";      //"속성을 지정할 대상을 먼저 양식에서 선택하세요.";
        var R_Msg_Ex1   ="* " + "<%=GetResource("Contents") %>" + " : ";
        var R_MSG_SUCCESS = "<%=GetResource("MSG_SUCCESS") %>";     //"처리되었습니다";
        var R_MSG_SPECIALCHAR = "<%=GetResource("MSG_Secure") %>";        // "해당 특수문자는 보안취약성 공격에 사용되므로 제한됩니다.";
    
        var TagDic_INFO0 = "<%=GetResource("TagDic_INFO0") %>";
        var TagDic_INFO1 = "<%=GetResource("TagDic_INFO1") %>";
        var TagDic_INFO2 = "<%=GetResource("TagDic_INFO2") %>";
        var TagDic_INFO3 = "<%=GetResource("TagDic_INFO3") %>";
        var TagDic_INFO4 = "<%=GetResource("TagDic_INFO4") %>";
        var TagDic_INFO5 = "<%=GetResource("TagDic_INFO5") %>";
        var TagDic_INFO6 = "<%=GetResource("TagDic_INFO6") %>";
        var TagDic_INFO7 = "<%=GetResource("TagDic_INFO7") %>";
        var TagDic_INFO8 = "<%=GetResource("TagDic_INFO8") %>";
        var TagDic_INFO9 = "<%=GetResource("TagDic_INFO9") %>";
        var TagDic_INFO10 = "<%=GetResource("TagDic_INFO10") %>";
        var TagDic_INFO11 = "<%=GetResource("TagDic_INFO11") %>";
        var TagDic_INFO12 = "<%=GetResource("TagDic_INFO12") %>";
        var TagDic_INFO13 = "<%=GetResource("TagDic_INFO13") %>";
        var TagDic_INFO14 = "<%=GetResource("TagDic_INFO14") %>";
        var TagDic_INFO15 = "<%=GetResource("TagDic_INFO15") %>";
        var TagDic_ZD0 = "<%=GetResource("TagDic_ZD0") %>";
        var TagDic_ZD = "<%=GetResource("TagDic_ZD") %>";
        var TagDic_ZP0 = "<%=GetResource("TagDic_ZP0") %>";
        var TagDic_ZP = "<%=GetResource("TagDic_ZP") %>";
        var TagDic_ZH = "<%=GetResource("TagDic_ZH") %>";
        var TagDic_ZC = "<%=GetResource("TagDic_ZC") %>";
        var TagDic_ZR = "<%=GetResource("TagDic_ZR") %>";
        var TagDic_ZA = "<%=GetResource("TagDic_ZA") %>";
        var TagDic_NO = "<%=GetResource("TagDic_NO") %>";
        var TagDic_DATE = "<%=GetResource("TagDic_DATE") %>";
        var TagDic_LIFE = "<%=GetResource("TagDic_LIFE") %>";
        var TagDic_SUBJ = "<%=GetResource("TagDic_SUBJ") %>";
        var TagDic_BODY = "<%=GetResource("TagDic_BODY") %>";
        var TagDic_BODY_etc = "<%=GetResource("TagDic_BODY_etc") %>";
        var TagDic_EDITOR = "<%=GetResource("TagDic_EDITOR") %>";
        var TagDic_COMPANY = "<%=GetResource("TagDic_COMPANY") %>";
        var TagDic_UPNM = "<%=GetResource("TagDic_UPNM") %>";
        var TagDic_EMPNUM = "<%=GetResource("TagDic_EMPNUM") %>";
        var TagDic_UPDPT = "<%=GetResource("TagDic_UPDPT") %>";
        var TagDic_UPJC = "<%=GetResource("TagDic_UPJC") %>";
        var TagDic_OPINION = "<%=GetResource("TagDic_OPINION") %>";
        var TagDic_OPINIONVIEW = "<%=GetResource("TagDic_OPINIONVIEW") %>";
        var TagDic_FLOWZD = "<%=GetResource("TagDic_FLOWZD") %>";
        var TagDic_FLOWZH = "<%=GetResource("TagDic_FLOWZH") %>";
        var TagDic_FLOWZA = "<%=GetResource("TagDic_FLOWZA") %>";
        var TagDic_FLOWZR = "<%=GetResource("TagDic_FLOWZR") %>";
        var TagDic_ENFORCEDEPTNAME = "<%=GetResource("TagDic_ENFORCEDEPTNAME") %>";
        var TagDic_ENFORCENAME = "<%=GetResource("TagDic_ENFORCENAME") %>";
        var TagDic_ENFORCERECEIVECARBON = "<%=GetResource("TagDic_ENFORCERECEIVECARBON") %>";
        var TagDic_Print = "<%=GetResource("Print") %>";

    </script>
    
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/FormGeneration/Add.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Approval/Js/Admin/FormGeneration/Common.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/FormGeneration/Common.js");
        //JsCall("/DeskPlusEIP/Approval/Js/Admin/FormGeneration/Add.js");
    </script>

    </form>
</body>
</html>