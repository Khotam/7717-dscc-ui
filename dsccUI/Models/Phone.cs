using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dsccUI.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }

    }
}