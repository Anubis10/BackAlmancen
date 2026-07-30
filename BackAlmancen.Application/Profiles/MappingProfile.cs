

namespace BackAlmancen.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductoDto, Producto>().ReverseMap();
        }
    }
}
