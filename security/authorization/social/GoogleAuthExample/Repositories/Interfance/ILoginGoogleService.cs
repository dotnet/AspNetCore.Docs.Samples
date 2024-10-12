namespace GoogleAuthticationExample.Repositories.Interfance
{
    public interface ILoginGoogleService
    {
        Task<bool> LoginExtnal(string email);
        Task<bool> RegisterExtnal(string email);

    }
}
