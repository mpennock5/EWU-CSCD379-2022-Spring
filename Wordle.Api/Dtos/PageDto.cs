using Wordle.Api.Data;

namespace Wordle.Api.Dtos
{
    public class PageDto
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPages { get; set; }
        public int PageSize { get; set; }
        public List<Tuple<string, bool>>? returnable { get; set; }

       
    }
}
