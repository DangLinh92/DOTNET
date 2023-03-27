using OPERATION_MNS.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Application.Interfaces
{
    public interface IOutGoingReceiptService : IDisposable
    {
        List<OUTGOING_RECEIPT_WLP2> GetAllToDay();
        List<OUTGOING_RECEIPT_WLP2> GetAllByDay(string day);

        OUTGOING_RECEIPT_WLP2 Update(OUTGOING_RECEIPT_WLP2 model);
        OUTGOING_RECEIPT_WLP2 GetById(int id);
        OUTGOING_RECEIPT_WLP2 GetByKey(string key);

        void Save();
    }
}
