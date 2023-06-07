namespace Police.Security.User {

    public interface IUserCacheService {

        void FlushCacheForWindowsSid(string windowsSid);
        void FlushEntireCache();

    }

}