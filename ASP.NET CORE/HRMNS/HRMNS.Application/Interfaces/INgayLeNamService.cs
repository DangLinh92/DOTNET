using HRMNS.Application.ViewModels.Time_Attendance;
using HRMNS.Data.Entities;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMNS.Application.Interfaces
{
    public interface INgayLeNamService : IDisposable
    {
        List<NgayLeNamViewModel> GetAll(string keyword);

        List<NgayLeNamViewModel> Search(string condition,string param);

        ResultDB ImportExcel(string filePath, string param);

        NgayLeNamViewModel GetById(string id, params Expression<Func<NGAY_LE_NAM, object>>[] includeProperties);

        NgayLeNamViewModel Add(NgayLeNamViewModel ngayleVm);

        void Update(NgayLeNamViewModel ngayleVm);

        void UpdateNghiBu();

        void Delete(string id);

        void Save();
    }
}
