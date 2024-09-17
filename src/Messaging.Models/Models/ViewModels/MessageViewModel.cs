using AutoMapper;
using Messaging.Dal.Models.Entities;
using Messaging.Models.Mapping;

namespace Messaging.Models.Models.ViewModels
{
    public class MessageViewModel : IMapWith<Message>
    {
        public string Message { get; set; }
        public int Number { get; set; }
        //[JsonConverter(typeof(MessagingDateTimeConverter))]
        public DateTime DateTime { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<MessageViewModel, Message>()
                .ForMember(dest => dest.Text, member => member.MapFrom(src => src.Message))
                .ForMember(dest => dest.DateTime, member => member.MapFrom(src => src.DateTime))
                .ForMember(dest => dest.Number, member => member.MapFrom(src => src.Number))
                .ReverseMap();
        }
    }
}
