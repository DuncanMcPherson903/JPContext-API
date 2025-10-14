using AutoMapper;
using Microsoft.AspNetCore.Identity;
using JPContext.API.DTOs;
using JPContext.API.Models;

namespace JPContext.API.Helpers;

public class MappingProfiles : Profile
{
  public MappingProfiles()
  {
    // User Profile mappings
    CreateMap<UserProfile, UserProfileDto>();
    CreateMap<RegistrationDto, UserProfile>();
    CreateMap<UserUpdateDto, UserProfile>();
    CreateMap<UserUpdateDto, UserUpdateResponseDto>()
      .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
  }
}
