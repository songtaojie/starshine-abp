using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Ui.Branding;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Branding
{
    /// <summary>
    ///  Starshine Branding Provider
    /// </summary>
    public interface IStarshineBrandingProvider: IBrandingProvider
    {
        /// <summary>
        /// app描述
        /// </summary>
        string? AppDescription { get; }
    }
}
