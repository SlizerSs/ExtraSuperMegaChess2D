namespace ChessAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            Sides = new HashSet<Side>();
        }
        
        public int GameID { get; set; }

        [StringLength(255)]
        public string FEN { get; set; }

        [StringLength(4)]
        public string Status { get; set; }

        [StringLength(63)]
        public string Winner { get; set; }

        [StringLength(6)]
        public string LastMove { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Side> Sides { get; set; }
    }
}
