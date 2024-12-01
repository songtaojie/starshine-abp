using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.SqlSugarCore
{
    /// <summary>
    /// 忽略更新种子数据特性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class IgnoreSeedUpdateAttribute : Attribute
    {
    }
}
