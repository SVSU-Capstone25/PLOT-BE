/*
    Filename: FixtureInstance.cs
    Part of Project: PLOT/PLOT-BE/Plot/Data/Models/Fixtures

    File Purpose:
    This file contains the object model for the instance
    of a fixture.
    
    Class Purpose:
    This record is used as the main model
    for the instance of a fixture in the application. This will
    look the same as the schema in the database.

    Written by: Jordan Houlihan
*/

namespace Plot.Data.Models.Fixtures;

public class FixtureInstance : IEquatable<FixtureInstance>
{
    public int? TUID { get; set; }
    public int? FLOORSET_TUID { get; set; }
    public string? NAME { get; set; }
    public int? WIDTH { get; set; }
    public int? LENGTH { get; set; }
    public double? X_POS { get; set; }
    public double? Y_POS { get; set; }
    public int? HANGER_STACK { get; set; }
    public double? ALLOCATED_LF { get; set; }
    public float? TOT_LF { get; set; }
    public string? NOTE { get; set; }
    public string? SUPERCATEGORY_NAME { get; set; }
    public string? SUBCATEGORY { get; set; }
    public int? TOTAL_SALES { get; set; }
    public string? COLOR { get; set; }
    public int? EDITOR_ID { get; set; }

    public bool Equals(FixtureInstance? other)
    {
        if (other is null)
        {
            return false;
        }
        return this.TUID == other.TUID;
    }

    public override bool Equals(object? obj) => Equals(obj as FixtureInstance);
    public override int GetHashCode() => (TUID).GetHashCode();
}