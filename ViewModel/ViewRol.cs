using System.ComponentModel.DataAnnotations;

namespace MVCKanban.ViewModel
{
    public class ViewRol
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
    public class RolDto
    {
        public string Name { set; get; }
        public string Id { set; get; }
        public bool check { set; get; }
    }
}