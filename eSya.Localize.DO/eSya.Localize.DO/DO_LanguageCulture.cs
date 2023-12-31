﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eSya.Localize.DO
{
    public class DO_LanguageCulture
    {
        public int ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public string Culture { get; set; }
        public string CultureValue { get; set; }
        public bool ActiveStatus { get; set; }
        public string FormId { get; set; }
        public int UserID { get; set; }
        public string TerminalID { get; set; }
    }
}
