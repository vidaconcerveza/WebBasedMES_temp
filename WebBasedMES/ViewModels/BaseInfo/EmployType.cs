﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.BaseInfo
{
    public class EmployTypeRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }


    }
    public class EmployTypeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Memo { get; set; }
        public bool IsUsing { get; set; }


    }
}
