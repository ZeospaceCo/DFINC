@echo off
cd bin
SET PATH=%PATH%;C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322
gacutil /u ICSharpCode.SharpZipLib.dll
gacutil /u System.Web.Extensions.dll
gacutil /u System.Web.Extensions.Design.dll
gacutil /i ICSharpCode.SharpZipLib.dll
gacutil /i System.Web.Extensions.dll
gacutil /i System.Web.Extensions.Design.dll

                                
