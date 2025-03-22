using RealEStateProject.Models;
using RealEstate.Services;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Services.Interfaces
{
    public interface IMessageService : IBaseService<Message, Message>
    {
        Task<Message> GetMessageByIdAsync(int id);
        Task<IEnumerable<Message>> GetMessagesByAgentIdAsync(string agentId);
        Task SendContactMessageAsync(ContactAgentViewModel model);
        Task MarkAsReadAsync(int id);
    }
}
