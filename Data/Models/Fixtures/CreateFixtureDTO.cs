namespace Plot.Data.Models.Fixtures;

public class CreateFixtureInstanceDTO
{
    public Fixture? Fixture { get; set; }
    public int? FloorsetID { get; set; }
    public double? XPosition { get; set; }
    public double? YPosition { get; set; }
}