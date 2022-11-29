namespace Mango.Services.ShoppingCartAPI.Models.Dto.Response
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; } = true;
        public T Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }

        public void AddExceptionMessage(Exception ex)
        {
            ErrorMessages ??= new List<string>();
            ErrorMessages.Add(ex.ToString());
        }
    }
}
