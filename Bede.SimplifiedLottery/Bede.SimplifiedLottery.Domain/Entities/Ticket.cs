using Bede.SimplifiedLottery.Domain.Enums;

namespace Bede.SimplifiedLottery.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public int PlayerId { get; set; }
        public DrawStatus DrawStatus { get; set; } = DrawStatus.NotSet;
        public int PrizeAmount { get; set; }
    }
}
