<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Album.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Template.Album" %>
<script type="text/javascript">
    var ImagePath = "<%=BaseImgPath%>";
    var PreLang = "<%=PreLang%>";

    $(document).ready(function () {

        var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

        //페이지 카운트
        var portletPageSize = parseInt("<%=PageSize %>");
        $("#hdnPageSize_" + portletDom).val(portletPageSize);

        if ("<%=IsAuth%>" != "True") {
            $("#basicMenu .del").remove();
        }

        //최소화 여부
        if ("<%=IsMinimize %>" == "1") {
            $("#ptSlide_" + portletDom).children('i').toggleClass('fa-chevron-up');
            $("#ptContent_" + portletDom).fadeOut("slow");
        }

        //포틀릿 최소화/최대화
        $("#ptSlide_" + portletDom).on("click", function (e) {
            if ($("#hdnTabSlide_" + portletDom).val() == "1") {
                //최대화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "0");
                $("#ptContent_" + portletDom).fadeIn("slow");
                $("#hdnTabSlide_" + portletDom).val("0");
            }
            else {
                //최소화
                Main.ModMinMaxPortlet(portletDom.split("_")[0], portletDom.split("_")[1], "1");
                $("#ptContent_" + portletDom).fadeOut("slow");
                $("#hdnTabSlide_" + portletDom).val("1");
            }
        });


        //포틀릿 설정
        $("#ptConfig_" + portletDom).on("click", function (e) {
            $("#ddlPageSize_" + portletDom).val(portletPageSize);
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

        /*
        //새로고침
        $("#ptReload_" + portletDom).on("click", function (e) {

            e.preventDefault();

            var loadingPath = "<img src='" + ImagePath + "/Icon/loading.gif' width='16' height='16' />";
            $(this).html(loadingPath);

            setTimeout(function () {

                //RSS 데이터 가져오기
                Main.GetPortletListData(portletDom, "0", portletPageSize - 1, "<%=DataType%>", "<%=DataURL%>", "<%=UITemplate%>", "/DeskPlusEIP/Portlet/Jt/Template/JTAlbum.htm");

                var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
                $("#ptReload_" + portletDom).html(loadingPath);

            }, 1);
        });
        */

        if ("<%=LinkURL%>" != "") {
            $("#ptPreview_" + portletDom).on("click", function (e) {
                e.preventDefault();
                var popupSize = "<%=LinkPopupSize%>";
                var arrPopUpSize = popupSize.split("|");
                var w = 0; h = 0;

                if (arrPopUpSize[0].length != 0) w = arrPopUpSize[0];
                if (arrPopUpSize[1].length != 0) h = arrPopUpSize[1];

                if (w != 0 && h != 0) {
                    PopupWinOpen("<%=LinkURL%>", "", "", w, h);
                } else {
                    WindowObjectReference = window.open("<%=LinkURL%>", "");
                    WindowObjectReference.focus();
                }
            });
        } else {
            //$("#ptPreview_" + portletDom).remove();
        }

        // 환경설정 저장
        $("#cbtnConfirm_" + portletDom).click(function (e) {
            e.preventDefault();

            portletPageSize = parseInt($("#ddlPageSize_<%=TabPortletID%>_<%=PortletID%>").val());
            $("#hdnPageSize_" + portletDom).val(portletPageSize);

            Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", portletPageSize, "");

            Main.GetPortletListData(portletDom, "0", portletPageSize - 1, "<%=DataType%>", "<%=DataURL%>", "<%=UITemplate%>", "/DeskPlusEIP/Portlet/Jt/Template/JTAlbum.htm");

            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        // 환경설정 닫기
        $("#cbtnClose_" + portletDom).click(function (e) {
            e.preventDefault();
            $("#ptListConfig_" + portletDom).slideToggle(600);
        });

        //$("#ptListConfig_" + portletDom).hide();

        //setTimeout(function () {
            //RSS Data 가져오기
            Main.GetPortletListData(portletDom, "0", portletPageSize - 1, "<%=DataType%>", "<%=DataURL%>", "<%=UITemplate%>", "/DeskPlusEIP/Portlet/Jt/Template/JTAlbum.htm");
		//}, 1);

        
    });
</script>

<!-- 포틀릿(탭+이미지-흰색바탕:사내앨범) Start //-->
<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet">    
   
        <div class="portlet-title portlet-header">
            <!-- 상단 타이틀 //-->
            <div class="caption">
                <span class="caption-subject text-uppercase"><%=Title%></span>
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
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="ddlPageSize_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
                            <option title="5" value="5">5 <%=GetResource("ListRowCnt")%></option>
                            <option title="10" value="10">10 <%=GetResource("ListRowCnt")%></option>
                            <option title="20" value="20">20 <%=GetResource("ListRowCnt")%></option>
                            <option title="30" value="30">30 <%=GetResource("ListRowCnt")%></option>
                        </select>
                    </div>
                </div>
                <div class="col-md-auto justify-content-end">                    
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnPDel_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("DeletePortlet")%></span></a>
                    </span>
                    <span>
                        <a class="btn btn-sm btn-outline-light" id="cbtnConfirm_<%=TabPortletID%>_<%=PortletID%>"><span><%=GetResource("Save")%></span></a>
                    </span>
                </div>    
            </div>
        </div>

       <div id="ptContent_<%=TabPortletID%>_<%=PortletID%>" class="portlet-body collapse show">           
             <div class="portlet-contents">
                <div class="nav-tabs">                                   
                </div>

                <!-- 내용 //-->
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>" >
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>
            </div>
        </div>  

        <div style="display:none">
            <input type="hidden" id="hdnPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnPageSize_<%=TabPortletID%>_<%=PortletID%>" value="5" />
            <input type="hidden" id="hdnPageNo_<%=TabPortletID%>_<%=PortletID%>" name="hdnPageNo_<%=TabPortletID%>_<%=PortletID%>" value="1" />
            <input type="hidden" id="hdnPageCount_<%=TabPortletID%>_<%=PortletID%>" name="hdnPageCount_<%=TabPortletID%>_<%=PortletID%>" value="50" />
            <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
        </div>
  
</div>
<!-- 포틀릿(탭+이미지-흰색바탕:사내앨범) End //-->