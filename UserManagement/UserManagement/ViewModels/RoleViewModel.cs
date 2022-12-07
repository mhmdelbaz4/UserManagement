using System.ComponentModel.DataAnnotations;

namespace UserManagement.ViewModels
{
    public class RoleViewModel
    {
        [Required,StringLength(256)]
        public string Name { get; set; }
    }
}
