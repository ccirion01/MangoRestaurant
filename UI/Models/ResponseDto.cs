namespace UI.Models
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ErrorMessages { get; set; }

        public static ResponseDto CreateWithError(string errorMessage)
        {
            return new()
            {
                IsSuccess = false,
                Message = "Error",
                ErrorMessages = new List<string>() { errorMessage }
            };
        }
    }
}
