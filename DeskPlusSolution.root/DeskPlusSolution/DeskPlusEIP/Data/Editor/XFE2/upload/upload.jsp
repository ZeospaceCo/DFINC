
<%@ page contentType = "text/html;charset=utf-8" %>
<%@ page import = "java.io.*,
                   java.util.*,
                   com.oreilly.servlet.MultipartRequest,
                   com.oreilly.servlet.multipart.DefaultFileRenamePolicy"
%>
<%!
	public String usf_replace(String src, String oldstr, String newstr)
	{
		if (src == null) return null;
		StringBuffer dest = new StringBuffer("");
		try
		{
			int  len = oldstr.length();
			int  srclen = src.length();
			int  pos = 0;
			int  oldpos = 0;

			while ((pos = src.indexOf(oldstr, oldpos)) >= 0)
			{
				dest.append(src.substring(oldpos, pos));
				dest.append(newstr);
				oldpos = pos + len;
			}

			if (oldpos < srclen)
				dest.append(src.substring(oldpos, srclen));
		}
		catch ( Exception e )
		{
			e.printStackTrace();
		}
		return dest.toString();
	}

	public boolean FileExists(String sPath) throws Exception
	{
		File file = new File(sPath);
		if (file.exists())
			return true;
		else
			return false;
	}
%>
<%
	// 파일 용량 제한 : JSP 단에서는 2^31-1 bytes로 (integer max, 2147483647 bytes == 2GB - 1 byte)
	int sizeLimit = (2 * 1024 * 1024 * 1024 - 1);

	// 업로드 폴더 경로 : 날짜별로 폴더 생성
	String sDate = new java.text.SimpleDateFormat("yyyyMMdd").format(new java.util.Date());
	String savePath = getServletContext().getRealPath(request.getServletPath());
	int pos = -1;
	if ((pos = savePath.lastIndexOf("/")) > 0)
	{
		savePath = savePath.substring(0, pos+1) + sDate;
	}
	else if ((pos = savePath.lastIndexOf("\\")) > 0)
	{
		savePath = savePath.substring(0, pos+1) + sDate;
	}

	String saveRelativePath = request.getRequestURI();
	pos = saveRelativePath.lastIndexOf("/");
	saveRelativePath = saveRelativePath.substring(0, pos+1) + sDate;

	// 업로드 폴더 생성
	File dir = new File(savePath);
	dir.mkdir();

	// Windows file system인 경우 MultipartRequest에서 인식할 수 있는 경로 스타일로 바꿔줘야..
	if ((pos = savePath.lastIndexOf("/")) == -1)
	{
		if ((pos = savePath.indexOf(":\\")) > 0)
		{
			// TOMCAT에서는 드라이브 명 ("D:\\") 굳이 제거하지 않아도 잘 인식하는 듯..
			// 나중에 혹시나 싶어서 코멘트 남겨 둠..
			// savePath = "/" + savePath.substring(pos+2, savePath.length());
			savePath = usf_replace(savePath, "\\", "/");
		}
	}

	// cos(com.oreilly.servlet)의 MultipartRequest를 이용해서 업로드 !!!
	MultipartRequest single = new MultipartRequest( request, savePath, sizeLimit, "UTF-8", new DefaultFileRenamePolicy() );

	// 업로드 후 파일 정보 가져오기
	Enumeration eFileNames = single.getFileNames();
	String sParamName = eFileNames.nextElement().toString();
	String sFileName_Org = single.getOriginalFileName(sParamName);
	String sFileName_Gen = single.getFilesystemName(sParamName);
	String sExtension = sFileName_Org.substring(sFileName_Org.lastIndexOf("."));

	// content type
	// 2009.12.22 bhkwon : multipart/form-data로 POST한 경우,
	//                     일반 request.getParameter() 메소드로는 인자 값을 받아올 수 없다.
	//                     ==> cos(com.oreilly.servlet)의 MultipartRequest 객체를 사용하면 된다.
	String sContentType = single.getParameter("content_type");
	String sFileNameHead = "";
	if (sContentType.equals("flash"))
		sFileNameHead = "FLA_";
	else
		sFileNameHead = "IMG_";

	// 파일 이름 규칙 적용
	String sTime = new java.text.SimpleDateFormat("HHmmss").format(new java.util.Date());
	String sNewFName = sFileNameHead + sTime + sExtension;
	
	int index = 1;
	while (FileExists(savePath + "/" + sNewFName) && index < 100)
	{
		sNewFName = sFileNameHead + sTime + "_" + index + sExtension;
		index += 1;
	}

	// 파일 이름 변경
	File file1 = new File(savePath + "/" + sFileName_Gen);
	File file2 = new File(savePath + "/" + sNewFName);
	file1.renameTo(file2);
	
	String sUploadedPath = saveRelativePath + "/" + sNewFName;
	String sErrorLog = "";
%>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
		<!--script src="../config/crossdomain.js" type="text/javascript" language="javascript"></script-->
		<title>XFE UPLOAD</title>
		<script type="text/javascript">
			window.onload = function()
			{
				var strContentType = document.getElementById("divContentType").innerHTML;
				var strImagePath = document.getElementById("divImagePath").innerHTML;
				
				if (strImagePath != undefined && strImagePath != "")
				{
					var strLocation = "<%=sUploadedPath%>";

					if (strContentType == "flash") {
						//parent.tseInsertThum(strLocation);
						parent.insertFlash.setFlash(strLocation);
					} else if (strContentType == "image") {
						//parent.tseInsertThum(strLocation);
						parent.insertImage.setImage(strLocation);
					}
				} else {
					alert("<%=sErrorLog%>");
				}
								
		
			};
		</script>
	</head>
	<body>
		<div id="divContentType"><%=sContentType%></div>
		<div id="divImagePath"><%=sUploadedPath%></div>
		<div><%=sErrorLog%></div>
	</body>
</html>
