<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GallerySubListN.aspx.cs" Inherits="Zeospace.DeskPlus.Community.Web.Site.Apps.Board.GallerySubListN" %>
<%@ Register Assembly="Zeospace.DeskPlus.Base.ApplicationFrameworks" Namespace="Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent"
    TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8" />
    <title><%=ConfigurationManager.AppSettings["SolutionTitle"].ToString()%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <meta name="description" content="Sliding Panel Photowall Gallery with jQuery" />
    <meta name="keywords" content="jquery, images, gallery, photowall, slideshow"/>    
    <link href="/DeskPlusEIP/Html/Js/SlidingPanelPhotowallGallery/css/style.css" rel="stylesheet" type="text/css" media="screen" />   
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="/DeskPlusEIP/Html/Js/jQuery/jquery.common.js"></script>
    <script type="text/javascript">
    </script>
</head>

<body>
    <form id="form1" runat="server">    
        <div class="infobar">
            <span id="description"></span>
            <span id="loading">Loading Image</span>
            <span class="reference">Image Viewer</span>
        </div>
        <div id="thumbsWrapper">
            <div id="content" >
                <%=ImageHtml %>
                <div class="placeholder"></div>
            </div>
        </div>
        <div id="panel">
            <div id="wrapper">
                <a id="prev"></a>
                <a id="next"></a>
            </div>
        </div>
    
     <script src="/DeskPlusEIP/Html/Js/SlidingPanelPhotowallGallery/PhootowallGallery.js" type="text/javascript"></script>

    </form>    
</body>
</html>
