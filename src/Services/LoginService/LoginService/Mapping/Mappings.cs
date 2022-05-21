using AutoMapper;

using LoginService.Entities;
using LoginService.Models;

namespace LoginService.Mapping
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            #region User

            CreateMap<User, UserSaveModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<UserSaveModel, User>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            CreateMap<User, UserViewModel>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                ;

            #endregion User
        }
    }
}