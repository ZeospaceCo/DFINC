using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Component.Linked.WebFax.Interfaces;
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;
using System.Data;

namespace Zeospace.DeskPlus.Component.Linked.WebFax.Repositories
{
    public class BizLotteRepository : IWebFax
    {
        /// <summary>
        /// 팩스 전송하기
        /// </summary>
        /// <param name="_be"></param>
        /// <param name="_alRecv"></param>
        /// <returns></returns>
        public bool Transfer(FaxInfo _be, List<FaxRecvInfo> _alRecv)
        {
            if (_be == null || _alRecv == null) return false;

            string filePath = "";

            #region LDCC 가 지정한 회사번호 4자리

            DataView dv = AppSettingHelper.GetSysCode(_be.CompanyID, "FaxClientNo");
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["CodeID"].ToString() == "ClientNo") _be.ClientNo = dv[i]["NameBase"].ToString();
                if (dv[i]["CodeID"].ToString() == "FaxFileDirectory") filePath = dv[i]["NameBase"].ToString();
            }

            if (_be.ClientNo.Length == 0 || String.IsNullOrEmpty(_be.ClientNo)) return false;

            dv = null;

            #endregion

            filePath += "SendData/"; //송신폴더
            filePath = FileHelper.GetServerMapPath(filePath);
            string fileFullPath = filePath + System.Guid.NewGuid().ToString().Replace("-", "") + ".cov";

            

            //TODO : EUC-KR 로 파일 생성해야 함.
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileFullPath,
                true, System.Text.Encoding.GetEncoding("euc-kr")))
            {
                //foreach (string line in lines)
                //{
                //    // If the line doesn't contain the word 'Second', write the line to the file. 
                //    if (!line.Contains("Second"))
                //    {
                //        file.WriteLine(line);
                //    }
                //}                

                file.WriteLine("[Basic]");
                file.WriteLine("RecvCount=" + _alRecv.Count);
                file.WriteLine("FileCount=" + _be.FileNameList.Count());
                file.WriteLine("");

                file.WriteLine("[CoverData]");
                file.WriteLine("UseCover=0");
                file.WriteLine("CoverIndex=1");
                file.WriteLine("SendName=");
                file.WriteLine("SendComp=");
                file.WriteLine("SendPhone=");
                file.WriteLine("SendMobile=");
                file.WriteLine("SendFax=");
                file.WriteLine("Subject=" + _be.Subject);
                file.WriteLine("Memo=");
                file.WriteLine("");

                int i = 1;
                foreach (var recv in _alRecv)
                {
                    file.WriteLine(String.Format("[Recipient{0}]", i));
                    file.WriteLine("RecvName=" + recv.RecvName);
                    file.WriteLine("RecvComp=" + recv.RecvComp);
                    file.WriteLine("RecvPhone=" + recv.RecvPhone);
                    file.WriteLine("RecvMobile=" + recv.RecvMobile);
                    file.WriteLine("RecvFax=" + recv.RecvFax);
                    file.WriteLine("key2=" + recv.Key2);
                    file.WriteLine("");

                    i += 1;
                }
                

                file.WriteLine("[DBInfo]");
                file.WriteLine("key=" + _be.Key);
                file.WriteLine("");

                file.WriteLine("[FileName]");
                i = 1;
                foreach (var files in _be.FileNameList)
                {
                    file.WriteLine(String.Format("File{0}", i) + "=" + files);
                    i += 1;
                }
                file.WriteLine("");

                file.WriteLine("[ReservedTime]");
                file.WriteLine("ResTime=");
                file.WriteLine("");

                file.WriteLine("[UserInfo]");
                file.WriteLine("ClientNo=" + _be.ClientNo);
                file.WriteLine("Dept=" + _be.Dept);
                file.WriteLine("UserName=" + _be.UserName);
                file.WriteLine("Email=" + _be.Email);
                file.WriteLine("UserCode=" + _be.UserCode);

            }


            return true;
        }
    }
}
