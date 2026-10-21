using System.Web.UI;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    public sealed class ServerFindControl
    {
        /// <summary>
        /// 컨트롤안에 속한 모든 자식컨트롤을 검색한다.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static Control FindControlAll(Control _obj, string _name)
        {
            foreach (Control oChild in _obj.Controls)
            {
                if (oChild.ID == _name)
                    return oChild;
                if (oChild.HasControls())
                {
                    Control oFind = FindControlAll(oChild, _name);
                    if (oFind != null)
                        return oFind;
                }
            }

            return null;
        }
    }
}