﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BooksAPI.Models
{
    public class UserCreateResponse
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public string Name { get; set; }
    }
}
