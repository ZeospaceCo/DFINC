<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMaster.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.AddMaster" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>

<html>
<head><meta charset="UTF-8">
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Etc.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Etc.css"/>
    <link rel="stylesheet" type="text/css" href="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.css" />
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DatePick.css" /><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DatePick.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" />
    <div id="layout_body">
        <div class="layout_full" style="padding:0px;border:0px solid red;">
    
            <div class="tabAline">
                <ul class="tabA">
                    <li style="font-size:12px;" class="last"><a href="#" class="tab_on"  id="Basic">기본정보</a></li>
                </ul>
            </div>

            <div class="gap"></div>
            <div id="layout_content" style="padding:0px;margin:0px;border:0px solid yellow;">
            <div class="master" style="display:inline-block;width:100%;min-height:45px;border:0px solid #efefef;height:100px;">
                <table class="table_typeB">
			    <caption>Vendor Info</caption>
			    <colgroup>
                    <col width="10%" />
                    <col width="16%" />
                    <col width="10%" />
                    <col width="9%" />
                    <col width="10%" />
                    <col  />
                    <col width="10%" />
                    <col width="14%" />
			    </colgroup>
			    <tbody>
				    <tr>
					    <th style="font-weight:bold;">Vendor</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctbVendorCD" InputMode="None" runat="server" Required="true" width="30px" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox ID="ctbVendorNM" InputMode="None" runat="server" Required="true" width="50px" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallVendor('M')" style="cursor:pointer;" alt="" />
                        </td>
                        <th style="font-weight:bold;">Posting Date</th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbPostingDate" Width="90%" MaxLength="12" InputMode="None" Required="true" ></cc3:ClientSmartTextBox></td>
                        <th style="font-weight:bold;display:none">Posting Group</th>
					    <td style="display:none">
                            <cc3:ClientSmartTextBox ID="ctvPostingGroup" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PostingGroup('M')" style="cursor:pointer;" alt="" />
                        </td>
                        <th style="font-weight:bold;display:none">Bank Account</th>
					    <td style="display:none">
                            <cc3:ClientSmartTextBox ID="ctvBankAccountCD" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallBankAccount('M')" style="cursor:pointer;" alt="" />
                        </td>

                        <th style="font-weight:bold;">DepartMent</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctvDepartMent" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallDepartMent('M')" style="cursor:pointer;" alt="" />
                            <span style = "display:none"><cc3:ClientSmartTextBox ID="ctvDepartMentCD" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox></span>
                        </td>
                        <th style="font-weight:bold;">Employee</th>
					    <td >
                            <cc3:ClientSmartTextBox ID="ctvEmployee" InputMode="None" runat="server" Required="true" width="50px" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallEmployee('M')" style="cursor:pointer;" alt="" />
                            <span style = "display:none"><cc3:ClientSmartTextBox ID="ctvEmployeeCD" InputMode="None" runat="server" Required="true" width="200px" ReadOnly="true"></cc3:ClientSmartTextBox></span>
                            &nbsp;&nbsp
                            <span id="spanUpLayer" style="display:none"><img src="/DeskPlusEIP/Design/C/Img/Icon/icon_up.gif" onclick="AddMaster.UpLayer()" style="cursor:pointer;" alt="" /></span>
                            <span id="spanDownLayer"><img src="/DeskPlusEIP/Design/C/Img/Icon/icon_down.gif" onclick="AddMaster.DownLayer()" style="cursor:pointer;" alt="" /></span>
                        </td>
				    </tr> 
				    <tr id="modDes" style="display:none" >
                        <th style="font-weight:bold;">Description</th>
					    <td colspan="7"><cc3:ClientSmartTextBox ID="ctbDescription" InputMode="None" runat="server" Required="true" width="70%" ></cc3:ClientSmartTextBox>
                        <ul class="btn_right" id="CardLayerbtn1"> 
                        <li><a href="javascript:AddMaster.CardUpdate()"><span title="Description 수정이 가능 합니다.">기본정보변경</span></a></li> 
                        </ul>
                        </td>

				    </tr> 
			    </tbody>
		        </table>
            </div>

            <div class="gap"></div>

            <div class="tabAline">
                <ul class="tabA">
                    <li style="font-size:12px;"><a href="javascript:AddMaster.SetTab('C');" class="tab_on"  id="tabCard">법인카드</a></li>
                    <li style="font-size:12px;" class="last"><a href="javascript:AddMaster.SetTab('V');" id="tabVendor">벤더</a></li>
                </ul>
                <ul>   
                   <li>&nbsp;<span id="CardDate" action="card" style="vertical-align:middle;"><img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" style="cursor:pointer;" alt="" action="card" /></span>
                   <span id="CardPeriodSh" style="display:none"><cc3:ClientSmartTextBox ID="CardPerioFr" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true"></cc3:ClientSmartTextBox> ~ <cc3:ClientSmartTextBox ID="CardPerioTo" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true"></cc3:ClientSmartTextBox></span>
                   </li>       
                </ul>
                <ul class="btn_right">   
                   <li class="btn_blue"><a href="javascript:AddMaster.AnotherCard()"><span style="display:none" id="AnotherCard">▼ Another Card</span></a></li>       
                </ul>
            </div>

            <div class="gap"></div>
            <div class="master" id="CardLayer" style="display:inline-block;border-bottom:1px solid #efefef;border-right:1px solid #efefef;">

                <table cellspacing="0" class="table_typeC" style="width:100% !important;" border="0">  
                <colgroup>
                    <col width="30px" />
                    <col width="150px" />
                    <col width="70px" />
                    <col width="65px" />
                    <col width="65px" />
                    <col width="130px" />
                    <col  />
                </colgroup>
                <tr>
                    <th style="font-size:11px;font-weight:bold;"><input id="cbxAllCard" type="checkbox" onclick="AddMaster.CardChecked()" /></th>
                    <th style="font-size:11px;font-weight:bold;">사용장소</th>
                    <th style="font-size:11px;font-weight:bold;">일자</th>
                    <th style="font-size:11px;font-weight:bold;">승인금액</th>
                    <th style="font-size:11px;font-weight:bold;">신청금액</th>
                    <th style="font-size:11px;font-weight:bold;display:none">부가세</th>
                    <th style="font-size:11px;font-weight:bold;display:none">Tax Code</th>
                    <th style="font-size:11px;font-weight:bold;">G/L Account</th>
                    <th style="font-size:11px;font-weight:bold;">Description</th>
                </tr>  
                </table>
                <div style="display:inline-block;overflow-y:auto;min-height:200px;height:200px;">
                <!-- UpdatePannel 사용" -->
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater ID="RepeaterEx2" runat="server" OnItemDataBound="RepeaterEx2_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" class="table_typeC" style="width:100% !important;">  
                                <colgroup>
                                    <col width="30px" />
                                    <col width="150px" />
                                    <col width="70px" />
                                    <col width="65px" />
                                    <col width="65px" />
                                    <col width="130px" />
                                    <col  />
                                </colgroup>
                                <tbody id="dataContainerCard" > 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><input type="checkbox" class="cardCbxID" id="cardcheckbox<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" value="<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" /></td>
                                        <td style="font-size:11px;vertical-align:middle;">
                                            <%#SubjectNotLong(DataBinder.Eval(Container.DataItem, "VENDOR_NAME").ToString())%>
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <span id="approveDate<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "APPROVE_DATE").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;">
                                            <span id="approve_AMT<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "APPROVE_AMT").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">    
                                            <input type="text" id="CardReqAMT<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="width:85%;text-align:right;" value="<%#DataBinder.Eval(Container.DataItem, "APPROVE_AMT").ToString()%>" class="chkNum" />
                                            <input type="text" id="CardAMT<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:85%" value ="<%#DataBinder.Eval(Container.DataItem, "APPROVE_AMT").ToString()%>" />
                                            
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;display:none;">    
                                            <input type="text" id="CardTaxAMT<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="width:85%;text-align:right;" value ="0" class="chkNum" />
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;display:none;">
                                            <input type="text" id="TaxCardCD<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none" value ="00" />
                                            <span id="cardTax<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>">해당없음</span>
                                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="javascript:AddMaster.PopTaxItem('<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>')" style="cursor:pointer;padding:0px 0px 2px 0px;vertical-align:middle;" alt="" />
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <input type="text" id="GLAccountCardCD<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none" value ="" />
                                            <span id="cardGL<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>">G/L Account</span>
                                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="javascript:AddMaster.PopGLAccountItem('<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>')" style="cursor:pointer;padding:0px 0px 2px 0px;vertical-align:middle;" alt="" />
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <span><input type="text" id="DescriptionCard<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="width:90%" value ="" /></span>
                                            <!-- 히든정보들 처리-->
                                            <input type="text" id="ProductGroup<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:85%" value ="" />
                                            <input type="text" id="Etc1_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"VENDOR_NUM").ToString()%>" /> <!-- 사업장코드-->
                                            <input type="text" id="Etc2_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"VENDOR_NAME").ToString()%>" /><!-- 벤더명-->
                                            <input type="text" id="Etc3_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"VENDOR_PERSON").ToString()%>" /><!-- 대표자명-->
                                            <input type="text" id="Etc4_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"VENDOR_ADDRESS1").ToString()%> <%#DataBinder.Eval(Container.DataItem,"VENDOR_ADDRESS2").ToString()%>" /><!-- 사업장주소-->
                                            <input type="text" id="Etc5_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"APPROVE_DATE").ToString()%>" /><!-- 증빙일-->
                                            <input type="text" id="Etc6_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="<%#DataBinder.Eval(Container.DataItem,"APPROVE_TIME").ToString()%>" /><!-- 사용시간-->
                                            <input type="text" id="Etc7_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="" />
                                            <input type="text" id="Etc8_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="" />
                                            <input type="text" id="Etc9_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="" />
                                            <input type="text" id="Etc10_<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none;width:1%" value ="" /> 
                                            
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <SeparatorTemplate></SeparatorTemplate>
                            <FooterTemplate>
                                </tbody></table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LBTNProcess" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>                                           
                </div>
            </div>
            <div class="gap"></div>
            <div id="CardLayerbtn" style="0px;">   
                <ul class="btn_right">   
                   <li><a href="javascript:AddMaster.CardSave()"><span>▼ ADD</span></a></li>    
                     
                </ul>
            </div> 

            <div class="master" style="display:inline-block;width:100%;min-height:45px;border:0px solid #efefef;" id="ExpenseLayer">
                <table class="table_typeB">
			    <caption>Vendor Info</caption>
			    <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="13%" />
                    <col width="10%" />
                    <col width="13%" />
                    <col width="10%" />
                    <col width="13%" />
			    </colgroup>
			    <tbody>
				    <tr>
					    <th style="font-size:11px;font-weight:bold;">Vendor</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctbVendorCDEx" InputMode="None" runat="server" Required="true" width="40px" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <cc3:ClientSmartTextBox ID="ctbVendorNMEx" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallVendor('D')" style="cursor:pointer;" alt="" />
                        </td>
					    <th style="font-size:11px;font-weight:bold;">Documant Date</th>
					    <td><cc3:ClientSmartTextBox ID="ctbDocumentDateAdd" InputMode="None" runat="server" Required="true" width="90%" ReadOnly="true"></cc3:ClientSmartTextBox></td>
                        <th style="font-size:11px;font-weight:bold;">Due Date</th>
					    <td><cc3:ClientSmartTextBox ID="ctbDueDateAdd" InputMode="None" runat="server" Required="true" width="90%" ReadOnly="true"></cc3:ClientSmartTextBox></td>
                        <th style="font-weight:bold;">Posting Group</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctvPostingGroupAdd" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PostingGroup('D')" style="cursor:pointer;" alt="" />
                        </td>
				    </tr> 
				    <tr>
                        <th style="font-size:11px;font-weight:bold;">Total Amt</th>
                        <td><cc3:ClientSmartTextBox runat="server" ID="ctbAmountAdd" Width="90%" MaxLength="13" InputMode="None" Required="true" class="text required chkNum"></cc3:ClientSmartTextBox></td>					    
                        <th style="font-size:11px;font-weight:bold;">TaxCode</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctbTaxNameAdd" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true" Text="해당없음"></cc3:ClientSmartTextBox>
                            <span style="display:none"><cc3:ClientSmartTextBox ID="ctbTaxCodeAdd" InputMode="None" runat="server" Required="true" width="85%" ReadOnly="true" Text="00"></cc3:ClientSmartTextBox></span>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopTaxVendor('Tax')" style="cursor:pointer;" alt="" />
                        </td>
                        <th style="font-size:11px;font-weight:bold;">Tax Amt</th>
                        <td>
                            <cc3:ClientSmartTextBox runat="server" ID="ctbTaxAmountAdd" Width="90%" MaxLength="12" InputMode="None" Required="true" class="text required chkNum"></cc3:ClientSmartTextBox>
                            <span style="display:none"><cc3:ClientSmartTextBox runat="server" ID="ctbAmountAdd1" Width="90%" MaxLength="12" InputMode="None" Required="true"></cc3:ClientSmartTextBox></span>
                        </td>
                        <th style="font-size:11px;font-weight:bold;">G/L Account</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctbGLAccountNMAdd" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <span style="display:none"><cc3:ClientSmartTextBox ID="ctbGLAccountCDAdd" InputMode="None" runat="server" Required="true" width="95%" ReadOnly="true"></cc3:ClientSmartTextBox></span>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallGLAccount('GLAccount')" style="cursor:pointer;" alt="" />
                        </td>
				    </tr> 
                    <tr>
                        <th style="font-weight:bold;">Bank Account</th>
					    <td>
                            <cc3:ClientSmartTextBox ID="ctvBankAccountCDEx" InputMode="None" runat="server" Required="true" width="65%" ReadOnly="true"></cc3:ClientSmartTextBox>
                            <img src="/DeskPlusEIP/Design/C/Img/Icon/tree_search.gif" onclick="AddMaster.PopupCallBankAccount('D')" style="cursor:pointer;" alt="" />
                        </td>
                        <th style="font-weight:bold;">Description</th>
					    <td colspan="5"><cc3:ClientSmartTextBox ID="ctbDescriptionAdd" InputMode="None" runat="server" Required="true" width="95%" ></cc3:ClientSmartTextBox></td>
                    </tr>
			    </tbody>
		        </table>
                <div class="gap"></div>
                <ul class="btn_right">
                    <li><span id="ExpenseNew" title="새로운 문서를 생성합니다."><a href="#">◎ NEW</a></span></li> 
                    <li><span id="ExpenseUpdate" style="display:none"><a href="#">▼ UPDATE</a></span></li>
                    <li><span id="ExpenseReset" title="현재 문서에 추가합니다." style="display:none"><a href="#">○ Reset</a></span></li>   
                    <li><span id="ExpenseSave"><a href="#">▼ ADD</a></span></li>
                </ul>
            </div> 
            </div>  
            <div class="dotLine" style="padding:3px 0px;margin:0px;"></div>
            <div class="gap"></div>
            <div class="tabAline">
                <ul class="tabA">
                    <li style="font-size:12px;" class="last"><a href="#" class="tab_on"  id="List">List</a></li>
                </ul>
            </div>

            <div class="gap"></div>
            <div class="master" style="display:inline-block;border-bottom:1px solid #efefef;border-right:1px solid #efefef;overflow-x:auto;min-width:800px;width:100%;overflow-y:auto;min-height:200px;height:200px;">
                <!-- UpdatePannel 사용" -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater ID="RepeaterEx1" runat="server" OnItemDataBound="RepeaterEx_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" class="table_typeC" style="width:100% !important;">  
                            <colgroup>
                                <col width="30px" />
                                <!--<col width="60px" />-->
                                <col width="30px" />
                                <col width="30px" />
                                <col width="100px" />
                                <col width="80px" />
                                <col width="100px" />
                                <col width="70px" />
                                <col width="60px" />
                                <col width="60px" />
                                <col width="150px" />
                                <col width="200px" />
                                <col width="90px" />
                                <col width="120px" />
                                <col width="90px"/>
                                <col width="90px"/>
                                <col width="90px"/>
                                <col width="90px"/>
                                <col width="90px"/>
                            </colgroup>
                                <tr>
                                    <th style="font-size:11px;font-weight:bold;"><input id="cbxAll" type="checkbox" onclick="AddMaster.ExChecked()" /></th>
                                    <!--<th style="font-size:11px;font-weight:bold;"></th>-->
                                    <th style="font-size:11px;font-weight:bold;"><img src="/DeskPlusEIP/Design/C/Img/Icon/trash.gif" onclick="AddMaster.AllDelItem()" style="cursor:pointer;" alt="" /></th>
                                    <th style="font-size:11px;font-weight:bold;"></th>
                                    <th style="font-size:11px;font-weight:bold;">Voucher No</th>
                                    <th style="font-size:11px;font-weight:bold;">Vendor</th>
                                    <th style="font-size:11px;font-weight:bold;">Document Date</th>
                                    <th style="font-size:11px;font-weight:bold;">Total Amt.</th>
                                    <th style="font-size:11px;font-weight:bold;">Amt.</th>
                                    <th style="font-size:11px;font-weight:bold;">Tax Amt.</th>
                                    <th style="font-size:11px;font-weight:bold;">G/L Account</th>
                                    <th style="font-size:11px;font-weight:bold;">Description</th>
                                    <th style="font-size:11px;font-weight:bold;">Bank Account</th>
                                    <th style="font-size:11px;font-weight:bold;">Bank Account No</th>
                                    <th style="font-size:11px;font-weight:bold;">Due Date</th>
                                    <th style="font-size:11px;font-weight:bold;">Posting Group</th>
                                    <th style="font-size:11px;font-weight:bold;">DepartMent</th>
                                    <th style="font-size:11px;font-weight:bold;">Employee</th>
                                    <th style="font-size:11px;font-weight:bold;">Product Group</th>     
                                </tr>  
                                <tbody id="dataContainer"> 
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr  class="exVoucher" style="background:<%#VoucherColor(DataBinder.Eval(Container.DataItem, "VoucherNo").ToString())%>" onclick="AddMaster.RowClick('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Type").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')" >
                                    <td style="font-size:11px;text-align:center;"><input type="checkbox" class="exCbxID" id="Excheckbox<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" value="<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"  onclick="AddMaster.VoucherCheck('Excheckbox<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>')" /></td>
                                    <td style="font-size:11px;text-align:center;display:none">
                                        <img src="/DeskPlusEIP/Design/C/Img/Icon/icon_up.gif" onclick="AddMaster.UpItem('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>')" style="cursor:pointer;" alt="" />&nbsp;
                                        <img src="/DeskPlusEIP/Design/C/Img/Icon/icon_Down.gif" onclick="AddMaster.DownItem('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>')" style="cursor:pointer;" alt="" />
                                    </td>
                                    <td style="font-size:11px;text-align:center;">
                                        <img src="/DeskPlusEIP/Design/C/Img/Icon/ico_tree_listDel.gif" onclick="AddMaster.DelItem('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CardApprovalNo").ToString()%>')" style="cursor:pointer;" alt="" />
                                        <span id="ExDimension3Code<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "ExDimension3Code").ToString()%></span>
                                        <span id="TaxCode<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "TaxCode").ToString()%></span>
                                        <span id="GLAccount<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "GLAccount").ToString()%></span>
                                        <span id="ExDimension4Code<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "ExDimension4Code").ToString()%></span>
                                        <span id="ExBankClearingID<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCearing").ToString()%></span>
                                        <span id="Description<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "VendorDescription").ToString()%></span>
                                        <span id="ExDimension1Code<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "ExDimension1Code").ToString()%></span>
                                        <span id="ExDimension2Code<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><%#DataBinder.Eval(Container.DataItem, "ExDimension2Code").ToString()%></span>
                                    </td>
                                    <td style="font-size:11px;text-align:center;">
                                        <img src="<%#ImageType(DataBinder.Eval(Container.DataItem, "Type").ToString())%>"  style="cursor:pointer;" alt="" />
                                    </td>
                                    <td style="font-size:11px;text-align:center;"><span id="VoucherNo<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;display:none"><span id="Type<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "Type").ToString()%></span></td>
                                    <td style="font-size:11px;">
                                        <span id="ExVendorNM<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" value="<%#DataBinder.Eval(Container.DataItem, "ExVendorNM").ToString()%>"><%#FormatExVendorNM(DataBinder.Eval(Container.DataItem, "ExVendorNM").ToString())%></span>
                                    </td>
                                    <td style="font-size:11px;text-align:center;"><span id="SpanDocumentDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DocumentDate").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:right;">
                                        <span id="CreditExLevel<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%></span>
                                    </td>
                                    <td style="font-size:11px;text-align:right;">
                                        <span id="DebitAmount1<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DebitAmount1").ToString()%></span>
                                    </td>
                                    <td style="font-size:11px;text-align:right;">
                                        <span id="Debit2ExLevel<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DebitAmount2").ToString()%></span>
                                    </td>
                                    <td style="font-size:11px;text-align:left;"><span id="SpanGLAccountNM<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "GLAccountNM").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:left;">
                                        <span id="ExDescription<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "ExDescription").ToString()%></span>
                                    </td>
                                    <td style="font-size:11px;display:none;"><span id="SpanTaxCodeNM<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "TaxCodeNM").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="ExBankAccountCode<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCode").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="ExBankAccountNo<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "ExBankAccountNo").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="SpanDueDate<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="ExPostingGroup<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "ExPostingGroup").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="DepartMentNM<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "etc9").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span id="EmpNM<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "etc10").ToString()%></span></td>
                                    <td style="font-size:11px;text-align:center;"><span><%#DataBinder.Eval(Container.DataItem, "etc7").ToString()%></span></td>
                                </tr>
                            </ItemTemplate>
                            <SeparatorTemplate>
                            </SeparatorTemplate>
                            <FooterTemplate>
                                </tbody></table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="LBTNProcess" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>                           
            </div>

            <!-- 상세내역 //-->
            <div class="detail" style="display:inline-block;width:100%;" title="상세내역">
                <div style="clear:both;width:100%;float:left;margin:0px;padding:5px 0px;">
                    <ul>
                        <li>Selected Amount : <cc3:ClientSmartTextBox ID="SelectedAmount" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox>&nbsp;&nbsp;
                            Document Amount : <cc3:ClientSmartTextBox ID="DocumentAmount" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox>&nbsp;&nbsp; 
                            Total Amount : <cc3:ClientSmartTextBox ID="TotalAmount" InputMode="None" runat="server" Required="true" width="70px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox> 
                        </li>
                    </ul>
                    <ul class="btn_right">
                        <li><a href="#"><span id ="draft" >임시저장</span></a></li>
                        <li class="btn_blue"><a href="#"><span id ="approvalSubmit" >결재승인</span></a></li>     
                    </ul>
                </div>
            </div>

        </div>
    </div>

    <div style="display: none">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton> 
        <asp:LinkButton ID="LBTNProcessCard" runat="server" OnClick="LBTNProcessCard_Click"></asp:LinkButton>  <!-- 사용안함-->
        <asp:TextBox ID="TmpData" runat="server" Width="1px"></asp:TextBox>  
        <asp:TextBox ID="TmpVoucherData" runat="server" Width="1px"></asp:TextBox>  
        <asp:TextBox ID="ctbDepartMentOld" runat="server" Width="1px"></asp:TextBox>  
        <asp:TextBox ID="ctbDepartMentOldCD" runat="server" Width="1px"></asp:TextBox>  
        <asp:TextBox ID="ctbEmployeeOld" runat="server" Width="1px"></asp:TextBox>  
        <asp:TextBox ID="ctbEmployeeOldCD" runat="server" Width="1px"></asp:TextBox>  
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
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.url.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.datepick.js"></script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/JModal/DeskPlusModal.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jtemplates.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.list.pager.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/validator.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/DeskPlusEIPComponent.js"></script>
    <script type="text/javascript">
        var ImagePath = "<%=BaseImgPath%>";
        var PreLang = "<%=PreLang%>";
        var PreLangReal = "<%=PreLangReal %>"; //ja  
        var UserID = "<%=CurrentUserInfo.UserID %>";
        // 정산 필요 변수 정의
        var VendorCD = "<%=VendorCD %>";
        var EmpCD = "<%=VendorCD %>"; // 사용자 사번 벤더랑 동일하나 사번 필드로써 추후 벤더가 바뀌더라도 이건 바뀌지 않음 
        var VendorNM = "<%=VendorNM %>";
        var VendorPostingGroup = "<%=VendorPostingGroup %>";
        var BankAccountCD = "<%=BankAccountCD %>";
        var ErpUserNM = "<%=ErpUserNM %>";
        var DepartMentDimensionCD = "<%=DepartMentDimensionCD %>";
        var BankClearingID = "<%=BankClearingID %>";
        var BankAccountNo = "<%=BankAccountNo %>";
        var ExVendorCD = "<%=VendorCD %>";
        var ExVendorNM = "<%=VendorNM %>";
        var ExVendorPostingGroup = "<%=VendorPostingGroup %>";
        var ExBankAccountCD = "<%=BankAccountCD %>";
        var ExBankClearingID = "<%=BankClearingID %>";
        var ExBankAccountNo = "<%=BankAccountNo %>";
        var DocumentNo = "<%=VoucherNo %>"; // 바우처 넘버 (그룹키로 사용)
        var DocumentNoEx = "<%=VoucherNo %>"; // 바우처 넘버 (그룹키로 사용)
        var CardExpenseKind = "C";
        var LegacyKey = "<%=LegacyKey %>";
        var ExUseValidation = "<%=ExUseValidation %>";
        var TargetCardNo = "<%=TargetCardNo %>";          // 기본 카드
        var TargetEtcCardNo = "<%=TargetEtcCardNo %>";    // 추가카드 (관리자)
        var TargetCardKind = "<%=TargetCardKind %>";      // 기본 추가 구분 (P,C)
        var ProductGroup = "";
        var ProductGroupNM = "";
        var ProductGroupStandard = "<%=ProductGroupStandard %>";
        var PaymentTerm = "<%=PaymentTerm %>";
        var ExPaymentTerm = "<%=PaymentTerm %>";
        var DueDateTemp = "<%=DueDate %>";
        var SelectedSeq = "";
        var CardDate = "<%=CardDate %>";
        var IsDel = "";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/AddMaster.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/Common.js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/AddMaster.js");

    </script>
</body>
</html>
