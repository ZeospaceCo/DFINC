<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Title.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Admin.Function.Skin.Title" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/DeskPlusEIPBase.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/DeskPlusEIPBase.css"/>
    <link rel="stylesheet" type="text/css" href="<%=BaseCssPath%>/Func_Community.css"/><link rel="stylesheet" type="text/css" href="<%=CssPath%>/Func_Community.css"/>    
</head>
<body>
    <form id="form1" runat="server">
<div id="layout_body">

    <div class="layout_full">

        <div id="layout_header">
            <!-- 페이지타이틀 //-->
            <h1><%=GetResource("SkinManager")%></h1>
            <!-- 버튼 //-->
            <span class="cm_subtxt">* <%=GetResource("MainTitleDes")%></span>
            <ul class="btn_right">
                <li><cc3:ClientSmartButton ID="cbtnConfirm" runat="server" Class="btn btn-secondary"></cc3:ClientSmartButton></li>
            </ul>
        </div>
        
        <div id="layout_content" class="cm_skin">
            <div class="row">
               <div class="col-12 content-area-body"><!-- width 100%(col-12) -->

                    <div class="layout_srh clearfix"> 
                        <div class="float-left">
                            <ul id="ulTabMenu" class="tab">
                                <li class="selected"><a><%=GetResource("TabTitle")%></a></li>
                                <li><a onclick="Title.goUrl();" ><%=GetResource("Menu")%></a></li>
                            </ul>
                        </div>
                    </div>

                    <div class="cm_gap5"></div>
                    <h2 style="font-weight:bold;"><%=GetResource("Preview")%></h2>
                    <div class="preview">
                        <div class="header" id="myCommunityTitleBG">
                            <div class="gap">
                                <span class="tit" id="myCommunityTitleColor1"><%=GetResource("Skin5")%> </span><!--span class="cm_tit_fontA">새싹</span-->
                                <ul class="info">
                                    <li><span class="txtbul" id="myCommunityTitleColor2"><%=GetResource("Skin6")%></span></li>
                                    <li><span class="txtbul" id="myCommunityTitleColor3"><%=GetResource("Skin7")%></span></li>
                                    <li><span class="txtbul" id="myCommunityTitleColor4"><%=GetResource("Skin8")%></span></li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <ul class="sett">
                        <li>
                            <div class="cm_gap"></div>
                            <h2><%=GetResource("Color")%></h2>
                            <!-- 색상표 //-->
                            <table id="colorPicker1" cellspacing="1" cellpadding="0" border="0" class="color">
                            <tbody>
                            <tr>
	                            <td id="FF0000T" onClick="Title.setTitleColorValue('#FF0000')" style="background:#FF0000;" title="#FF0000"></td>
	                            <td id="FFFF00T" onClick="Title.setTitleColorValue('#FFFF00')" style="background:#FFFF00;" title="#FFFF00"></td>
	                            <td id="00FF00T" onClick="Title.setTitleColorValue('#00FF00')" style="background:#00FF00;" title="#00FF00"></td>
	                            <td id="00FFFFT" onClick="Title.setTitleColorValue('#00FFFF')" style="background:#00FFFF;" title="#00FFFF"></td>
	                            <td id="0000FFT" onClick="Title.setTitleColorValue('#0000FF')" style="background:#0000FF;" title="#0000FF"></td>
	                            <td id="FF00FFT" onClick="Title.setTitleColorValue('#FF00FF')" style="background:#FF00FF;" title="#FF00FF"></td>
	                            <td id="FFFFFFT" onClick="Title.setTitleColorValue('#FFFFFF')" style="background:#FFFFFF;" title="#FFFFFF"></td>
                                <td id="E4F2FFT" onClick="Title.setTitleColorValue('#E4F2FF')" style="background:#E4F2FF;" title="#E4F2FF"></td>
	                            <td id="E6E6E6T" onClick="Title.setTitleColorValue('#E9EBF3')" style="background:#E9EBF3;" title="#E9EBF3"></td>	                    
	                            <td id="CCCCCCT" onClick="Title.setTitleColorValue('#CCCCCC')" style="background:#CCCCCC;" title="#CCCCCC"></td>
	                            <td id="BFBFBFT" onClick="Title.setTitleColorValue('#BFBFBF')" style="background:#BFBFBF;" title="#BFBFBF"></td>
	                            <td id="B3B3B3T" onClick="Title.setTitleColorValue('#B3B3B3')" style="background:#B3B3B3;" title="#B3B3B3"></td>
	                            <td id="A6A6A6T" onClick="Title.setTitleColorValue('#A6A6A6')" style="background:#A6A6A6;" title="#A6A6A6"></td>
	                            <td id="999999T" onClick="Title.setTitleColorValue('#999999')" style="background:#999999;" title="#999999"></td>
	                            <td id="8C8C8CT" onClick="Title.setTitleColorValue('#8C8C8C')" style="background:#8C8C8C;" title="#8C8C8C"></td>
	                            <td id="808080T" onClick="Title.setTitleColorValue('#808080')" style="background:#808080;" title="#808080"></td>
                            </tr>
                            <tr>
	                            <td id="DF0024T" onClick="Title.setTitleColorValue('#DF0024')" style="background:#DF0024;" title="#DF0024"></td>
	                            <td id="F9F400T" onClick="Title.setTitleColorValue('#F9F400')" style="background:#F9F400;" title="#F9F400"></td>
	                            <td id="009F62T" onClick="Title.setTitleColorValue('#009F62')" style="background:#009F62;" title="#009F62"></td>
	                            <td id="00B2EBT" onClick="Title.setTitleColorValue('#00B2EB')" style="background:#00B2EB;" title="#00B2EB"></td>
	                            <td id="3D117BT" onClick="Title.setTitleColorValue('#3D117B')" style="background:#3D117B;" title="#3D117B"></td>
	                            <td id="E3007BT" onClick="Title.setTitleColorValue('#E3007B')" style="background:#E3007B;" title="#E3007B"></td>
	                            <td id="737373T" onClick="Title.setTitleColorValue('#737373')" style="background:#737373;" title="#737373"></td>
	                            <td id="666666T" onClick="Title.setTitleColorValue('#666666')" style="background:#666666;" title="#666666"></td>
	                            <td id="595959T" onClick="Title.setTitleColorValue('#595959')" style="background:#595959;" title="#595959"></td>
	                            <td id="4D4D4DT" onClick="Title.setTitleColorValue('#4D4D4D')" style="background:#4D4D4D;" title="#4D4D4D"></td>
	                            <td id="404040T" onClick="Title.setTitleColorValue('#404040')" style="background:#404040;" title="#404040"></td>
	                            <td id="333333T" onClick="Title.setTitleColorValue('#333333')" style="background:#333333;" title="#333333"></td>
	                            <td id="262626T" onClick="Title.setTitleColorValue('#262626')" style="background:#262626;" title="#262626"></td>
	                            <td id="1A1A1AT" onClick="Title.setTitleColorValue('#1A1A1A')" style="background:#1A1A1A;" title="#1A1A1A"></td>
	                            <td id="0D0D0DT" onClick="Title.setTitleColorValue('#0D0D0D')" style="background:#0D0D0D;" title="#0D0D0D"></td>
	                            <td id="000000T" onClick="Title.setTitleColorValue('#000000')" style="background:#000000;" title="#000000"></td>
                            </tr>
                            <tr>
	                            <td id="F39E77T" onClick="Title.setTitleColorValue('#F39E77')" style="background:#F39E77;" title="#F39E77"></td>
	                            <td id="F6B580T" onClick="Title.setTitleColorValue('#F6B580')" style="background:#F6B580;" title="#F6B580"></td>
	                            <td id="F9CD8AT" onClick="Title.setTitleColorValue('#F9CD8A')" style="background:#F9CD8A;" title="#F9CD8A"></td>
	                            <td id="FFF99DT" onClick="Title.setTitleColorValue('#FFF99D')" style="background:#FFF99D;" title="#FFF99D"></td>
	                            <td id="C7E19ET" onClick="Title.setTitleColorValue('#C7E19E')" style="background:#C7E19E;" title="#C7E19E"></td>
	                            <td id="A8D59DT" onClick="Title.setTitleColorValue('#A8D59D')" style="background:#A8D59D;" title="#A8D59D"></td>
	                            <td id="89CA9DT" onClick="Title.setTitleColorValue('#89CA9D')" style="background:#89CA9D;" title="#89CA9D"></td>
	                            <td id="8CCCCAT" onClick="Title.setTitleColorValue('#8CCCCA')" style="background:#8CCCCA;" title="#8CCCCA"></td>
	                            <td id="8DCFF4T" onClick="Title.setTitleColorValue('#8DCFF4')" style="background:#8DCFF4;" title="#8DCFF4"></td>
	                            <td id="94AAD6T" onClick="Title.setTitleColorValue('#94AAD6')" style="background:#94AAD6;" title="#94AAD6"></td>
	                            <td id="9595C6T" onClick="Title.setTitleColorValue('#9595C6')" style="background:#9595C6;" title="#9595C6"></td>
	                            <td id="9681B7T" onClick="Title.setTitleColorValue('#9681B7')" style="background:#9681B7;" title="#9681B7"></td>
	                            <td id="AE88B8T" onClick="Title.setTitleColorValue('#AE88B8')" style="background:#AE88B8;" title="#AE88B8"></td>
	                            <td id="C790BAT" onClick="Title.setTitleColorValue('#C790BA')" style="background:#C790BA;" title="#C790BA"></td>
	                            <td id="F4A0BDT" onClick="Title.setTitleColorValue('#F4A0BD')" style="background:#F4A0BD;" title="#F4A0BD"></td>
	                            <td id="F49E9CT" onClick="Title.setTitleColorValue('#F49E9C')" style="background:#F49E9C;" title="#F49E9C"></td>
                            </tr>
                            <tr>
	                            <td id="EA6B48T" onClick="Title.setTitleColorValue('#EA6B48')" style="background:#EA6B48;" title="#EA6B48"></td>
	                            <td id="F09450T" onClick="Title.setTitleColorValue('#F09450')" style="background:#F09450;" title="#F09450"></td>
	                            <td id="F4B757T" onClick="Title.setTitleColorValue('#F4B757')" style="background:#F4B757;" title="#F4B757"></td>
	                            <td id="FEF668T" onClick="Title.setTitleColorValue('#FEF668')" style="background:#FEF668;" title="#FEF668"></td>
	                            <td id="ABD46ET" onClick="Title.setTitleColorValue('#ABD46E')" style="background:#ABD46E;" title="#ABD46E"></td>
	                            <td id="79C471T" onClick="Title.setTitleColorValue('#79C471')" style="background:#79C471;" title="#79C471"></td>
	                            <td id="31B573T" onClick="Title.setTitleColorValue('#31B573')" style="background:#31B573;" title="#31B573"></td>
	                            <td id="3EB8B4T" onClick="Title.setTitleColorValue('#3EB8B4')" style="background:#3EB8B4;" title="#3EB8B4"></td>
	                            <td id="3FBCEFT" onClick="Title.setTitleColorValue('#3FBCEF')" style="background:#3FBCEF;" title="#3FBCEF"></td>
	                            <td id="5E8AC6T" onClick="Title.setTitleColorValue('#5E8AC6')" style="background:#5E8AC6;" title="#5E8AC6"></td>
	                            <td id="676EB0T" onClick="Title.setTitleColorValue('#676EB0')" style="background:#676EB0;" title="#676EB0"></td>
	                            <td id="6A509BT" onClick="Title.setTitleColorValue('#6A509B')" style="background:#6A509B;" title="#6A509B"></td>
	                            <td id="8E569BT" onClick="Title.setTitleColorValue('#8E569B')" style="background:#8E569B;" title="#8E569B"></td>
	                            <td id="AF5D9CT" onClick="Title.setTitleColorValue('#AF5D9C')" style="background:#AF5D9C;" title="#AF5D9C"></td>
	                            <td id="ED6C9ET" onClick="Title.setTitleColorValue('#ED6C9E')" style="background:#ED6C9E;" title="#ED6C9E"></td>
	                            <td id="EC6B76T" onClick="Title.setTitleColorValue('#EC6B76')" style="background:#EC6B76;" title="#EC6B76"></td>
                            </tr>
                            <tr>
	                            <td id="E8641BT" onClick="Title.setTitleColorValue('#E8641B')" style="background:#E8641B;" title="#E8641B"></td>
	                            <td id="EE9C00T" onClick="Title.setTitleColorValue('#EE9C00')" style="background:#EE9C00;" title="#EE9C00"></td>
	                            <td id="7CC623T" onClick="Title.setTitleColorValue('#7CC623')" style="background:#7CC623;" title="#7CC623"></td>
	                            <td id="00B034T" onClick="Title.setTitleColorValue('#00B034')" style="background:#00B034;" title="#00B034"></td>
	                            <td id="009F3CT" onClick="Title.setTitleColorValue('#009F3C')" style="background:#009F3C;" title="#009F3C"></td>
	                            <td id="00A49DT" onClick="Title.setTitleColorValue('#00A49D')" style="background:#00A49D;" title="#00A49D"></td>
	                            <td id="00A8ECT" onClick="Title.setTitleColorValue('#00A8EC')" style="background:#00A8EC;" title="#00A8EC"></td>
	                            <td id="0669B2T" onClick="Title.setTitleColorValue('#0669B2')" style="background:#0669B2;" title="#0669B2"></td>
	                            <td id="314396T" onClick="Title.setTitleColorValue('#314396')" style="background:#314396;" title="#314396"></td>
	                            <td id="3D107BT" onClick="Title.setTitleColorValue('#3D107B')" style="background:#3D107B;" title="#3D107B"></td>
	                            <td id="67087BT" onClick="Title.setTitleColorValue('#67087B')" style="background:#67087B;" title="#67087B"></td>
	                            <td id="91007BT" onClick="Title.setTitleColorValue('#91007B')" style="background:#91007B;" title="#91007B"></td>
	                            <td id="E10052T" onClick="Title.setTitleColorValue('#E10052')" style="background:#E10052;" title="#E10052"></td>
	                            <td id="960014T" onClick="Title.setTitleColorValue('#960014')" style="background:#960014;" title="#960014"></td>
	                            <td id="9C410FT" onClick="Title.setTitleColorValue('#9C410F')" style="background:#9C410F;" title="#9C410F"></td>
	                            <td id="A06700T" onClick="Title.setTitleColorValue('#A06700')" style="background:#A06700;" title="#A06700"></td>
                            </tr>
                            <tr>
	                            <td id="A7A400T" onClick="Title.setTitleColorValue('#A7A400')" style="background:#A7A400;" title="#A7A400"></td>
	                            <td id="518412T" onClick="Title.setTitleColorValue('#518412')" style="background:#518412;" title="#518412"></td>
	                            <td id="007520T" onClick="Title.setTitleColorValue('#007520')" style="background:#007520;" title="#007520"></td>
	                            <td id="006A25T" onClick="Title.setTitleColorValue('#006A25')" style="background:#006A25;" title="#006A25"></td>
	                            <td id="006D68T" onClick="Title.setTitleColorValue('#006D68')" style="background:#006D68;" title="#006D68"></td>
	                            <td id="00709ET" onClick="Title.setTitleColorValue('#00709E')" style="background:#00709E;" title="#00709E"></td>
	                            <td id="044476T" onClick="Title.setTitleColorValue('#044476')" style="background:#044476;" title="#044476"></td>
	                            <td id="1D2A63T" onClick="Title.setTitleColorValue('#1D2A63')" style="background:#1D2A63;" title="#1D2A63"></td>
	                            <td id="260751T" onClick="Title.setTitleColorValue('#260751')" style="background:#260751;" title="#260751"></td>
	                            <td id="440351T" onClick="Title.setTitleColorValue('#440351')" style="background:#440351;" title="#440351"></td>
	                            <td id="600051T" onClick="Title.setTitleColorValue('#600051')" style="background:#600051;" title="#600051"></td>
	                            <td id="990050T" onClick="Title.setTitleColorValue('#990050')" style="background:#990050;" title="#990050"></td>
	                            <td id="980034T" onClick="Title.setTitleColorValue('#980034')" style="background:#980034;" title="#980034"></td>
	                            <td id="6C000AT" onClick="Title.setTitleColorValue('#6C000A')" style="background:#6C000A;" title="#6C000A"></td>
	                            <td id="712D08T" onClick="Title.setTitleColorValue('#712D08')" style="background:#712D08;" title="#712D08"></td>
	                            <td id="744900T" onClick="Title.setTitleColorValue('#744900')" style="background:#744900;" title="#744900"></td>
                            </tr>
                            <tr>
	                            <td id="7A7700T" onClick="Title.setTitleColorValue('#7A7700')" style="background:#7A7700;" title="#7A7700"></td>
	                            <td id="395F0BT" onClick="Title.setTitleColorValue('#395F0B')" style="background:#395F0B;" title="#395F0B"></td>
	                            <td id="005414T" onClick="Title.setTitleColorValue('#005414')" style="background:#005414;" title="#005414"></td>
	                            <td id="004B18T" onClick="Title.setTitleColorValue('#004B18')" style="background:#004B18;" title="#004B18"></td>
	                            <td id="004E49T" onClick="Title.setTitleColorValue('#004E49')" style="background:#004E49;" title="#004E49"></td>
	                            <td id="005073T" onClick="Title.setTitleColorValue('#005073')" style="background:#005073;" title="#005073"></td>
	                            <td id="032F55T" onClick="Title.setTitleColorValue('#032F55')" style="background:#032F55;" title="#032F55"></td>
	                            <td id="121C46T" onClick="Title.setTitleColorValue('#121C46')" style="background:#121C46;" title="#121C46"></td>
	                            <td id="190438T" onClick="Title.setTitleColorValue('#190438')" style="background:#190438;" title="#190438"></td>
	                            <td id="2E0239T" onClick="Title.setTitleColorValue('#2E0239')" style="background:#2E0239;" title="#2E0239"></td>
	                            <td id="440039T" onClick="Title.setTitleColorValue('#440039')" style="background:#440039;" title="#440039"></td>
	                            <td id="6F0039T" onClick="Title.setTitleColorValue('#6F0039')" style="background:#6F0039;" title="#6F0039"></td>
	                            <td id="6E0023T" onClick="Title.setTitleColorValue('#6E0023')" style="background:#6E0023;" title="#6E0023"></td>
	                            <td id="CBB99DT" onClick="Title.setTitleColorValue('#CBB99D')" style="background:#CBB99D;" title="#CBB99D"></td>
	                            <td id="9E8B75T" onClick="Title.setTitleColorValue('#9E8B75')" style="background:#9E8B75;" title="#9E8B75"></td>
	                            <td id="766454T" onClick="Title.setTitleColorValue('#766454')" style="background:#766454;" title="#766454"></td>
                            </tr>
                            </tbody>
                            </table>
                            <!-- 선택값 //-->
                            <div class="tit">
                                <ul>
                                    <li style="padding:5px 5px 0px 10px;"><span class="cap1">* <%=GetResource("Skin9")%></span>
                                    </li>
                                    <li><span id="titleTextColor"></span></li>
                                    <li class="range" id="titleTextColorSmaple">&nbsp;</li>
                                </ul>
                            </div>     
                        </li> 
                        <li>
                            <div class="cm_gap5"></div>
                            <h2><%=GetResource("TitleDisplay")%></h2>
                            <div class="bg">
                                <div class="item1 on" onclick="Title.setTitleImage('item1')" id="imgitem1"></div>
                                <div class="item2" onclick="Title.setTitleImage('item2')" id="imgitem2"></div>
                                <div class="item3" onclick="Title.setTitleImage('item3')" id="imgitem3"></div>
                                <div class="item4" onclick="Title.setTitleImage('item4')" id="imgitem4"></div>
                                <div class="item5" onclick="Title.setTitleImage('item5')" id="imgitem5"></div>
                                <div class="item6" onclick="Title.setTitleImage('item6')" id="imgitem6"></div>
                                <div class="item7" onclick="Title.setTitleImage('item7')" id="imgitem7"></div>
                                <div class="item8" onclick="Title.setTitleImage('item8')" id="imgitem8"></div>
                                <div class="item9" onclick="Title.setTitleImage('item9')" id="imgitem9"></div>
                                <div class="item10" onclick="Title.setTitleImage('item10')" id="imgitem10"></div>
                                <div class="item11" onclick="Title.setTitleImage('item11')" id="imgitem11"></div>
                                <div class="item12" onclick="Title.setTitleImage('item12')" id="imgitem12"></div>
                                <div class="item13" onclick="Title.setTitleImage('item13')" id="imgitem13"></div>
                                <div class="item14" onclick="Title.setTitleImage('item14')" id="imgitem14"></div>
                                <div class="item15" onclick="Title.setTitleImage('item15')" id="imgitem15"></div>
                                <div class="item16" onclick="Title.setTitleImage('item16')" id="imgitem16"></div>
                                <div class="item17" onclick="Title.setTitleImage('item17')" id="imgitem17"></div>
                                <div class="item18" onclick="Title.setTitleImage('item18')" id="imgitem18"></div>
                                <div class="item19" onclick="Title.setTitleImage('item19')" id="imgitem19"></div>
                                <div class="item20" onclick="Title.setTitleImage('item20')" id="imgitem20"></div>
                                <div class="item21" onclick="Title.setTitleImage('item21')" id="imgitem21"></div>
                                <div class="item22" onclick="Title.setTitleImage('item22')" id="imgitem22"></div>
                            </div>
                        </li>
                    </ul>
                 </div>
                </div>
        </div>        
    </div>
</div>
    </form>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>   
    <script type='text/javascript' src='/DeskPlusEIP/Html/Js/jQuery/jquery.ui.all.js'></script>
    <script type="text/javascript">
        var CommunityID = "<%=CommunityID %>";
        var TitleColor = "<%=TitleColor%>";
        var TitleImage = "<%=TitleImage %>";
        var MenuColor1 = "<%=MenuColor1 %>";
        var MenuColor2 = "<%=MenuColor2 %>";
        var MenuColor3 = "<%=MenuColor3 %>";
        var MenuBlock1TextClass = "<%=MenuBlock1TextClass %>";
        var MenuBlock2TextClass = "<%=MenuBlock2TextClass %>";
        var R_CompleteConfirm = "<%=GetResource("CompleteConfirm") %>";
        var R_ProcessingError = "<%=GetResource("ProcessingError") %>";
    </script>
    <script type='text/javascript' src='/DeskPlusEIP/Community/Js/Site/Admin/Function/Skin/Title.js'></script>
    <script type="text/javascript">
        //JsCall("/DeskPlusEIP/Community/Js/Site/Admin/Function/Skin/Title.js");
    </script>
</body>
</html>
