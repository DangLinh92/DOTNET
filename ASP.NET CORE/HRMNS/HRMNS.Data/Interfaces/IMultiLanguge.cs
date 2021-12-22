using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Data.Interfaces
{
    public interface IMultiLanguge<T>
    {
        T LanguageId { get; set; }
    }
}
