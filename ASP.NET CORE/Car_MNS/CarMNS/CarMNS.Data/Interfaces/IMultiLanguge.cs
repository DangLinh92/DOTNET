using System;
using System.Collections.Generic;
using System.Text;

namespace CarMNS.Data.Interfaces
{
    public interface IMultiLanguge<T>
    {
        T LanguageId { get; set; }
    }
}
