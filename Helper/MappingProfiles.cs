using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ReviewApp.Dto;
using ReviewApp.Models;

namespace ReviewApp.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Review, ReviewDto>();
        CreateMap<ReviewDto, Review>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CommentDto, Comment>();
        CreateMap<Tag, TagDto>();
        CreateMap<TagDto, Tag>();
        CreateMap<Like, LikeDto>();
        CreateMap<LikeDto, Like>();
        CreateMap<Piece, PieceDto>();
        CreateMap<PieceDto, Piece>();


    }
}
