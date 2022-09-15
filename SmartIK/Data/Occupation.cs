namespace SmartIK.Data
{
    public class Occupation
    {
        public int Id { get; set; }
        public string OccupationName { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
