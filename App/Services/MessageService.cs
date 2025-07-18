using Rift.Models;
using Rift.Repositories;

namespace Rift.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageFilesRepository _messageFileRepository;
    private readonly IFileRepository _fileRepository;

    public MessageService(IMessageRepository messageRepository, IMessageFilesRepository messageFileRepository, IFileRepository fileRepository)
    {
        _messageFileRepository = messageFileRepository;
        _messageRepository = messageRepository;
        _fileRepository = fileRepository;
    }

    public async Task<Message?> CreateMessageAsync(
        int? conversationId,
        int? promptMessageId,
        string content,
        string role,
        float xCoordinate,
        float yCoordinate
    )
    {
        var message = new Message
        {
            ConversationId = conversationId,
            PromptMessageId = promptMessageId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Role = role,
            XCoordinate = xCoordinate,
            YCoordinate = yCoordinate,
        };
        return await _messageRepository.CreateAsync(message);
    }

    public async Task<List<Message>> GetMessagesForConversationAsync(string userId, int conversationId)
    {
        // Get messages for the conversation
        var messages = await _messageRepository.GetUserConversationMessagesAsync(userId, conversationId) ?? new List<Message>();
        var messageIds = messages.Select(m => m.Id).ToList();

        // Get files associated with the messages
        var messageFiles = await _messageFileRepository.GetMessageFilesByMessageIdsAsync(messageIds) ?? new List<MessageFiles>();
        var fileIds = messageFiles.Select(mf => mf.FileId).Distinct().ToList();

        // Get files from the separate files repository
        var files = await _fileRepository.GetDocumentsByIdsAsync(fileIds) ?? new List<FileEntityDto>();
        var fileDict = files.ToDictionary(f => f.Id);

        // Group messagefiles by message
        var messageFilesByMessageId = messageFiles
            .GroupBy(mf => mf.MessageId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Combine messages with their associated files - Note: this would be much easier if Files were not in a separate database.
        var result = messages.Select(m =>
        {
            var docs = messageFilesByMessageId.TryGetValue(m.Id, out var mfs)
                ? mfs.Select(mf =>
                {
                    if (fileDict.TryGetValue(mf.FileId, out var file))
                    {
                        return new FileEntityDto
                        {
                            Id = file.Id,
                            Name = file.Name,
                            CreatedAt = file.CreatedAt,
                            UploadedBy = file.UploadedBy,
                            SourceLink = file.SourceLink,
                            SourceType = file.SourceType
                        };
                    }
                    return null;
                })
                .OfType<FileEntityDto>()
                .ToList()
                : new List<FileEntityDto>();

            return new Message
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                PromptMessageId = m.PromptMessageId,
                Content = m.Content,
                OncApiQuery = m.OncApiQuery,
                OncApiResponse = m.OncApiResponse,
                IsHelpful = m.IsHelpful,
                Role = m.Role,
                XCoordinate = m.XCoordinate,
                YCoordinate = m.YCoordinate,
                CreatedAt = m.CreatedAt,
                Documents = docs
            };
        }).ToList();

        return result;
    }

    public async Task<List<Message>> GetGuestMessagesForConversationAsync(string sessionId, int conversationId)
    {
        return await _messageRepository.GetGuestConversationMessagesAsync(sessionId, conversationId);
    }

    public async Task<Message?> UpdateMessageAsync(
        int messageId,
        string userId,
        float xCoordinate,
        float yCoordinate
    )
    {
        var message = await _messageRepository.GetByIdAsync(userId, messageId);
        if (message == null)
        {
            return null; // Message not found
        }

        message.XCoordinate = xCoordinate;
        message.YCoordinate = yCoordinate;

        return await _messageRepository.UpdateAsync(message);
    }

    public async Task<Message?> UpdateMessageFeedbackAsync(
        string userId,
        int messageId,
        bool isHelpful
    )
    {
        var message = await _messageRepository.GetByIdAsync(userId, messageId);
        if (message == null)
        {
            return null; // Message not found
        }

        message.IsHelpful = isHelpful;

        return await _messageRepository.UpdateAsync(message);
    }

    public async Task<Message?> DeleteMessageAsync(string userId, int messageId)
    {
        return await _messageRepository.DeleteAsync(userId, messageId);
    }
}