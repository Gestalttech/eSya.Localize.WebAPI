using System;
using System.Collections.Generic;

namespace eSya.Localize.DL.Entities
{
    public partial class GtEcltu
    {
        public int UserId { get; set; }
        public string LoginId { get; set; } = null!;
        public string LoginDesc { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }
    }
}
