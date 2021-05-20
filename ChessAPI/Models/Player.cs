namespace ChessAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Player
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            Sides = new HashSet<Side>();
        }

        public int PlayerID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public int? Games { get; set; }

        public int? Wins { get; set; }

        public int? Loses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Side> Sides { get; set; }
    }
}
