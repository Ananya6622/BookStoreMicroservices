using BookStore.User.Entity;

namespace BookStore.User.Interfaces
{
    public interface IUser
    {
        public UserEntity UserRegistration(UserEntity userEntity);
        public string GenerateToken(string Email, int UserId);
        public string UserLogin(string email, string password);
        public UserEntity MyProfile(int userId);

    }
}
