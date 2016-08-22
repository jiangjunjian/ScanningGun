using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanningGun.entity
{
    [Table("Goods")]
    public class GoodsEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        public String barCode { get; set; }
        public String Name { get; set; }
        public int cateogory { get; set; }
        public int quanty { get; set; }
    }
}
