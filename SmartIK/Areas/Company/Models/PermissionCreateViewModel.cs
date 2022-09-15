using Microsoft.AspNetCore.Mvc.Rendering;
using SmartIK.Areas.Company.Enums;
using SmartIK.Data;
using System.ComponentModel.DataAnnotations;

namespace SmartIK.Areas.Company.Models
{
    public class PermissionCreateViewModel
    {
        public ApplicationUser   ApplicationUser { get; set; }
        
        [Display(Name = "Permission Type")]
        public PermissionType PermissionType { get; set; }
        public DateTime RequestPermissionDate { get; set; } = DateTime.Now;

        [Display(Name = "Permission Start Date")]
        public DateTime PermissionStartDate { get; set; }
        
        [Display(Name = "Permission End Date")]
        public DateTime PermissionEndDate { get; set; }
        public int? PermissionDays { get; set; }
        public IEnumerable<SelectListItem> PermissionTypes { get; set; }


    }
}
