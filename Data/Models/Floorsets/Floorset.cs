namespace Plot.Data.Models.Floorsets;

public class Floorset
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int? StoreId { get; set; }
    public DateTime? DateCreated { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public int? ModifiedBy { get; set; }
}