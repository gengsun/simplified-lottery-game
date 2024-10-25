using Bede.SimplifiedLottery.Domain.Enums;

namespace Bede.SimplifiedLottery.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; init; }
        public int PlayerId { get; init; }
        public DrawStatus DrawStatus { get; set; } = DrawStatus.NotSet;
        public int PrizeAmount { get; set; }
    }
}
