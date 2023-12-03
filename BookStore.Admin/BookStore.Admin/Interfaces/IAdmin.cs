using BookStore.Admin.Entity;

namespace BookStore.Admin.Interfaces
{
    public interface IAdmin
    {
        public AdminEntity AdminRegistration(AdminEntity adminEntity);
        public AdminEntity AdminLogin(string email, string password);
    }
}
