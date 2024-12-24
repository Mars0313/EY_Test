namespace EY_Test.Models
{
    public class MyException: Exception
    {

        public MyException(int statusCode, string messgage)
        {
            StatusCode = statusCode;
            MSG = messgage;
        }
        public int StatusCode { get; set; }
        public string MSG { get; set; }
    }
}
