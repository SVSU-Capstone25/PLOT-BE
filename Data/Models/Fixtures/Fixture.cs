namespace Plot.Data.Models.Fixtures;

public class Fixture
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public float? LFCapacity { get; set; }
    public int? HangerStack { get; set; }
    public float? LFTotal { get; set; }
    public int? StoreId { get; set; }
    public IFormFile? FixtureImage { get; set; }
}