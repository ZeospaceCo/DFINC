	<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Album.ascx.cs" Inherits="Zeospace.DeskPlus.Portlet.Web.Package.Board.Album"  EnableViewState="false" %>
	<script type="text/javascript">
	    var ImagePath = "<%=BaseImgPath%>";
	    var PreLang = "<%=PreLang%>";

	    $(document).ready(function () {

		var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

		// 리스트 행 개수
		$("#hdnBoardAlbumPageSize_" + portletDom).val("<%=PageSize %>");
		var boardAlbum_PageSize = $("#hdnBoardAlbum_PageSize_" + portletDom).val();

		// 카테고리 아이디
		$("#hdnBoardAlbumCategoryID_" + portletDom).val("<%=BoardCategoryID %>");

		if ("<%=IsAuth%>" != "True") {
		    $("#cbtnPDel_" + portletDom).remove();
		}

		//최소화 여부
		if ("<%=IsMinimize %>" == "1") {
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
            // 리스트 행 개수 바인딩
		    $("#boardAlbumListCount_" + portletDom).val($("#hdnBoardAlbumPageSize_" + portletDom).val());
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
			LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>();

			var loadingPath = "<img src='<%=BaseImgPath%>/Main/ico_ref.gif' />";
			$("#ptReload_" + portletDom).html(loadingPath);

		    }, 1);
		});
        */

		//상세바로가기
		$("#ptPreview_" + portletDom).on("click", function (e) {
		
		    e.preventDefault();

		    var paramValue = $("#hdnBoardAlbumParamValue_" + portletDom).val();	    

		    if(paramValue != ""){
			//menuCatID$1^catID$57^dcparm$BOARD1
			var paramValueStr = paramValue.split("^");
			var menuCatID = "";
			var catID = "";
			var dcparm = "";

			for(var i=0;i<paramValueStr.length;i++)
			{
			    switch(paramValueStr[i].split("$")[0])
			    {
				case "menuCatID" :
				    menuCatID = paramValueStr[i].split("$")[1];
				    break;
				case "catID" :
				    catID = paramValueStr[i].split("$")[1];
				    break;
				case "dcparm" :
				    dcparm = paramValueStr[i].split("$")[1];
				    break;
			    }
			}
			
			ParamArray = {
			    'menuCatID': menuCatID,
			    'catID': catID,
			    'dcparm': dcparm
			};

			CallEncrypt(CallGetParam());            
			location = "/DeskPlusEIP/Board/Default.aspx?p=" + GetEncryptValue;
		    }
		});

		// 환경설정 저장
		$("#cbtnConfirm_" + portletDom).click(function (e) {
		    e.preventDefault();

		    // 리스트 행 개수 설정
		    boardAlbum_PageSize = $("#boardAlbumListCount_" + portletDom).val();
		    $("#hdnBoardAlbumPageSize_" + portletDom).val(boardAlbum_PageSize);   

		    Main.ModTabPortletUser("<%=TabPortletID%>", "<%=PortletID%>", boardAlbum_PageSize, "");

		    LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>();

		    $("#ptListConfig_" + portletDom).slideToggle(600);
		});

		// 환경설정 닫기
		$("#cbtnClose_" + portletDom).click(function (e) {
		    e.preventDefault();
		    $("#ptListConfig_" + portletDom).slideToggle(600);
		});

		//$("#ptListConfig_" + portletDom).hide();

		//setTimeout(function () {
		    LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>();
		//}, 1);

	    });

	    /**************************************************
	    작성자: freeman
	    작성일:2012-02-07
	    작성목적 : 첫 페이지 가져오기
	    ***************************************************/
	    function LoadBoardAlbumList_<%=TabPortletID%>_<%=PortletID%>() {
		var portletDom = "<%=TabPortletID%>_<%=PortletID%>";

		//게시판 리스트
		var urlPage = "/DeskPlusEIP/Portlet/Ws/Package/Board/Album.asmx/GetBoardList";
		
		ParamArray = {
		    'pageSize': $("#hdnBoardAlbumPageSize_" + portletDom).val(),     // 리스트 행 개수
		    'categoryID': $("#hdnBoardAlbumCategoryID_" + portletDom).val()  // 카테고리 아이디
		};

		CallEncrypt(CallGetParam());
		var paramData = "{p:'" + GetEncryptValue + "'}";
		
		$.ajax({
		    type: "POST",
		    url: urlPage,
		    async: true,
		    data: paramData,
		    contentType: "application/json; charset=utf-8",
		    dataType: "json",
		    success: function (data) {
			$('#plList_' + portletDom).setTemplateURL("/DeskPlusEIP/Portlet/Jt/Package/Board/JTAlbum.htm?seq=" + Guids(), null, { filter_data: false });
			$('#plList_' + portletDom).processTemplate(data.d);

			$("#plList_" + portletDom + " .imageSlider .imageList").jCarouselLite({
			    btnNext: "#plList_" + portletDom + " .imageSlider .right",
			    btnPrev: "#plList_" + portletDom + " .imageSlider .left",
			    visible: Math.floor($("#plList_" + portletDom).width() / 160),
			    speed: 800
			});

			
			if($(".NoListData") != undefined) { $(".NoListData").text(R_NoContent); }
			
			if ($(".PtlType3Left") != undefined) { $(".PtlType3Left").attr('src', ImagePath + '/Package/gallleft.gif'); }
			if ($(".IconNoImg") != undefined) { $(".IconNoImg").attr('src', ImagePath + '/Icon/ico_noimg2.gif'); }
			if ($(".PtlType3Right") != undefined) { $(".PtlType3Right").attr('src', ImagePath + '/Package/gallright.gif'); }
		    }
		,
		    error: function (data, status, err) {
			alert(R_ErrorConnection + data);
		    }
		});
	    }
	     
	</script>
	<!-- 포틀릿(탭+이미지-흰색바탕:사내앨범) Start //-->
	<div id="portlet_<%=TabPortletID%>_<%=PortletID%>" class="portlet portlet-notice">    
	    
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
                <div class="col col-lg portlet-setting-select">
                    <div class="form-group">
                        <select id="boardAlbumListCount_<%=TabPortletID%>_<%=PortletID%>" class="form-control form-control-sm">
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
                <div class="content-tabs" id="plList_<%=TabPortletID%>_<%=PortletID%>">
                    <div style="padding-top:50px;text-align:center;">
                        <img src="<%=BaseImgPath%>/pims/loading.gif" alt="Loading..." title="Loading..." />
                    </div>
                </div>

            </div>
        </div> 

		<div style="display: none">
		<input type="hidden" id="hdnBoardAlbumPageSize_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardAlbumPageSize_<%=TabPortletID%>_<%=PortletID%>" />
		<input type="hidden" id="hdnBoardAlbumCategoryID_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardAlbumCategoryID_<%=TabPortletID%>_<%=PortletID%>" />
		<input type="hidden" id="hdnBoardAlbumParamValue_<%=TabPortletID%>_<%=PortletID%>" name="hdnBoardAlbumParamValue_<%=TabPortletID%>_<%=PortletID%>" value="<%=ParamValue %>" />
        <input type="hidden" id="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" name="hdnTabSlide_<%=TabPortletID%>_<%=PortletID%>" value="<%=IsMinimize %>" />
		</div>

</div>
	<!-- 포틀릿(탭+이미지-흰색바탕:사내앨범) End //-->