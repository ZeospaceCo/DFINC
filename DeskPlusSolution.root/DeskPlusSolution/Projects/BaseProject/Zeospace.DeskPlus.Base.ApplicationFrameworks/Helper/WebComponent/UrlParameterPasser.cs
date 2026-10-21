using System.Collections;
using System.Web;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent
{
    /// <summary>
    /// UrlParameterPasser에 대한 요약 설명입니다.
    /// </summary>
    public class UrlParameterPasser : BaseParameterPasser
    {
        #region Member Variables

        private SortedList localQueryString = null;

        #endregion Member Variables

        #region Constructors

        public UrlParameterPasser()
            : base()
        {
        }

        public UrlParameterPasser(string Url)
            : base(Url)
        {
        }

        #endregion Constructors

        #region Methods

        public override void PassParameters()
        {
            // add parameters, if any exist
            if (localQueryString.Count > 0)
            {
                // see if we need to add the ?
                if (base.Url.IndexOf("?") == -1)
                    base.Url += "?";
                else
                    base.Url += "&";

                bool firstOne = true;
                foreach (DictionaryEntry o in localQueryString)
                {
                    if (!firstOne)
                        base.Url += "&";
                    else
                        firstOne = false;

                    base.Url += string.Concat(HttpContext.Current.Server.UrlEncode(o.Key.ToString()), "=", HttpContext.Current.Server.UrlEncode(o.Value.ToString()));
                }
            }

            base.PassParameters();
        }

        public string GetParameter()
        {
            // add parameters, if any exist
            if (localQueryString.Count > 0)
            {
                // see if we need to add the ?
                if (base.Url.IndexOf("?") == -1)
                    base.Url += "?";
                else
                    base.Url += "&";

                bool firstOne = true;
                foreach (DictionaryEntry o in localQueryString)
                {
                    if (!firstOne)
                        base.Url += "&";
                    else
                        firstOne = false;

                    base.Url += string.Concat(HttpContext.Current.Server.UrlEncode(o.Key.ToString()), "=", HttpContext.Current.Server.UrlEncode(o.Value.ToString()));
                }
            }

            return base.Url;
        }

        #endregion Methods

        #region Properties

        public override string this[string name]
        {
            get
            {
                if (localQueryString == null)
                {
                    if (HttpContext.Current != null)
                        return HttpContext.Current.Request.QueryString[name];
                    else
                        return null;
                }
                else
                    return localQueryString[name].ToString();
            }
            set
            {
                if (localQueryString == null)
                    localQueryString = new SortedList();

                // add if it is new, or replace the old value
                if ((localQueryString[name]) == null)
                    localQueryString.Add(name, value);
                else
                    localQueryString[name] = value;
            }
        }

        public override ICollection Keys
        {
            get
            {
                if (localQueryString == null)
                {
                    if (HttpContext.Current != null)
                        return HttpContext.Current.Request.QueryString.Keys;
                    else
                        return null;
                }
                else
                    return localQueryString.Keys;
            }
        }

        #endregion Properties
    }
}