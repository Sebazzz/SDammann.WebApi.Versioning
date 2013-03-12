namespace SDammann.WebApi.Versioning {
    using System;
    using System.Web.Http;

    /// <summary>
    ///   Previously implementation is now in <see cref="AcceptHeaderVersionedControllerSelectorBase"/>, this class is obsolete and will be replaced in a future version.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [Obsolete("Derive from AcceptHeaderVersionedControllerSelectorBase. This class will be replaced in a future version.")]
    public abstract class AcceptHeaderVersionedControllerSelector : AcceptHeaderVersionedControllerSelectorBase {
        protected AcceptHeaderVersionedControllerSelector(HttpConfiguration configuration) : base(configuration) {}
    }
}