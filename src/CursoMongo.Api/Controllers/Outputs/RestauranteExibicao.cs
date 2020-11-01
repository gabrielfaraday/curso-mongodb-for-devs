namespace CursoMongo.Api.Controllers.Outputs
{
    public class RestauranteExibicao
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public int Cozinha { get; set; }
        public EnderecoExibicao Endereco { get; set; }
    }
}