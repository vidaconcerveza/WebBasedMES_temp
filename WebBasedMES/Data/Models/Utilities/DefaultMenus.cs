using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBasedMES.Data.Models.Utilities
{
    public class DefaultMenu
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public bool IsAbleToAccess { get; set; } = true;
        public bool IsAbleToRead { get; set; } = true;
        public bool IsAbleToReadWrite { get; set; } = true;
        public bool IsAbleToDelete { get; set; } = false;
        public List<DefaultSubMenu> Submenu { get; set; }
    }

    public class DefaultSubMenu
    {
        public int Id { get; set; }
        public int DefaultMenuId { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsAbleToAccess { get; set; } = true;
        public bool IsAbleToRead { get; set; } = true;
        public bool IsAbleToReadWrite { get; set; } = true;
        public bool IsAbleToDelete { get; set; } = false;

        public bool IsFavorite { get; set; } = false;
    }

}
