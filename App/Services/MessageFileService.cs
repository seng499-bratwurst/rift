using System.Collections.Generic;
using System.Threading.Tasks;
using Rift.Models;
using Rift.Repositories;

namespace Rift.Services
{
    public class MessageFilesService : IMessageFilesService
    {
        private readonly IMessageFilesRepository _repository;

        public MessageFilesService(IMessageFilesRepository repository)
        {
            _repository = repository;
        }

        public async Task InsertMessageFilesAsync(IEnumerable<FileEntityDto> files, int messageId)
        {
            var messageFilesList = new List<MessageFiles>();
            foreach (var file in files)
            {
                messageFilesList.Add(new MessageFiles
                {
                    MessageId = messageId,
                    FileId = file.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }
            await _repository.AddMessageFilesAsync(messageFilesList);
        }
    }
}