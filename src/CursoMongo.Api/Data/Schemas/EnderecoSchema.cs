namespace CursoMongo.Api.Data.Schemas
{
    public class EnderecoSchema
    {
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Cep { get; set; }
    }
}