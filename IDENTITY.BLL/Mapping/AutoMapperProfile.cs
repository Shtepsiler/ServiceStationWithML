using AutoMapper;
using IDENTITY.BLL.DTO.Requests;
using IDENTITY.BLL.DTO.Responses;
using IDENTITY.DAL.Entities;

namespace IDENTITY.BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateUserMaps();

        }
        private void CreateUserMaps()
        {
            CreateMap<UserSignUpRequest, User>().ReverseMap();
            CreateMap<UserRequest, User>().ReverseMap();
            CreateMap<UserResponse, User>().ReverseMap();

        }




    }
}
