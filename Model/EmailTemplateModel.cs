
namespace Plot.Model
{
    public class EmailTemplateModel
    {
        public required string Name { get; set; }
        public required string BodyText { get; set; }
        public required string ButtonText { get; set; }
        public required string ButtonLink { get; set; }
        public required string AfterButtonText { get; set; }

    }
}
