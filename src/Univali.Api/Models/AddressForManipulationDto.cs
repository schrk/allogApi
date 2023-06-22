namespace Univali.Api.Models;

public abstract class AddressForManipulationDto
{
    /// <summary>
  /// The name street
  /// </summary>
  public string Street {get; set;} = string.Empty;
    /// <summary>
  /// The name city
  /// </summary>
  public string City {get; set;} = string.Empty;
}
