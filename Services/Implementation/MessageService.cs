using AutoMapper;
using RealEstate.Services;
using RealEStateProject.Models;
using RealEStateProject.Repositories.Interfaces;
using RealEStateProject.Services.Interfaces;
using RealEStateProject.ViewModels.Property;

namespace RealEStateProject.Services.Implementation
{
    public class MessageService : BaseService<Message, Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
            : base(messageRepository, mapper)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _messageRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Message>> GetMessagesByAgentIdAsync(string agentId)
        {
            return await _messageRepository.GetByAgentIdAsync(agentId);
        }


        public async Task SendContactMessageAsync(ContactAgentViewModel model)
        {
            var message = new Message
            {
                PropertyId = model.PropertyId,
                AgentId = model.AgentId,
                SenderName = model.ContactName,
                SenderEmail = model.ContactEmail,
                SenderPhone = model.ContactPhone,
                MessageText = model.ContactMessage,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageRepository.AddAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task UpdateMessageAsync(Message message)
        {
            await _messageRepository.UpdateAsync(message);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task DeleteMessageAsync(int id)
        {
            await _messageRepository.DeleteAsync(id);
            await _messageRepository.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);
            if (message != null)
            {
                message.IsRead = true;
                await _messageRepository.UpdateAsync(message);
                await _messageRepository.SaveChangesAsync();
            }
        }
    }
}

