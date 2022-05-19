﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string Picture { get; set; }

        //Navigation Properties
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
