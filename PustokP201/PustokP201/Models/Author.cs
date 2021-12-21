using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PustokP201.Models
{
    public class Author:BaseEntity
    {
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }

        public List<Book> Books { get; set; }
    }
}
