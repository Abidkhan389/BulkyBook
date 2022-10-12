using System;
using System.Collections.Generic;

namespace BulkyBookWeb.Models
{
    public partial class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Displayorder { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
