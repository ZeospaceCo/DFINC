<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DeptUserSearch.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.UserSearch.DeptUserSearch" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";
    var PreLangReal = "<%=PreLangReal %>";
    var R_DeptOrName = "<%=GetResource("DeptOrName") %>";
    var DeptUserSearch_IsMini = "<%=IsMinimize %>";

    //ContextMenu를 동적으로 만들어서 Main 페이지에 추가함 (Layout Style에 영향을 맞지 않도록하기 위함) ByFreeman 2012.05.30
    $(document).ready(function () {

        var portletDom = <%=TabPortletID%> + "_" + <%=PortletID%>; 
        
         //최소화 여부
        if (DeptUserSearch_IsMini == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");   
        }       

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if($("#hdnTabSlide_" + portletDom).val() == "1")
            {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");     
                $("#hdnTabSlide_" + portletDom).val("0"); 
            }
            else{
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");    
                $("#hdnTabSlide_" + portletDom).val("1");
            }            
        });

        
        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            $("#ptListConfig_" + portletDom).slideToggle(600);      
        });      
         
        // 포틀릿 삭제
        $("#cbtnPDel_" + portletDom).click(function (e) {
            e.preventDefault();

            Main.DelTabPortlet(portletDom);
            if (PortletAuth == "True") {
                $("#portlet_" + portletDom).fadeOut("slow");
            }
        });
      
      
        //바로가기
        $("#ptPreview_" + portletDom).on("click", function (e) {
            e.preventDefault();                        

	        parent.frmTop.Menu.GoParam("AccountSearch"); 
            document.location.href = "/DeskPlusEIP/account/search/default.aspx";
            
        });
                
        $('#userSearch').click(function (e) {
            e.preventDefault();
            GoTree();
        });

        $('#cbtnSearchName').click(function (e) {
            e.preventDefault();
            Search();
        });

       $('#delDept').click(function (e) {
            e.preventDefault();
            DelDept();
        });        
       
    });

    /**************************************************
작성자: freeman
작성일:2012-02-07
작성목적 : 포틀릿 Context Menu
***************************************************/
function ContextMenuWork(id, action, el, pos) {
    switch (action) {
        case "del":
            {
                Main.DelTabPortlet(id);
                if (PortletAuth == "True") {
                    $("#portlet_" + id).fadeOut("slow");
                }
                break;
            }
        case "min":
            {
                Main.ModMinMaxPortlet(<%=TabPortletID%>, <%=PortletID%>, "1");
                $("#ptContent_" + id).fadeOut("slow");
                break;
            }
        case "max":
            {
                Main.ModMinMaxPortlet(<%=TabPortletID%>, <%=PortletID%>, "0");
                $("#ptContent_" + id).fadeIn("slow");
                break;
            }
        case "info":
            {
                alert(R_NoSettingInfo);
                break;
            }
    }
}
//트리 호출
function GoTree()
{

  var url ="/DeskPlusEIP/Portlet/Package/UserSearch/Search.aspx";
    PopupWin(400, 478, url, "", "", true);
   
}

//트리값바인딩
function SetInfo(id,value)
{
    var decValue =  unescape(value);
    decValue = decValue.split('|')[1];
    lblDept.innerText = decValue; //부서이름표기
    $("#txtDeptID").val(id);    //부서ID 

}

//검색
function Search()
{    
    var strName = $("#txtSearchName").val();    
    var deptID = $("#txtDeptID").val();
    
    if (jQuery.trim(strName) == "" && deptID == "") {
        alert(R_DeptOrName);
        return;
    }

    var deptName = lblDept.innerText;
    
     ParamArray = {
        'userName': strName.replace("&", "zeo$"),
        'deptID':deptID,
        'gubun':0,
        'deptName':deptName.replace("&", "zeo$")
    };

    CallEncrypt(CallGetParam());
    
    var url ="/DeskPlusEIP/Portlet/Package/UserSearch/AccountInfo.aspx?p="+GetEncryptValue;
    //alert(url);
    PopupWin(600, 435, url, "", "", true);

}
//검색어삭제
function DelDept()
{    
    lblDept.innerText = ""; //textbox초기화
    $("#txtDeptID").val("");  //부서ID초기화

}
</script>

<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-msearch">    
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><a class="ptReload text-primary" id="ptPreview_<%=TabPortletID%>_<%=PortletID%>"><%=Title%>&nbsp;&nbsp;<i class="fas fa-chevron-right"></i></a></span>
                <span class="caption-helper"></span>
            </div>
            <div class="actions">
                <a href="" id="ptConfig_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-setting-icon"><i class="fas fa-cog"></i></a>
                <a href="" id="ptSlide_<%=TabPortletID%>_<%=PortletID%>" data-toggle="collapse" aria-expanded="false" class="btn portlet-roll-icon"><i class="fas fa-chevron-down"></i></a>
            </div>
        </div>

         <div id="ptListConfig_<%=TabPortletID%>_<%=PortletID%>" class="portlet-setting" style="display:none;">
            <!-- 환경설정 //-->             
            <div class="row">
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

        <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">
            <div class="portlet-contents">

                <div class="nav-tabs">
                    <!-- 탭이 없을경우 내용만 비울것 -->
                </div>

                <div class="content-tabs">
                   <form class="form-inline">
                        <div class="col-12" style="padding:3px 0">
	                        <div style="width:20%; display:inline-block"><%=GetResource("Dept")%></div>
	                        <div style="width:40%; display:inline-block">
							    <span id="lblDept"></span>
						    </div>
	                        <div style="width:35%; display:inline-block">
							    <button type="button" class="btn btn-sm btn-secondary" id="userSearch"><i class="fas fa-sitemap"></i></button>
							    <button type="button" class="btn btn-sm btn-secondary" id="delDept"><i class="fas fa-times-circle"></i></button>
						    </div>
					    </div>
                        <div class="col-12" style="padding:3px 0">
	                        <div style="width:20%; display:inline-block"><%=GetResource("SearchName") %></div>
	                        <div style="width:40%; display:inline-block;">
	                             <input type="text" id="txtSearchName" class="keyword form-control form-control-sm" style="width:100px" />
						    </div>
	                        <div style="width:35%; display:inline-block">
							    <button type="button" class="btn btn-sm btn-outline-light" id="cbtnSearchName"><%=GetResource("Search")%></button>
						    </div>
					    </div>
					</form> 
                </div>
            </div>
        </div>

        <div style="display: none">           
                <input type="text" id="txtDeptID" style="width:80px;display:none"  />    
                <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>    
</div>
