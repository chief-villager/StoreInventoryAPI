namespace storeInventoryApi.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }

     
        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
            Message = "Operation successful.";
        }
         
        public ApiResponse(string message, bool isSuccess )
        {
            Success = isSuccess;
            Message = message;
            Data = default;

        }

        
    }

}
