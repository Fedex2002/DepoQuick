using Logic.DTOs;
using Model;

using Model.Enums;


namespace Logic.Interfaces;

public interface IStorageUnitController
{
    public void CreateStorageUnit(StorageUnitDto storageUnitDto);
    public void RemoveStorageUnit(StorageUnitDto storageUnitDto);
    public List<StorageUnitDto> GetStorageUnitsDto();
    public StorageUnitDto GetStorageUnitDtoFromId(string id);
    public void DeletePromotionFromAllStorageUnits(PromotionDto promotionDto);
    public double CalculateStorageUnitPricePerDay(StorageUnitDto storageUnitDto, DateRangeDto dateRangeDto);
    public void AddAvailableDateRangeToStorageUnit(string id, DateRangeDto dateRangeDto);
    public List<StorageUnitDto> SearchAvailableStorageUnits(DateRangeDto dateRangeDto);

    public AreaTypeDto ConvertAreaTypeToAreaTypeDto(AreaType areaType);
    public SizeTypeDto ConvertSizeTypeToSizeTypeDto(SizeType sizeType);

}