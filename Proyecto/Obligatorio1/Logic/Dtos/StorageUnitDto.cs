using Model;
using Model.Enums;
namespace Logic.DTOs;


public class StorageUnitDto
{
    public string Id { get; set; }
    public AreaType Area { get; set; }
    public SizeType Size { get; set; }
    public bool Climatization { get; set; }
    public List<Promotion>? Promotions { get; set; }

    public StorageUnitDto()
    {
    }

    public StorageUnitDto(string id, AreaType area, SizeType size, bool climatization, List<Promotion> promotions)
    {
        Id = id;
        Area = area;
        Size = size;
        Climatization = climatization;
        Promotions = promotions;
    }
}
