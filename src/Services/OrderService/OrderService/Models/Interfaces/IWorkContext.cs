namespace OrderService.Models.Interfaces
{
    public interface IWorkContext
    {
        public int GetUserId();

        public string GetUserName();

        public string GetEmail();
    }
}