using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bulkybookweb.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        private string _RoleName;

        [Required]
        public string RoleName 
        {
            get { return this._RoleName; }
            set { this._RoleName = value; }
        }
    }
}
