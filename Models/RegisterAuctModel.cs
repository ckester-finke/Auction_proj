using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Auction_proj.Models
{
    public class RegisterAuctModel : BaseEntity
    {
        [Required]
        [MinLength(3)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string ProductName { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public int StartingBid { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
}