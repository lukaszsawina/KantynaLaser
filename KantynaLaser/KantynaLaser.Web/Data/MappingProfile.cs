using AutoMapper;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;

namespace KantynaLaser.Web.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserAccount, UserAccountDto>();
        CreateMap<UserAccountDto, UserAccount>();
        CreateMap<Recipe, RecipeDto>();
        CreateMap<RecipeDto, RecipeDto>();
    }
}
