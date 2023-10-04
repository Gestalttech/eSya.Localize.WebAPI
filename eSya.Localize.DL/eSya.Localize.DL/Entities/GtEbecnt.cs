using System;
using System.Collections.Generic;

namespace eSya.Localize.DL.Entities
{
    public partial class GtEbecnt
    {
        public int Id { get; set; }
        public string Area { get; set; } = null!;
        public string Controller { get; set; } = null!;
        public bool ActiveStatus { get; set; }
    }
}
