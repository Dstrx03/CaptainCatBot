
namespace Cat.Business.Services.InternalServices
{
    public struct RefresherSettings
    {
        public bool IsEnabled { get; set; }

        public int IntervalMinutes { get; set; }
    }
}