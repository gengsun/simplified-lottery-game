using System.Globalization;

namespace Bede.SimplifiedLottery.Domain.Extensions
{
    public static class IntegerExtensions
    {
        public static string ToDisplayString(this int value)
            => (value / 100m).ToString("C", CultureInfo.CurrentCulture);
    }
}
