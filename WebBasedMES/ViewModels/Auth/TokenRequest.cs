﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.ViewModels.Auth
{
    public class TokenRequest
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
    }
}
