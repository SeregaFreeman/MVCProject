using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class Tag
    {
        [Required]
        public int TagId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string TagName { get; set; }

        public List<Post> Posts { get; set; }
       
    }
}