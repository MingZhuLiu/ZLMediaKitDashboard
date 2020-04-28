using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ZLMediaKitService.Commons;

namespace ZLMediaKitService.DataBase
{
    public abstract class Meta
    {

    }

    public abstract class MetaEntity : Meta
    {
        public MetaEntity()
        {
            Id = Tools.NewID;
            SortId = 0;
            var now = Tools.ToTimestampLong(DateTime.Now,true);
            CreateTime = now;
            UpdateTime = now;
        }

        [Key, Required]
        [Column("ID")]
        public long Id { get; set; }

        [Required]
        [Column("SORT_ID")]
        public long SortId { get; set; }

        [Required]
        [Column("CREATE_TIME")]
        public long CreateTime { get; set; }

        [Required]
        [Column("UPDATE_TIME")]
        public long UpdateTime { get; set; }
    }



}
