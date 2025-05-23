using PassiveMoneyTracker.Enums;

namespace PassiveMoneyTracker.Models
{
    public class PassiveIncome
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public IncomeType Type { get; set; }

        public string Source { get; set; }

        public string Description { get; set; }

        public ReceivedStatus Received { get; set; }
    }
}
