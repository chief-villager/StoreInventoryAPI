using Mapster;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Reflection;
using storeInventoryApi.Models.DTO;
using storeInventoryApi.Models;

namespace storeInventoryApi.Config
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<EditProductDto, Products>()
                .Map(dest => dest.Id, src => src.Id )
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Price, src => src.Price);
               
        }
    }
}
