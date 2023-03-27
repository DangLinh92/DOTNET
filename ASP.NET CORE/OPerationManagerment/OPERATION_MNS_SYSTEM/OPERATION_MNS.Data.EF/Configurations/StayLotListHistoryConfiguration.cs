using OPERATION_MNS.Data.EF.Extensions;
using OPERATION_MNS.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace OPERATION_MNS.Data.EF.Configurations
{

    public class StayLotListHistoryConfiguration : DbEntityConfiguration<STAY_LOT_LIST_HISTORY>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_HISTORY> entity)
        {
            entity.HasKey(x => new { x.LotId, x.History_seq });
        }
    }

    public class StayLotListHistoryWlp2Configuration : DbEntityConfiguration<STAY_LOT_LIST_HISTORY_WLP2>
    {
        public override void Configure(EntityTypeBuilder<STAY_LOT_LIST_HISTORY_WLP2> entity)
        {
            entity.HasKey(x => new { x.LotId, x.History_seq });
        }
    }
}
