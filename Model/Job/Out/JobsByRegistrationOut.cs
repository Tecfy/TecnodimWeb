﻿using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class JobsByRegistrationOut : ResultServiceVM
    {
        public JobsByRegistrationOut()
        {
            result = new List<JobsByRegistrationVM>();
        }

        public List<JobsByRegistrationVM> result { get; set; }
    }
}
