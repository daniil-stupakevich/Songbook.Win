using Songbook.Win.Core.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Songbook.Win.Persistent.Services.Interfaces
{
    public interface IConfigurationPersistentService
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();
    }
}
