<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListAdmin.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.Admin.ListAdmin" %>
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
        <div class="layout_full">

            <div id="layout_header">
                <h1>Expense Confirm (<span id="templeteNM"></span>-<span id="batchNM"></span>)</h1>
                <ul class="btn_right">
                    <li><a href="" title=""><span id="spanSubmit">승인</span></a></li>
                </ul>
                <ul class="btn_left">
                    <li><a href="#"><span id ="spanBatch" >Change Batch</span></a>        
                    </li>
                </ul>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect"></asp:DropDownList></li>  
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn" Text="검색"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_content">

            <div class="tabAline">
                <ul class="tabA">
                    <li style="font-size:12px;"><a href="javascript:ListAdmin.SetTab('I');" class="tab_on"  id="tabIng">승인대기</a></li>
                    <li style="font-size:12px;" class="last"><a href="javascript:ListAdmin.SetTab('C');" id="tabEnd">승인완료</a></li>
                </ul>
            </div>
            <div class="gap"></div>
            <div class="master" id="IngLayer" style="display:inline-block;border:0px solid #efefef;overflow-x:auto;min-width:800px;width:100%;overflow-y:auto;min-height:250px;height:250px;">
                    <!-- UpdatePannel 사용" -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Repeater ID="RepeaterEx1" runat="server" OnItemDataBound="RepeaterEx_ItemDataBound">
                                <HeaderTemplate>
                                    <table cellspacing="0" class="table_typeC" style="width:100% !important;" border="0">  
                                <colgroup>
                                    <col width="30px" />
                                    <col width="40px"/>
                                    <col width="90px" />
                                    <col width="60px" />
                                    <col width="200px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="85px" />
                                    <col width="80px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="120px" />
                                    <col width="120px" />
                                    <col width="140px" />
                                    <col width="80px" />                         
                                </colgroup>
                                    <tr>
                                        <th style="font-size:11px;font-weight:bold;"><input id="cbxAll" type="checkbox" onclick="ListAdmin.ExChecked()" /></th>
                                        <th style="font-size:11px;font-weight:bold;"></th>
                                        <th style="font-size:11px;font-weight:bold;">Voucher No</th>
                                        <th style="font-size:11px;font-weight:bold;">Type</th>
                                        <th style="font-size:11px;font-weight:bold;">Vendor</th>
                                        <th style="font-size:11px;font-weight:bold;">Posting Date</th>
                                        
                                        <th style="font-size:11px;font-weight:bold;">Total Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Tax Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Due Date</th>
                                        <th style="font-size:11px;font-weight:bold;">Posting Group</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account Code</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account No</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account Cearing</th>
                                        <th style="font-size:11px;font-weight:bold;">Document ID</th>
                                    </tr>  
                                    <tbody id="dataContainer"> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><input type="checkbox" class="exCbxID" id="Excheckbox<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>" value="<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>" onclick="ListAdmin.SelectedAmount()"  /></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <img src="/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif" onclick="ListAdmin.AppPopupView('<%#DataBinder.Eval(Container.DataItem, "documentid").ToString()%>')" style="cursor:pointer;" alt="" />
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "Type").ToString()%></span></td>
                                        <td style="font-size:11px;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExVendorNM").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "PostingDate").ToString()%></span></td>
                                        
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span id="CreditExLevel<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span><%#DataBinder.Eval(Container.DataItem, "DebitAmount1").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span><%#DataBinder.Eval(Container.DataItem, "DebitAmount2").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExPostingGroup").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCode").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountNo").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCearing").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "documentid").ToString()%></span></td>

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
                <div class="master" id="EndLayer" style="display:none;border:0px solid #efefef;overflow-x:auto;min-width:800px;width:100%;overflow-y:auto;min-height:250px;height:250px;">
                    <!-- UpdatePannel 사용" -->
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Repeater ID="RepeaterEx3" runat="server" OnItemDataBound="RepeaterEx_ItemDataBound">
                                <HeaderTemplate>
                                    <table cellspacing="0" class="table_typeC" style="width:100% !important;" border="0">  
                                <colgroup>
                                    <col width="40px"/>
                                    <col width="90px" />
                                    <col width="60px" />
                                    <col width="200px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="85px" />
                                    <col width="80px" />
                                    <col width="100px" />
                                    <col width="100px" />
                                    <col width="120px" />
                                    <col width="120px" />
                                    <col width="140px" />
                                    <col width="80px" />                         
                                </colgroup>
                                    <tr>
                                        <th style="font-size:11px;font-weight:bold;"></th>
                                        <th style="font-size:11px;font-weight:bold;">Voucher No</th>
                                        <th style="font-size:11px;font-weight:bold;">Type</th>
                                        <th style="font-size:11px;font-weight:bold;">Vendor</th>
                                        <th style="font-size:11px;font-weight:bold;">Posting Date</th>
                                        
                                        <th style="font-size:11px;font-weight:bold;">Total Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Tax Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Due Date</th>
                                        <th style="font-size:11px;font-weight:bold;">Posting Group</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account Code</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account No</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account Cearing</th>
                                        <th style="font-size:11px;font-weight:bold;">Document ID</th>
                                    </tr>  
                                    <tbody id="dataContainer"> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <img src="/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif" onclick="ListAdmin.AppPopupView('<%#DataBinder.Eval(Container.DataItem, "documentid").ToString()%>')" style="cursor:pointer;" alt="" />
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "Type").ToString()%></span></td>
                                        <td style="font-size:11px;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExVendorNM").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "PostingDate").ToString()%></span></td>
                                        
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span id="CreditExLevel<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span><%#DataBinder.Eval(Container.DataItem, "DebitAmount1").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')">
                                            <span><%#DataBinder.Eval(Container.DataItem, "DebitAmount2").ToString()%></span>
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExPostingGroup").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCode").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountNo").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCearing").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;" onclick="ListAdmin.GetDetail('<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "ExVendorCD").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>')"><span><%#DataBinder.Eval(Container.DataItem, "documentid").ToString()%></span></td>

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

                <!-- 테이블 끝 //-->


            <div class="gap"></div>
            <div class="dotLine" style="padding:3px 0px;margin:0px;"></div>
            <div class="gap"></div>
            <div class="master" id="CardLayer" style="display:inline-block;border-bottom:1px solid #efefef;border-right:1px solid #efefef;overflow-x:auto;min-width:800px;width:100%;overflow-y:auto;min-height:200px;height:200px;">
                <!-- UpdatePannel 사용" -->
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Repeater ID="RepeaterEx2" runat="server" OnItemDataBound="RepeaterEx2_ItemDataBound">
                            <HeaderTemplate>
                                <table cellspacing="0" class="table_typeC" style="width:100% !important;">  
                                <colgroup>
                                    <col width="90px" />
                                    <col width="90px" />
                                    <col width="95px" />
                                    <col width="80px" />
                                    <col width="80px" />
                                    <col width="200px" />
                                    <col width="90px" />
                                    <col width="130px" />
                                    <col width="90px" />
                                    <col width="90px" />
                                    <col width="110px" />
                                    <col width="100px" />
                                    <col width="130px"/>
                                    <col width="110px"/>
                                    <col width="100px"/>
                                    <col width="100px"/>
                                    <col width="200px"/>
                                    <col width="100px"/>
                                    <col width="70px" />
                                </colgroup>
                                <tr>
                                    <th style="font-size:11px;font-weight:bold;">Voucher No</th>
                                    <th style="font-size:11px;font-weight:bold;">Posting Date</th>
                                    <th style="font-size:11px;font-weight:bold;">Document Date</th>
                                    <th style="font-size:11px;font-weight:bold;">Due Date</th>
                                    <th style="font-size:11px;font-weight:bold;">Account No</th>
                                    <th style="font-size:11px;font-weight:bold;">Account Name</th>
                                    <th style="font-size:11px;font-weight:bold;">Posting Group</th>
                                    <th style="font-size:11px;font-weight:bold;">Description</th>
                                    <th style="font-size:11px;font-weight:bold;">Debit Amount</th>
                                    <th style="font-size:11px;font-weight:bold;">Credit Amount</th>
                                    <th style="font-size:11px;font-weight:bold;">BankAcount Code</th>
                                    <th style="font-size:11px;font-weight:bold;">BankAccount No</th>
                                    <th style="font-size:11px;font-weight:bold;">BankAcount CearingID</th>
                                    <th style="font-size:11px;font-weight:bold;">Department Code</th>
                                    <th style="font-size:11px;font-weight:bold;">Employee Code</th>
                                    <th style="font-size:11px;font-weight:bold;">Vendor Code</th>
                                    <th style="font-size:11px;font-weight:bold;">Vendor Name</th>
                                    <th style="font-size:11px;font-weight:bold;">Product Group</th>
                                    <th style="font-size:11px;font-weight:bold;">Line No</th>
                                </tr>  
                                <tbody id="dataContainerCard" > 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;"><%#DataBinder.Eval(Container.DataItem, "DocumentNo").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.PostingDateClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id="LvPostingDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "PostingDate").ToString()%></span>
                                        <input type="text" onchange="ListAdmin.ChangePostingDate('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "DocumentNo").ToString()%>')" id="InPostingDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%" value ="<%#DataBinder.Eval(Container.DataItem, "PostingDate").ToString()%>" />
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.DocumentDateClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id="LvDocumentDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DocumentDate").ToString()%></span>
                                        <input type="text" onchange="ListAdmin.ChangeDocumentDate('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "DocumentNo").ToString()%>')" id="InDocumentDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%" value ="<%#DataBinder.Eval(Container.DataItem, "DocumentDate").ToString()%>" />
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.DueDateClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id="LvDueDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%></span>
                                        <input type="text" onchange="ListAdmin.ChangeDueDate('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "DocumentNo").ToString()%>')" id="InDueDate<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%" value ="<%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%>" />
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.GLAccountClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "AccountNo").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.GLAccountClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "AccountNM").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.PostingGroupClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "PostingGroup").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;" onclick="ListAdmin.DesClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id="LvDes<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "Description").ToString()%></span>
                                        <input type="text" id="InDes<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%" value ="<%#DataBinder.Eval(Container.DataItem, "Description").ToString()%>" />
                                        </td> 
                                        <td style="font-size:11px;vertical-align:middle;text-align:right;" onclick="ListAdmin.DebitAmountClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id ="LvDebit<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#MoneyString(DataBinder.Eval(Container.DataItem, "DebitAmount").ToString())%></span>
                                        <input type="text" id="InDebit<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%;text-align:right;" value ="<%#MoneyString(DataBinder.Eval(Container.DataItem, "DebitAmount").ToString())%>"  class="chkNum" />
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:right;" onclick="ListAdmin.CreditAmountClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')">
                                        <span id ="LvCredit<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>"><%#MoneyString(DataBinder.Eval(Container.DataItem, "CreditAmount").ToString())%></span>
                                        <input type="text" id="InCredit<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>" style="display:none;width:80%;text-align:right;" value ="<%#MoneyString(DataBinder.Eval(Container.DataItem, "CreditAmount").ToString())%>" class="chkNum" />
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.BankClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "BankAcountCode").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;" onclick="ListAdmin.BankClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "BankAccountNo").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.BankClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "BankAcountCearingID").ToString()%></td> 
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.DepartMentClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "Dimension1Code").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.EmployeeClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "Dimension2Code").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.VendorClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "Dimension3Code").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.VendorClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "VendorNM").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;" onclick="ListAdmin.ProductGroupClick('<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "Kind").ToString()%>_<%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%>')"><%#DataBinder.Eval(Container.DataItem, "Dimension4Code").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;text-align:center;"><%#DataBinder.Eval(Container.DataItem, "LineNo").ToString()%></td>
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
            <div class="detail" style="display:inline-block;width:100%;" title="Change Batch">
                <div style="clear:both;width:100%;float:left;margin:0px;padding:5px 0px;">
                    <table width="100%" border="0">
                        <tr>
                            <td align ="left">Selected Amount : <cc3:ClientSmartTextBox ID="SelectedAmount" InputMode="None" runat="server" Required="true" width="80px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox>&nbsp;&nbsp;
                            Document Amount : <cc3:ClientSmartTextBox ID="DocumentAmount" InputMode="None" runat="server" Required="true" width="80px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox>&nbsp;&nbsp; 
                            Total Amount : <cc3:ClientSmartTextBox ID="TotalAmount" InputMode="None" runat="server" Required="true" width="80px" ReadOnly="true" class="text required chkNum"></cc3:ClientSmartTextBox> 
                            </td>
                        </tr>
                    </table>
                </div>
                <ul class="btn_right">
                    <li class="btn_red"><a href="" title=""><span id="spanReject">반려</span></a></li>
                </ul>
            </div>
            </div>    

        </div>
    </div>
    <div style="display: none">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton> 
        <asp:TextBox ID="TmpData" runat="server" Width="1px"></asp:TextBox>      
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
        var CurrentPage = "1"; //현재페이지
        var TotalItemCount = 0; //게시물수
        var RecordPerPage = "<%=RecordPerPage %>"; //보여줄게시물수        
        var R_Previous = "<%=GetResource("Previous") %>";
        var R_Next = "<%=GetResource("Next") %>";
        var R_NoContent = "<%=GetResource("NoContent") %>";  
        var SelVoucher = "";
        var SelVendor = "";
        var SelCardVendorKind = "";
        var SelVal = "";
        var TempleteName = "<%=TempleteName%>";
        var BatchName = "<%=BatchName%>";
        var IsTab = "I";
        var isScroll = "";

        var R_PlaceholderMenu =  "<%=GetResource("PlaceholderMenu")%>";
        var R_PlaceholderText =  "<%=GetResource("PlaceholderText")%>";
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/Common.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/Admin/ListAdmin.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/Common.js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/Admin/ListAdmin.js");
    </script>
</body>
</html>
