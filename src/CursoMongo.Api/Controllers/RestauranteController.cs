using System.Linq;
using System.Threading.Tasks;
using CursoMongo.Api.Controllers.Inputs;
using CursoMongo.Api.Controllers.Outputs;
using CursoMongo.Api.Data.Repositories;
using CursoMongo.Api.Domain.Entities;
using CursoMongo.Api.Domain.Enums;
using CursoMongo.Api.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace CursoMongo.Api.Controllers
{
    [ApiController]
    public class RestauranteController : ControllerBase
    {
        private readonly RestauranteRepository _restauranteRepository;

        public RestauranteController(RestauranteRepository restauranteRepository)
        {
            _restauranteRepository = restauranteRepository;
        }

        [HttpPost("restaurante")]
        public ActionResult IncluirRestaurante([FromBody] RestauranteInclusao restauranteInclusao)
        {
            var cozinha = ECozinhaHelper.ConverterDeInteiro(restauranteInclusao.Cozinha);

            var restaurante = new Restaurante(restauranteInclusao.Nome, cozinha);
            var endereco = new Endereco(
                restauranteInclusao.Logradouro,
                restauranteInclusao.Numero,
                restauranteInclusao.Cidade,
                restauranteInclusao.UF,
                restauranteInclusao.Cep);

            restaurante.AtribuirEndereco(endereco);

            if (!restaurante.Validar())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurante.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                    });
            }

            _restauranteRepository.Inserir(restaurante);

            return Ok(
                new
                {
                    data = "Restaurante inserido com sucesso"
                }
            );
        }

        [HttpGet("restaurante/todos")]
        public async Task<ActionResult> ObterRestaurantes()
        {
            var restaurantes = await _restauranteRepository.ObterTodos();

            var listagem = restaurantes.Select(_ => new RestauranteListagem
            {
                Id = _.Id,
                Nome = _.Nome,
                Cozinha = (int)_.Cozinha,
                Cidade = _.Endereco.Cidade
            });

            return Ok(
                new
                {
                    data = listagem
                }
            );
        }

        [HttpGet("restaurante/{id}")]
        public ActionResult ObterRestaurante(string id)
        {
            var restaurante = _restauranteRepository.ObterPorId(id);

            if (restaurante == null)
                return NotFound();

            var exibicao = new RestauranteExibicao
            {
                Id = restaurante.Id,
                Nome = restaurante.Nome,
                Cozinha = (int)restaurante.Cozinha,
                Endereco = new EnderecoExibicao
                {
                    Logradouro = restaurante.Endereco.Logradouro,
                    Numero = restaurante.Endereco.Numero,
                    Cidade = restaurante.Endereco.Cidade,
                    Cep = restaurante.Endereco.Cep,
                    UF = restaurante.Endereco.UF
                }
            };

            return Ok(
                new
                {
                    data = exibicao
                }
            );
        }

        [HttpPut("restaurante")]
        public ActionResult AlterarRestaurante([FromBody] RestauranteAlteracaoCompleta restauranteAlteracaoCompleta)
        {
            var restaurante = _restauranteRepository.ObterPorId(restauranteAlteracaoCompleta.Id);

            if (restaurante == null)
                return NotFound();

            var cozinha = ECozinhaHelper.ConverterDeInteiro(restauranteAlteracaoCompleta.Cozinha);
            restaurante = new Restaurante(restauranteAlteracaoCompleta.Id, restauranteAlteracaoCompleta.Nome, cozinha);
            var endereco = new Endereco(
                restauranteAlteracaoCompleta.Logradouro,
                restauranteAlteracaoCompleta.Numero,
                restauranteAlteracaoCompleta.Cidade,
                restauranteAlteracaoCompleta.UF,
                restauranteAlteracaoCompleta.Cep);

            restaurante.AtribuirEndereco(endereco);

            if (!restaurante.Validar())
            {
                return BadRequest(
                    new
                    {
                        errors = restaurante.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                    });
            }

            if (!_restauranteRepository.AlterarCompleto(restaurante))
            {
                return BadRequest(new
                {
                    errors = "Nenhum documento foi alterado"
                });
            }

            return Ok(
                new
                {
                    data = "Restaurante alterado com sucesso"
                }
            );
        }

        [HttpPatch("restaurante/{id}")]
        public ActionResult AlterarCozinha(string id, [FromBody] RestauranteAlteracaoParcial restauranteAlteracaoParcial)
        {
            var restaurante = _restauranteRepository.ObterPorId(id);

            if (restaurante == null)
                return NotFound();

            var cozinha = ECozinhaHelper.ConverterDeInteiro(restauranteAlteracaoParcial.Cozinha);

            if (!_restauranteRepository.AlterarCozinha(id, cozinha))
            {
                return BadRequest(new
                {
                    errors = "Nenhum documento foi alterado"
                });
            }

            return Ok(
                new
                {
                    data = "Restaurante alterado com sucesso"
                }
            );
        }

        [HttpGet("restaurante")]
        public ActionResult ObterRestaurantePorNome([FromQuery] string nome)
        {
            var restaurantes = _restauranteRepository.ObterPorNome(nome);

             var listagem = restaurantes.Select(_ => new RestauranteListagem
            {
                Id = _.Id,
                Nome = _.Nome,
                Cozinha = (int)_.Cozinha,
                Cidade = _.Endereco.Cidade
            });

            return Ok(
                new
                {
                    data = listagem
                }
            );
        }

        [HttpPatch("restaurante/{id}/avaliar")]
        public ActionResult AvaliarRestaurante(string id, [FromBody] AvaliacaoInclusao avaliacaoInclusao)
        {
            var restaurante = _restauranteRepository.ObterPorId(id);

            if (restaurante == null)
                return NotFound();

            var avaliacao = new Avaliacao(avaliacaoInclusao.Estrelas, avaliacaoInclusao.Comentario);

            if (!avaliacao.Validar())
            {
                return BadRequest(
                    new
                    {
                        errors = avaliacao.ValidationResult.Errors.Select(_ => _.ErrorMessage)
                    });
            }

            _restauranteRepository.Avaliar(id, avaliacao);

            return Ok(
                new
                {
                    data = "Restaurante avaliado com sucesso"
                }
            );
        }
    }
}
