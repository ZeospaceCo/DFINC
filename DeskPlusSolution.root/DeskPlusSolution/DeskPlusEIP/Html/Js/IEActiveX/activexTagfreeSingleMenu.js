//document.write('<OBJECT id="TFEditor" style="Z-INDEX: 101; POSITION: absolute" codeBase="/DeskPlusEIP/Data/Editor/tweditor.cab#version=2,5,0,16" height="100%" width="100%" classid="CLSID:6E1AAB9B-F6D8-43f1-AD33-7C2EBCDF0AE4" VIEWASTEXT><PARAM NAME="InitFile" VALUE="/DeskPlusEIP/Data/Editor/env_singleMenu.xml"><PARAM NAME="ApplyInitData" VALUE="1"><PARAM NAME="Mode" VALUE="0"></OBJECT>')

var clsid =  "976A7D6C-B14C-4e50-A5C3-B43D8C49D8C8";
var version = "3,7,1,1020";
var cab = "/DeskPlusEIP/Data/Editor/tweditor.cab";
var env = "/DeskPlusEIP/Data/Editor/env_singleMenu.xml";
var id = "TFEditor";
var applyinitdata = 1;//apply:1
var editmode = 0;//edit:0

var key = "+hqj8GWmFAe94x0al0N3ToMjmgMBNidWvjdk3ILWs0KNRkRasF18xPgN0IkgCzyK3vgviEQO22mErVWezWk0KWR2LCrl1IogqDjk0J8xI6pOC8+qCbi0yqwzbkt9ZrGj81+85rvKkKaESK9c64xCBYrWaUIE52KO2xOA8VQk2zoWP6IgzAMiNRwnEE6d9OSH6pxkTamj+LSiVgmzjanvuBf1IQ4mIXaDN4e6zOiJWQ1GRpk2v6DtBUdqvvlgPBlucLluDhIPNIfAL3T76sqMClpK4pdFwRmWD69CpRVRjGIGk78fMqKcjnvRfuSyvqK8sMKZVIlcgqfgpoXWlW0riA6zkiHCHjFVts3LHY7HA62cavXh7kf/e5laj2F3mUSNOkGdWkTmhzWeeE1q7JayKm0lArjZ+LVhOifbzhKXxm5b9u8UpqXCFm6/DJj3LC5rgZIHafDWOvfhca2a7QIcFC9BFNu8yzY8/ewUHSfrGXV0vuoIKcv7REpzROn8JWoXRtjIOJhqSPZ2JoxPBJSv4LdQcUcVOqe7KaOR94J+VAZFYAFojnxRYqJgqPfR62QIQZgI3CvJU+muk74RphArQL4mdfjK0vDpvbN9Rt1gbcHJVOasWN48g1smFLua9Obhh92iA2XlaEQCJrjoFNg4IQtynSlyBoU+Mpxk5N4YCEJ+qYYg5BaP/bYMupt7guz82H28eui3M7/Y64umSGj3KzQb2grwKFogeYYQRSksAfs=";

document.write('<object ID="'+id+'" width="100%" height="100%" CLASSID="CLSID:'+clsid+'" CODEBASE="'+cab+'#version='+version+'" >');
document.write('<PARAM name="InitFile" value="'+env+'"/>');
document.write('<PARAM name="ApplyInitData" VALUE="'+applyinitdata+'"/>');
document.write('<PARAM name="Mode" VALUE="'+editmode+'"/>');
document.write('<PARAM name="LicenseKey" value="'+key+'">');
document.write('</object>');