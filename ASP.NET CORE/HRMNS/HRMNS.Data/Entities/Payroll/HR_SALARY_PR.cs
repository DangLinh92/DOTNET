using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities.Payroll
{
    [Table("HR_SALARY_PR")]
    public class HR_SALARY_PR : DomainEntity<int>, IDateTracking
    {
        public HR_SALARY_PR()
        {

        }

        public decimal BasicSalary { get; set; }

        public decimal LivingAllowance { get; set; } // phu cap đời sống
        public decimal PositionAllowance { get; set; }
        public decimal AbilityAllowance { get; set; }
        public decimal FullAttendanceSupport { get; set; }
        public decimal SeniorityAllowance { get; set; }
        public decimal HarmfulAllowance { get; set; }

        public decimal IncentiveStandard { get; set; }
        public decimal IncentiveLanguage { get; set; }
        public decimal IncentiveTechnical { get; set; }
        public decimal IncentiveOther { get; set; }

        [StringLength(250)]
        public string Note { get; set; }

        /// <summary>
        /// Hỗ trợ công đoạn
        /// </summary>
        public decimal HoTroCongDoan { get; set; }

        public decimal PCCC_CoSo { get; set; }
        public decimal HoTroATVS_SinhVien { get; set; }

        public decimal SoNguoiPhuThuoc { get; set; }

        /// <summary>
        /// Thuộc đối tượng tham gia BHXH (x)
        /// Insuarance Member
        /// </summary>
        /// 
        [StringLength(50)]
        public string ThuocDoiTuongBaoHiemXH { get; set; } // x


        [StringLength(50)]
        public string DoiTuongTruyThuBHYT { get; set; } // x

        [StringLength(50)]
        public string DoiTuongPhuCapDocHai { get; set; } // x

        /// <summary>
        /// Tham gia công đoàn
        /// Nhũng TH ko tham gia công đoàn:
        /// 1. Vào làm việc sau ngày 15 của tháng đó(dựa vào cột ngày vào làm)
        /// 2. Nghỉ thải sản(dựa vào cột note trong bảng lương)
        /// 3. Nghỉ không lương > 14 ngày(ở bảng công cột số ngày nghỉ không hưởng lương)
        /// 4. Rút khỏi công đoàn
        /// </summary>
        /// 

        [StringLength(50)]
        public string ThamGiaCongDoan { get; set; } // x

        [StringLength(50)]
        public string IncentiveSixMonth1 { get; set; }// đánh giá 6 tháng đầu

        [StringLength(50)]
        public string IncentiveSixMonth2 { get; set; }

        public int SoConNho { get; set; }

        [StringLength(50)]
        public string MaNV { get; set; }

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
