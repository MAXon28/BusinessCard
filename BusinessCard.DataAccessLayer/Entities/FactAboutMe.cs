using SqlQueryStrings.Annotations;

namespace BusinessCard.DataAccessLayer.Entities
{
    [SqlTable("FactsAboutMe")]
    public class FactAboutMe
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public int Priority { get; set; }

        [SqlServerForeignKey("TypesOfFacts", "TypeId")]
        public TypeOfFact Type { get; set; }
    }
}