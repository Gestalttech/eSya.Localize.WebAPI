using System;
using System.Collections.Generic;

namespace eSya.Localize.DL.Entities
{
    public partial class GtEclttl
    {
        public string LanguageCode { get; set; } = null!;
        public int TableCode { get; set; }
        public int TablePrimaryKeyId { get; set; }
        public string FieldDescLanguage { get; set; } = null!;
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; } = null!;
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedTerminal { get; set; } = null!;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedTerminal { get; set; }

        public virtual GtEclttm TableCodeNavigation { get; set; } = null!;
    }
}
