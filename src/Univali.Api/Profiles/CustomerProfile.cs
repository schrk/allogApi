using AutoMapper;
using Univali.Api.Entities;
using Univali.Api.Features.Customers.Commands.CreateCustomer;
using Univali.Api.Features.Customers.Queries.GetCustomerDetail;

namespace Univali.Api.Profiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        // Velhos
        CreateMap<Entities.Customer, Models.CustomerDto>();
        CreateMap<Models.CustomerForUpdateDto, Entities.Customer>();
        CreateMap<Models.CustomerForCreationDto, Entities.Customer>();
        CreateMap<Entities.Customer, Models.CustomerWithAddressesDto>();

        // Novos
        CreateMap<Customer, GetCustomerDetailDto>();
        CreateMap<CreateCustomerCommand, Customer>();
        CreateMap<Customer, CreateCustomerDto>();
    }
}