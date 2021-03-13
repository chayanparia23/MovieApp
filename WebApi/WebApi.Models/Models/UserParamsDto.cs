using WebApi.Models.Helpers;

namespace WebApi.Models.Models
{
    public class UserParamsDto: PaginationParams
    {
        public string SearchText { get; set; }
        public string SearchTextType { get; set; }
    }
}
