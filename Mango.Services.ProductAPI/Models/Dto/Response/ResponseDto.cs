namespace Mango.Services.ProductAPI.Models.Dto.Response
{
    public class ResponseDto<T>
    {
        public bool IsSuccess { get; set; } = true;
        public T Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }

        public void AddErrorMessage(string message)
        {
            if (ErrorMessages == null)
                ErrorMessages = new List<string>();

            ErrorMessages.Add(message);
        }
    }
}
