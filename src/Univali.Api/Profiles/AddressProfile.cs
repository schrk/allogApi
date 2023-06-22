using AutoMapper;

namespace Univali.Api.Profiles;

public class AddressProfile : Profile
{
    public AddressProfile()
    {
        CreateMap<Entities.Address, Models.AddressDto>();
        CreateMap<Models.AddressForUpdateDto, Entities.Address>();
    }
}