using AutoMapper;

namespace Messaging.Models.Mapping
{
    public interface IMapWith<T>
    {
        public void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T)).ReverseMap();
    }
}
