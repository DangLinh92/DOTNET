using HRMNS.Data.Interfaces;
using HRMS.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HRMNS.Data.Entities
{
    [Table("FILE_MANAGER")]
    public class FILE_MANAGER : DomainEntity<int>
    {
        [Column(TypeName = "NCHAR(40)")]
        [Required(AllowEmptyStrings = false)] // or false
        [StringLength(40)]
        public string Name { get; set; }

        public int? ParentID { get; set; }

        [Column(TypeName = "BIGINT")]
        public long? Size { get; set; }

        [Column(TypeName = "BIT")]
        public bool? IsFile { get; set; }

        [Column(TypeName = "NCHAR(200)")]
        [StringLength(200)]
        public string MimeType { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]
        public byte[] Content { get; set; }

        [Column(TypeName = "BIT")]
        public bool? HasChild { get; set; }

        [Column(TypeName = "BIT")]
        public bool? IsRoot { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [StringLength(255)]
        public string Type { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        [StringLength(200)]
        public string FilterPath { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        [StringLength(200)]
        public string StorageLocation { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        [StringLength(50)]
        public string DateEx { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName = "DATETIME")]
        public DateTime? DateModified { get; set; }

        [StringLength(50)]
        public string UserCreated { get; set; }

        [StringLength(50)]
        public string UserModified { get; set; }
    }
}
