-----------------------------------
브라우저 정보 얻기 : jquery-migrate-1.1.1.min.js 사용법
-----------------------------------

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>브라우저 정보 얻기</title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
       
    </style>
   <script src="jquery-1.9.1.min.js" ></script>
   <script src="jquery-migrate-1.1.1.min.js"></script>
   
   <script type="text/javascript">
       $(document).ready(function() {
            var result = "";
            // $.browser의 모든 속성 출력
            jQuery.each(jQuery.browser, function(i, val){
                result += i + ":" + val + "\n";
           
            });
            alert(result);
           
            //브라우저 확인
            if ($.browser.msie){
                alert("IE 웹 브라우저");  
            }else{
                alert("다른 웹브라우저");   
            }
           
       }); //end jQuery ready  
    </script>
 
</head>
<body>  
 
</body>
</html>