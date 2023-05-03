using PuppeteerSharp;
using Scout.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IZeroLayerService
    {
        /// <summary>
        /// Create Initial Page
        /// </summary>
        /// <returns></returns>
        Task<IPage> CreatePageAsync();

        /// <summary>
        /// Get Category Url List
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns></returns>
        Task<CommandResult> GetCategoryUrlsAsync(IPage CurrentPage);

        /// <summary>
        /// Get Category Name List
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns></returns>
        Task<CommandResult> GetCategoryNamesAsync(IPage CurrentPage);
    }
}
