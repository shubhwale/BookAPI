﻿using System;
using System.Collections.Generic;

namespace BooksAPI.Models
{
    public partial class RefreshToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public virtual Users User { get; set; }
    }
}
