using System;
using System.Collections.Generic;

namespace eSya.Localize.DL.Entities
{
    public partial class GtEclttm
    {
        public GtEclttm()
        {
            GtEclttls = new HashSet<GtEclttl>();
        }

        public int TableCode { get; set; }
        public string SchemaName { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual ICollection<GtEclttl> GtEclttls { get; set; }
    }
}
