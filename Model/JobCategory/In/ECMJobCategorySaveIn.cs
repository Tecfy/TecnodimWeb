﻿using System;

namespace Model.In
{
    public class ECMJobCategorySaveIn : BaseIn
    {
        public string registration { get; set; }

        public string categoryId { get; set; }

        public string archive { get; set; }

        public string code { get; set; }

        public string title { get; set; }

        public string unityCode { get; set; }
        
        public string unityName { get; set; }

        public DateTime dataJob { get; set; }

        public string user { get; set; }

        public string extension { get; set; }
    }
}
