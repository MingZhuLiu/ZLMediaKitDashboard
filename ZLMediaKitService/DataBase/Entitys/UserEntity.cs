
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZLMediaKitService.DataBase
{
    [Table("Tb_User")]
    public class UserEntity :MetaEntity
    {
        // [Key, Required]
        // [Column("ID")]
        // public long Id { get; set; }

        // [Required]
        // [Column("SORT_ID")]
        // public long SortId { get; set; }

        // [Required]
        // [Column("CREATE_TIME")]
        // public long CreateTime { get; set; }

        // [Required]
        // [Column("UPDATE_TIME")]
        // public long UpdateTime { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [ Required, StringLength(5, MinimumLength = 16)]
        [Column("Account")]
        public string Account { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Required]
        [Column("State")]
        public int State { get; set; }
    }

}