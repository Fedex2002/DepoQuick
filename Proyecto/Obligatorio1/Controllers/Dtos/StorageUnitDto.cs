using Model;
using Model.Enums;
namespace Logic.DTOs;


public class StorageUnitDto
{
    public string Id { get; set; }
    public AreaTypeDto Area { get; set; }
    public SizeType Size { get; set; }
    public bool Climatization { get; set; }
    public List<PromotionDto>? Promotions { get; set; }
    public List<DateRangeDto> AvailableDates { get; set; }

    public StorageUnitDto()
    {
    }

    public StorageUnitDto(string id, AreaTypeDto area, SizeType size, bool climatization, List<PromotionDto> promotions, List<DateRangeDto> availableDates)
    {
        Id = id;
        Area = area;
        Size = size;
        Climatization = climatization;
        Promotions = promotions;
        AvailableDates = availableDates;
    }
}
