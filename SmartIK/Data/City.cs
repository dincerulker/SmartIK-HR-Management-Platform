namespace SmartIK.Data
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public List<Corporation> Companies { get; set; }
    }
}
