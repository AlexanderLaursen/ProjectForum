namespace WebApi.Models
{
    public class ResultDto<T>
    {
        public T Data { get; set; }

        public ResultDto()
        {
        }
        public ResultDto(T data)
        {
            Data = data;
        }
    }
}
