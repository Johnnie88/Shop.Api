using Microsoft.AspNetCore.Mvc;

//Endpoint = URL
//https://localhost:5001/categories/
//http://localhost:5000
//https://meuapp.azurewebsites.bet/

[Route("categories")]
public class CategoryController : ControllerBase
{
    [Route("")]
    public string MeuMetodo()
    {
        return "Olá mundo";
    }
}