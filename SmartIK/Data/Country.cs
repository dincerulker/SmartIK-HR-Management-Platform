namespace SmartIK.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public List<City> Cities { get; set; }
        public List<Corporation> Companies { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}
