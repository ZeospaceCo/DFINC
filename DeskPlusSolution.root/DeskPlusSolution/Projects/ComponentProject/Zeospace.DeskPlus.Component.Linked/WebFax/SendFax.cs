using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;
using System.Reflection;
using Zeospace.DeskPlus.Component.Linked.WebFax.Interfaces;
using System.Data;

namespace Zeospace.DeskPlus.Component.Linked.WebFax
{
    public class SendFax
    {   
        private readonly string AssemblyName = "Zeospace.DeskPlus.Component.Linked";
        private string ComName = String.Empty;

        public SendFax()
        {
            
        }

        /// <summary>
        /// 전송하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_be"></param>
        /// <param name="_alRecv"></param>
        /// <returns></returns>
        public bool Transfer(FaxInfo _be, List<FaxRecvInfo> _alRecv)
        {
            DataView dv = AppSettingHelper.GetSysCode(_be.CompanyID, "FaxClientNo");
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["CodeID"].ToString() == "FaxRepository")
                {
                    ComName = dv[i]["NameBase"].ToString();
                    break;
                }
            }            

            if (ComName.Length == 0) return false;

            string AssemClass = "Zeospace.DeskPlus.Component.Linked.WebFax.Repositories." + ComName;
            AssemClass = AssemClass + ", " + AssemblyName;

            #region REPOSITORY 패턴 사용

            Type obj = Type.GetType(AssemClass);
            ConstructorInfo constructor = obj.GetConstructor(new Type[] { });
            IWebFax defaultRepository = (IWebFax)constructor.Invoke(null);

            Biz.FaxBiz faxBiz = new Biz.FaxBiz(defaultRepository);
            return defaultRepository.Transfer(_be, _alRecv);

            #endregion

        }
    }
}
