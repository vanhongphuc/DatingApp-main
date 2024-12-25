using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>>GetUsersAsync();
        Task<AppUser>GetUsersByIdAsync(int id);
        Task<AppUser>GetUsersByUsernameAsync(string username);
        Task<PagedList<MemberDto>>GetMembersAsync(UserParams userParams);
        Task<MemberDto>GetMemberAsync(string username);
        Task GetUserByUsernameAsync(string v);
    }
}