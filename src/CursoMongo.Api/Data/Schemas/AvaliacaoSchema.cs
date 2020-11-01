using CursoMongo.Api.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CursoMongo.Api.Data.Schemas
{
    public class AvaliacaoSchema
    {
        public ObjectId Id { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string RestauranteId { get; set; }
        public int Estrelas { get; set; }
        public string Comentario { get; set; }
    }

    public static class AvaliacaoSchemaExtensao
    {
        public static Avaliacao ConverterParaDomain(this AvaliacaoSchema document)
        {
            return new Avaliacao(document.Estrelas, document.Comentario);
        }
    }
}