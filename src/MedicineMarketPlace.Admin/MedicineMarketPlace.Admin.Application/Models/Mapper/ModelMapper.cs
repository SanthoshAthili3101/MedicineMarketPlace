﻿using AutoMapper;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.Shared.Entities;
using Newtonsoft.Json;

namespace MedicineMarketPlace.Admin.Application.Models
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            AllowNullCollections = true;
            AllowNullDestinationValues = true;
            CreateMap<ApplicationUser, LoginUserDto>();
            CreateMap<CreateUserDto, ApplicationUser>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName.Trim()));

            CreateMap<UpdateUserDto, ApplicationUser>();

            //TaxStatus
            CreateMap<TaxStatus, TaxStatusDto>();
            CreateMap<CreateOrUpdateTaxStatusDto, TaxStatus>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Trim()));
        }
        private string SetNullValue()
        {
            return null;
        }

        private string GetSerializedData(List<string> data)
        {
            return data?.Any() == true ? JsonConvert.SerializeObject(data) : null;
        }

        private string GetSerializedData(List<int> data)
        {
            return data?.Any() == true ? JsonConvert.SerializeObject(data) : null;
        }
    }
}
