namespace CursoMongo.Api.Controllers.Inputs
{
    public class RestauranteAlteracaoCompleta
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public int Cozinha { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Cep { get; set; }
    }
}