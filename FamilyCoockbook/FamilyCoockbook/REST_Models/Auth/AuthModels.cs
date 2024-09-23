namespace FamilyCookbook.REST_Models.Auth
{
    public sealed class AuthModels
    {
        public sealed record AuthLogIn (string username, string password);
    }
}
