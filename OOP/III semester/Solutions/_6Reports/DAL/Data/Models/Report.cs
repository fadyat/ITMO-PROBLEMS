using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab_6.DAL.Data.Models
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; }
        public Employee Employee { get; set; }
    }
}