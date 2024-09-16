using Messaging.Api.Models.Entities.BaseEntities;

namespace Messaging.Api.Models.Entities
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public int Number { get; set; }
    }
}
