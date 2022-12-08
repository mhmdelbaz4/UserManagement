using System.Collections.Generic;

namespace UserManagement.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<RoleViewModel> Roles { get; set; }
    }
}
