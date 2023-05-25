using HRMNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IPhuCapLuongService: IDisposable
    {
        public List<PHUCAP_DOC_HAI> GetAll();
        public PHUCAP_DOC_HAI AddDH(PHUCAP_DOC_HAI en);
        public PHUCAP_DOC_HAI UpdateDH(PHUCAP_DOC_HAI en);
        public void DeleteDH(int Id);
        public PHUCAP_DOC_HAI GetAllById(int Id);
        public List<BOPHAN> GetBoPhanAll();
    }
}
