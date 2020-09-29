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
                .ConvertUsing(x => new ApplicationCodeAuthKeyValidateRequest(
                    x.ApplicationCode,
                    x.TargetApplicationCode,
                    x.AuthKeyValue));
        }
    }
}
