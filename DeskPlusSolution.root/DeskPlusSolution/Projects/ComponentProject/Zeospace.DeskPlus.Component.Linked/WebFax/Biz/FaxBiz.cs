using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Component.Linked.WebFax.Interfaces;

namespace Zeospace.DeskPlus.Component.Linked.WebFax.Biz
{
    public class FaxBiz
    {
        IWebFax _repository;

        public FaxBiz(IWebFax repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 처리하기
        /// </summary>
        /// <param name="_faxInfo"></param>
        /// <param name="_recvList"></param>
        /// <returns></returns>
        public bool Transfer(Entity.FaxInfo _faxInfo, List<Entity.FaxRecvInfo> _recvList)
        {
            return _repository.Transfer(_faxInfo, _recvList);
        }
    }
}
