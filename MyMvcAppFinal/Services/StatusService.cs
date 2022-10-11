namespace MyMvcAppFinal.Services
{
    public interface IStatusService
    {
        public string GetStatus();
    }
    public class StatusService : IStatusService
    {
        private string Active { get { return "Активно"; } }
        private string Blocked { get { return "Заблокировано"; } }
        private bool Flag { get; set; }

        public string GetStatus()
        {
            return Flag ? Active : Blocked;
        }
    }
}
