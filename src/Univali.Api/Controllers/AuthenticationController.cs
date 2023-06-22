using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Univali.Api.Models;

namespace Univali.Api.Controllers;
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    // Permite o acesso as informações do arquivo appsettings.Development.json
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    // O método deve ser Post, porque o nome de usuário e senha são enviados por meio do corpo da solicitação.
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationRequestDto authenticationRequestDto)
    {
        var user = ValidateUserCredentials
        (
            authenticationRequestDto.Username!,
            authenticationRequestDto.Password!
        );

        if (user == null)
        {
            return Unauthorized();
        }
        /*
          "Chave secreta / Chave de segurança"

          Para assinar o token precisamos de uma chave de segurança.

          As chaves de segurança são criadas a partir da chave secreta.

          A chave secreta está armazenada no arquivo appsettings.Development.json.

          Em produção a chave secreta deve ser armazenado em um local seguro como
          um serviço tipo Azure KeyVault.

          "SymmetricSecurityKey" cria uma chave de segurança através do segredo.

          Instale "Microsoft.IdentityModel.Tokens"

          Na prática, a chave segurança tem o mesmo valor que a chave secreta,
          a diferença é que é uma instância de SymmetricSecurityKey,
          sendo assim possui mais propriedades.
          */

        var securityKey = new SymmetricSecurityKey
        (
            Encoding.UTF8.GetBytes(
                _configuration["Authentication:SecretKey"]
                    ?? throw new ArgumentNullException(nameof(_configuration))
            )
        );

        /*
        "Credencial de Assinatura"
        Para assinar o token precisamos de uma credencial de assinatura.

        1˚ arg. chave de segurança que foi criada através do segredo, que é um
        objeto SymmetricSecurityKey.

        2˚ arg. O algoritmo de assinatura a ser aplicado.
        */
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // /'Claims/' são informações incluídas no token sobre quem é o usuário.
        // Claims é um conjunto de chave-valor, observe a propriedade "Properties" da classe é um dictionary.
        var claims = new List<Claim>();
        //"Sub" é a identificação de quem o token representa
        claims.Add(new Claim("sub", user.UserId.ToString()));
        // given_name é um padrão também
        claims.Add(new Claim("given_name", user.Name));

        // Define o token
        // Instale System.IdentityModel.Tokens.Jwt
        var jwt = new JwtSecurityToken(
            // Emissor
            _configuration["Authentication:Issuer"],
            // Audience
            _configuration["Authentication:Audience"],
            // Claims
            claims,
            // Indica o início da validade do token. Antes dessa hora o token é invalido.
            DateTime.UtcNow,
            // Indica o fim da validade do token. Depois dessa hora o token é invalido.
            DateTime.UtcNow.AddHours(1),
            // Credencial de assinatura
            signingCredentials
        );
        
        // Cria o token
        // Para escrever o token nós precisamos criar um manipulador e chamar o método WriteToken.
        // Argumento é a definição do token
        var jwtToReturn = new JwtSecurityTokenHandler().WriteToken(jwt);
        return Ok(jwtToReturn);
    }



    private InfoUser? ValidateUserCredentials(string userName, string password)
    {
        // Esse user deve vir do banco de dados
        var userFromDatabase = new Entities.User
        {
            Id = 1,
            Name = "Ada Lovelace",
            UserName = "love",
            Password = "MinhaSenha"
        };

        // Deve comparar o userName e a password  com os dados do usuário no banco
        // ????????? Precisa fazer algo antes de comparar ??????????????
        if (userFromDatabase.UserName == userName && userFromDatabase.Password == password)
        {
            return new InfoUser(userFromDatabase.Id, userName, userFromDatabase.Name);
        }
        return null;
    }

    // Armazena as informações validadas do usuário
    // É o tipo do retorno do método ValidateUserCredentials
    private class InfoUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        // Construtor
        public InfoUser(int userId, string userName, string name)
        {
            UserId = userId;
            UserName = userName;
            Name = name;
        }
    }


}
