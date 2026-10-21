<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListUser.aspx.cs" Inherits="Zeospace.DeskPlus.ETC.Web.Customize.User.ListUser" %>
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
                <h1>비용 정산 리스트</h1>
            </div>

            <div id="layout_srh">
                <ul class="btn_searchbox">
                    <li><div id="loading"></div></li>
                    <!--
                    <li>
                        <div class="searchterm">
                            <asp:DropDownList ID="ddlSearchTerm" runat="server" CssClass="searchselect">
                                <asp:ListItem Value=""></asp:ListItem>
                                <asp:ListItem Value="week"></asp:ListItem>
                                <asp:ListItem Value="month"></asp:ListItem>
                                <asp:ListItem Value="period"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="startEndDate" class="searchterm">
                            <input type="text" id="startDate" size="10" class="clsDate" />-<input type="text" id="endDate" size="10" class="clsDate" />
                        </div>
                    </li>
                    -->
                    <li><asp:DropDownList ID="ddlSearchField" runat="server" CssClass="searchselect"></asp:DropDownList></li>  
                    <li>
                        <input type="text" id="dummy" style="display: none;" />
                        <cc3:ClientSmartTextBox runat="server" ID="ctbSearch" Width="200px" InputMode="None" class="searchtext" placeholder="검색어 입력"></cc3:ClientSmartTextBox>
                    </li>
                <li><cc3:ClientSmartButton ID="cbtnSearch" runat="server" Class="searchbtn" Text="검색"></cc3:ClientSmartButton></li>
                </ul>
            </div>

            <div id="layout_content">

            <div class="master" style="display:inline-block;width:100%;min-height:50px;border:0px solid #efefef;">
                    <!-- UpdatePannel 사용" -->
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Repeater ID="RepeaterEx1" runat="server" OnItemDataBound="RepeaterEx_ItemDataBound">
                                <HeaderTemplate>
                                    <table cellspacing="0" class="table_typeC" style="width:100% !important;">  
                                <colgroup>
                                    <col width="30px" />
                                    <col width="85px" />
                                    <col width="60px" />
                                    <col width="140px" />
                                    <col width="80px" />
                                    <col width="80px" />
                                    <col />
                                    <col width="85px" />
                                    <col width="80px" />
                                    <col width="60px" />
                                    <col width="70px" />
                                    <col width="80px" />
                                    <col width="40px"/>
                                </colgroup>
                                    <tr>
                                        <th style="font-size:11px;font-weight:bold;"><input id="cbxAll" type="checkbox" onclick="ListUser.ExChecked()" /></th>
                                        <th style="font-size:11px;font-weight:bold;">Voucher No</th>
                                        <th style="font-size:11px;font-weight:bold;">Type</th>
                                        <th style="font-size:11px;font-weight:bold;">Vendor</th>
                                        <th style="font-size:11px;font-weight:bold;">Document Date</th>
                                        <th style="font-size:11px;font-weight:bold;">Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">G/L Account</th>
                                        <th style="font-size:11px;font-weight:bold;">Tax Code</th>
                                        <th style="font-size:11px;font-weight:bold;">Tax Amount</th>
                                        <th style="font-size:11px;font-weight:bold;">Bank Account</th>
                                        <th style="font-size:11px;font-weight:bold;">Due Date</th>
                                        <th style="font-size:11px;font-weight:bold;">진행상태</th>
                                        <th style="font-size:11px;font-weight:bold;"></th>
                                    </tr>  
                                    <tbody id="dataContainer"> 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><input type="checkbox" class="exCbxID" id="Excheckbox<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" value="<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"  onclick="ListUser.VoucherCheck('Excheckbox<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%>','<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>')" /></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span id="VoucherNo<%#DataBinder.Eval(Container.DataItem, "seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "VoucherNo").ToString()%></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "Type").ToString()%></td>
                                        <td style="font-size:11px;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "ExVendorNM").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "DocumentDate").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;">
                                            <span id="CreditExLevel<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%></span>
                                            <span id="CreditExInput<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><input type="text" id="CreditInput<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="width:90%" value ="<%#DataBinder.Eval(Container.DataItem, "CreditAmount").ToString()%>"/></span>
                                        </td>
                                        <td style="font-size:11px;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "GLAccountNM").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "TaxCodeNM").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:right;vertical-align:middle;">
                                            <span id="Debit2ExLevel<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>"><%#DataBinder.Eval(Container.DataItem, "DebitAmount2").ToString()%></span>
                                            <span id="Debit2ExInput<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="display:none"><input type="text" id="Debit2Input<%#DataBinder.Eval(Container.DataItem,"seq").ToString()%>" style="width:90%" value ="<%#DataBinder.Eval(Container.DataItem, "DebitAmount2").ToString()%>" /></span>
                                        </td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "ExBankAccountCode").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "DueDate").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;"><span><%#DataBinder.Eval(Container.DataItem, "Process").ToString()%></span></td>
                                        <td style="font-size:11px;text-align:center;vertical-align:middle;">
                                            <img src="/DeskPlusEIP/Design/C/Img/Icon/doc_view.gif" onclick="ListUser.AppPopupView('<%#DataBinder.Eval(Container.DataItem, "documentid").ToString()%>')" style="cursor:pointer;" alt="" />
                                        </td>
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



            </div>    

        </div>
    </div>
    <div style="display: none">
        <asp:LinkButton ID="LBTNProcess" runat="server" OnClick="LBTNProcess_Click"></asp:LinkButton> 
        <asp:TextBox ID="TmpData" runat="server" Width="1px"></asp:TextBox>      
    </div>  

    </form>
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
    </script>    
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-<%=PreLangReal%>.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/ETC/Js/Customize/User/ListUser.js"></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Html/Js/jQuery/jquery.ui.datepicker-" + PreLangReal + ".js");
        //JsCall("/DeskPlusEIP/ETC/Js/Customize/User/ListUser.js");
    </script>
</body>
</html>
