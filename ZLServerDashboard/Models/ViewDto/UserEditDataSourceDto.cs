using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Models.ViewDto
{
    public class UserEditDataSourceDto
    {
        public UserEditDataSourceDto()
        {
            NowUserRoles = new List<UserRoleDto>();
        }
        public List<RoleDto> AllRoles { get; set; }
        public UserDto User { get; set; }
        public List<UserRoleDto> NowUserRoles { get; set; }

    }
}
