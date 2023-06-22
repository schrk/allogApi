using System.ComponentModel.DataAnnotations;
namespace Univali.Api.Models;


// Armazena os dados enviados no corpo da Requisição pelo usuário
// Essa classe é um Parâmetro do método Post
public class AuthenticationRequestDto
{
    [Required(ErrorMessage = "You should fill out a Name")]
    [MaxLength(50, ErrorMessage = "The Username shouldn't have more than 50 characters")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "You should fill out a Password")]
    public string? Password { get; set; }
}
