namespace Comment_Post.ViewModels
{
   
    public class ResultViewModel<T>
    {
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
