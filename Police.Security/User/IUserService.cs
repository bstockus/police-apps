using System;
using System.Threading.Tasks;

namespace Police.Security.User {

    public interface IUserService {

        Task<UserInformation> FetchUserInformationByWindowsSid(string windowsSid);

        Task<UserInformation> FetchUserInformationByUserId(Guid userId);

    }

}