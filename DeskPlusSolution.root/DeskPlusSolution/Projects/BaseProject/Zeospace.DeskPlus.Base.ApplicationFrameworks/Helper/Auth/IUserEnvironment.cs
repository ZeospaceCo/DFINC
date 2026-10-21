namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth
{
    /// <summary>
    /// 작성자   : 심재운
    /// 작성일자 : 2008-08-08
    /// 작성목적 : 사용자 정보 인터페이스
    /// </summary>
    internal interface IUserEnvironment
    {
        System.Reflection.Assembly AssemblyName { set; }

        /// <summary>
        /// Gets the pre lang.
        /// </summary>
        /// <value>The pre lang.</value>
        string PreLang { get; }

        /// <summary>
        /// Gets the pre lang real.
        /// </summary>
        /// <value>The pre lang real.</value>
        string PreLangReal { get; }

        /// <summary>
        /// Sets the name of the resource.
        /// </summary>
        /// <value>The name of the resource.</value>
        string ResourceName { set; }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The pre lang.</value>
        string BaseTimeZone { get; }
    }
}