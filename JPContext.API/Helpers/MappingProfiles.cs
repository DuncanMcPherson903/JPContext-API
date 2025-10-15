using AutoMapper;
using Microsoft.AspNetCore.Identity;
using JPContext.API.DTO;
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

    // Vocabulary Mappings
    CreateMap<Vocabulary, VocabularyDto>();
    CreateMap<VocabularyCreateDto, Vocabulary>();
    CreateMap<VocabularyUpdateDto, Vocabulary>();

        // Example Mappings
    CreateMap<Example, ExampleDto>();
    CreateMap<ExampleCreateDto, Example>();
    CreateMap<ExampleUpdateDto, Example>();

    // Comment Mappings
    CreateMap<Comment, CommentDto>()
      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserProfile.Username));
    CreateMap<CommentCreateDto, Comment>();
    CreateMap<CommentUpdateDto, Comment>();
  }
}
