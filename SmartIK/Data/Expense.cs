using SmartIK.Enums;

namespace SmartIK.Data
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ResponseDate { get; set; }
        public StatusEnum Status { get; set; }
        public decimal Price { get; set; }
        public ExpenseTypeEnum ExpenseType { get; set; }
        public string FilePath { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
