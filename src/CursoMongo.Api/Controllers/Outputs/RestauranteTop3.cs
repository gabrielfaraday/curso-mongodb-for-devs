namespace CursoMongo.Api.Controllers.Outputs
{
    public class RestauranteTop3
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public int Cozinha { get; set; }
        public string Cidade { get; set; }
        public double Estrelas { get; set; }
    }
}