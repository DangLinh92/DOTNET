using OPERATION_MNS.Data.Enums;
using OPERATION_MNS.Data.Interfaces;
using OPERATION_MNS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OPERATION_MNS.Data.Entities
{
    [Table("VIEW_CONTROL_CHART_MODEL")]
    public class VIEW_CONTROL_CHART_MODEL : DomainEntity<int>, IDateTracking
    {
        public VIEW_CONTROL_CHART_MODEL()
        {

        }

        public string CHART_X { get; set; }
        public string DATE { get; set; }
        public string MATERIAL_ID { get; set; }
        public string LOT_ID { get; set; }
        public string CASSETTE_ID { get; set; }
        public string MAIN_OPERATION { get; set; }
        public string MAIN_EQUIPMENT_ID { get; set; }
        public string MAIN_EQUIPMENT_NAME { get; set; }
        public string MAIN_CHARACTER { get; set; }
        public string MAIN_UNIT { get; set; }
        public double MAIN_TARGET_USL { get; set; }
        public double MAIN_FIXED_UCL { get; set; }
        public double MAIN_TARGET { get; set; }
        public double MAIN_FIXED_LCL { get; set; }
        public double MAIN_TARGET_LSL { get; set; }
        public double MAIN_TARGET_UCL { get; set; }
        public double MAIN_TARGET_LCL { get; set; }
        public double MAIN_VALUE_COUNT { get; set; }
        public double MAIN_VALUE1 { get; set; }
        public double MAIN_VALUE2 { get; set; }
        public double MAIN_VALUE3 { get; set; }
        public double MAIN_VALUE4 { get; set; }
        public double MAIN_VALUE5 { get; set; }
        public double MAIN_VALUE6 { get; set; }
        public double MAIN_VALUE7 { get; set; }
        public double MAIN_VALUE8 { get; set; }
        public double MAIN_VALUE9 { get; set; }
        public double MAIN_VALUE10 { get; set; }
        public double MAIN_VALUE11 { get; set; }
        public double MAIN_VALUE12 { get; set; }
        public double MAIN_VALUE13 { get; set; }
        public double MAIN_VALUE14 { get; set; }
        public double MAIN_VALUE15 { get; set; }
        public double MAIN_VALUE16 { get; set; }
        public double MAIN_VALUE17 { get; set; }
        public double MAIN_VALUE18 { get; set; }
        public double MAIN_VALUE19 { get; set; }
        public double MAIN_VALUE20 { get; set; }
        public double MAIN_VALUE21 { get; set; }
        public double MAIN_VALUE22 { get; set; }
        public double MAIN_VALUE23 { get; set; }
        public double MAIN_VALUE24 { get; set; }
        public double MAIN_VALUE25 { get; set; }
        public double MAIN_VALUE26 { get; set; }
        public double MAIN_VALUE27 { get; set; }
        public double MAIN_VALUE28 { get; set; }
        public double MAIN_VALUE29 { get; set; }
        public double MAIN_VALUE30 { get; set; }
        public double MAIN_MAX_VALUE { get; set; }
        public double MAIN_MIN_VALUE { get; set; }
        public double MAIN_AVG_VALUE { get; set; }
        public double MAIN_RANGE { get; set; }

        [StringLength(50)]
        public string MAIN_JUDGE_FLAG { get; set; }

        [StringLength(10)]
        public string IsSendTeams { get; set; }

        [StringLength(50)]
        public string DateCreated { get; set; }

        [StringLength(50)]
        public string DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
