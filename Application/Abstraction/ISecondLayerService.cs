using PuppeteerSharp;
using Scout.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface ISecondLayerService
    {
        /// <summary>
        /// Create New Page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Task<IPage> CreatePageAsync(string url);

        /// <summary>
        /// Scroll down page until end.
        /// </summary>
        /// <param name="currentPage"></param>
        /// <returns>Get Production List</returns>
        Task<List<string>> TraversePageAsync(IPage currentPage);

        /// <summary>
        /// Click Next Page Async
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns>CommandResult</returns>
        Task<CommandResult> ClickNextPageAsync(IPage CurrentPage);

        /// <summary>
        /// Click Next Page Async
        /// </summary>
        /// <param name="CurrentPage"></param>
        /// <returns>CommandResult</returns>
        Task<CommandResult> CheckLastPageAsync(IPage CurrentPage);
    }
}
