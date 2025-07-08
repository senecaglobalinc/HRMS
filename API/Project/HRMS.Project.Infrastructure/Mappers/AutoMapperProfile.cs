using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using HRMS.Project.Entities;
using HRMS.Project.Infrastructure.Models.Request;

namespace HRMS.Project.Infrastructure.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProjectRequest, Entities.Project>().ReverseMap();
            CreateMap<SOWRequest, SOW>().ReverseMap();
            CreateMap<ClientBillingRoles, ClientBillingRoles>();
            CreateMap<ClientBillingRolesHistory, ClientBillingRolesHistory>();
            CreateMap<AddendumRequest, Addendum>();
        }
    }
}
