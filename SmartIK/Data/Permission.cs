using SmartIK.Enums;

namespace SmartIK.Data
{
    public class Permission
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public DateTime RequestPermissionDate { get; set; } = DateTime.Now;
        public DateTime PermissionStartDate { get; set; }
        public DateTime PermissionEndDate { get; set; }
        public int? PermissionDays { get; set; }
        public StatusEnum Status { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
