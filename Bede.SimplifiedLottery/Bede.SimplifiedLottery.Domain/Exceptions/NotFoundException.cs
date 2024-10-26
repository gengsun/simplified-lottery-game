namespace Bede.SimplifiedLottery.Domain.Exceptions
{
    public class NotFoundException(string message) : BaseException(message) { }
}
