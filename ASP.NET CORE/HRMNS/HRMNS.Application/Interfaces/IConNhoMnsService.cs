using HRMNS.Data.Entities.Payroll;
using HRMNS.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRMNS.Application.Interfaces
{
    public interface IConNhoMnsService : IDisposable
    {
        List<HR_CON_NHO> GetConNhos();
        HR_CON_NHO PutConNho(HR_CON_NHO en);
        HR_CON_NHO PostConNho(HR_CON_NHO en);
        HR_CON_NHO GetConNhoById(int id);
        void DeleteConNho(int id);
        public ResultDB ImportConNhoExcel(string filePath, string param);
        void Save();
    }
}
