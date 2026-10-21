using System;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    public class ExecuteFile
    {
        /// <summary>
        /// 실행파일 실행여부
        /// </summary>
        /// <param name="_fullFilePath">The _full file path.</param>
        /// <param name="_folderPath">c:\\temp\\</param>
        /// <returns>
        ///   <c>true</c> if [is execute file] [the specified _full file path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExecuteFile(string _fullFilePath, string _folderPath)
        {
            try
            {
                //전체 파일 경로
                string strFilePath = _fullFilePath;

                // ProcessInfo object 생성
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe");
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardError = true;

                //해당 디렉토리 경로
                psi.WorkingDirectory = _folderPath;

                // 프로세스 시작
                System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);

                // 읽기 위해 batch 파일 실행
                System.IO.StreamReader strm = System.IO.File.OpenText(strFilePath);

                // 읽기 위해 output 첨부
                System.IO.StreamReader sOut = proc.StandardOutput;

                // 쓰기 위해 첨부
                System.IO.StreamWriter sIn = proc.StandardInput;

                // 실행한 결과값을 line 별로 가져오기
                while (strm.Peek() != -1)
                {
                    sIn.WriteLine(strm.ReadLine());
                }

                //닫기
                strm.Close();

                // CMD.EXE 나가기
                string stEchoFmt = "# {0} run successfully. Exiting";

                sIn.WriteLine(String.Format(stEchoFmt, strFilePath));
                sIn.WriteLine("EXIT");

                // 프로세스 닫기
                proc.Close();

                // 스트링 문자 가져오기
                string results = sOut.ReadToEnd().Trim();

                // io Streams 닫기
                sIn.Close();
                sOut.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}