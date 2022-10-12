using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bulkybookweb.ViewModels.Administration
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            _Users = new List<string>();
        }
        private string id;
        private string _RoleName;
        public List<string> _Users { get; set; }
        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [Required(ErrorMessage = "Role Name is Required")]
        public string RoleName
        {
            get { return this._RoleName; }
            set { this._RoleName = value; }
        }

    }
}

