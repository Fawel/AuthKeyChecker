using AutoMapper;
using KeyChecker.Api.Models;
using KeyChecker.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KeyChecker.Api.AutoMapperProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ValidateKeyRequest, ApplicationCodeAuthKeyValidateRequest>()
                .ForMember(
                    z => z.Key,
                    x =>
                    {
                        x.DoNotAllowNull();
                        x.MapFrom(y => y.ApplicationCode);
                    })
                .ForMember(
                    z => z.RequestingApplicationCode,
                    x =>
                    {
                        x.DoNotAllowNull();
                        x.MapFrom(y => y.ApplicationCode);
                    })
                .ForMember(
                    z => z.TargetApplicationCode,
                    x =>
                    {
                        x.DoNotAllowNull();
                        x.MapFrom(y => y.TargetApplicationCode);
                    });
        }
    }
}
