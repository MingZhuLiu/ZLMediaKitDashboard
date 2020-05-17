using AutoMapper;
using ZLServerDashboard.DataBase;
using ZLServerDashboard.Models.Dto;

namespace ZLServerDashboard.Commons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TbUser, UserDto>().ReverseMap();
            CreateMap<TbMenu, MenuDto>().ReverseMap();
            CreateMap<TbRole, RoleDto>().ReverseMap();
            CreateMap<TbUserRole, UserRoleDto>().ReverseMap();
            CreateMap<TbMenuRole, MenuRoleDto>().ReverseMap();
            CreateMap<TbDomain, DomainDto>().ReverseMap();
            CreateMap<TbApplication, ApplicationDto>().ReverseMap();
            CreateMap<TbStreamProxy, StreamProxyDto>().ReverseMap();

        }
    }
}
