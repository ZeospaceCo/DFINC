using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Component.Linked.WebFax.Entity;

namespace Zeospace.DeskPlus.Component.Linked.WebFax.Interfaces
{
    /// <summary>
    /// 인터페이스 생성
    /// 심재운
    /// </summary>
    public interface IWebFax
    {
        /// <summary>
        /// 전송하기
        /// 작성자 : 심재운
        /// </summary>
        /// <param name="_be"></param>
        /// <param name="_alRecv"></param>
        /// <returns></returns>
        bool Transfer(FaxInfo _be, List<FaxRecvInfo> _alRecv);
    }
}
