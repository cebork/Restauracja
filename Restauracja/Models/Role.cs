namespace Restauracja.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Role> Roles { get; set;}

    }
}
