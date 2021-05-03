namespace ChessAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Side
    {
        public int SideID { get; set; }

        public int? GameID { get; set; }

        public int? PlayerID { get; set; }

        [StringLength(1)]
        public string Color { get; set; }

        public virtual Game Game { get; set; }

        public virtual Player Player { get; set; }
    }
}
