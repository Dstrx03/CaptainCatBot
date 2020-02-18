
namespace Cat.Business.Services.InternalServices
{
    public struct ATriggerSettings
    {
        public bool IsEnabled { get; set; }

        public int TimeSliceMinutes { get; set; }
    }
}