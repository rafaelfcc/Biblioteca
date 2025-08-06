using AutoMapper;
using Biblioteca.API.Models;
using Biblioteca.Domain.Entities;

namespace Biblioteca.API.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LivroInputModel, Livro>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.GeneroLivro, opt => opt.Ignore())
                .ForMember(dest => dest.Editora, opt => opt.Ignore());

            CreateMap<Livro, LivroInputModel>();
        }
    }
}
