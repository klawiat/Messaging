using AutoMapper;
using Messaging.Dal.Models.Entities;
using Messaging.Models.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Messaging.Models.Models.ViewModels
{
    public class SendMessageRequest : IMapWith<Message>
    {
        [Required(ErrorMessage = "Обязательное свойство")]
        [DisplayName("Текст сообщения")]
        [StringLength(128, ErrorMessage = "Строка не может быть длиннее 128 символов")]
        public string Text { get; set; }
        [DisplayName("Порядковый номер")]
        public int Number { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<SendMessageRequest, Message>()
                .ForMember(dest => dest.Text, member => member.MapFrom(src => src.Text))
                .ForMember(dest => dest.Number, member => member.MapFrom(src => src.Number))
                .ReverseMap();
        }
    }
}
