namespace ChessAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PlayerStatistic
    {
        [Key]
        public int PlayerID { get; set; }

        public int? Games { get; set; }

        public int? Wins { get; set; }

        public int? Loses { get; set; }

        public int? Draws { get; set; }

        public int? Resigns { get; set; }

        public virtual Player Player { get; set; }
    }
}
