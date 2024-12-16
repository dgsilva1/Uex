using TesteUex.Entities;
using TesteUex.Extensions;
using TesteUex.Models.AccountModels;
using AutoMapper;
using TesteUex.Models.ContactModels;
using TesteUex.Models.UserModels;

namespace TesteUex.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserModel>();
            CreateMap<ContactUpdateModel, Contact>();
            CreateMap<RegisterModel, User>();
            CreateMap<Contact, ContactModel>();
            CreateMap<ContactModel, Contact>();
        }
    }
}
