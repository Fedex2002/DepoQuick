using Model.Enums;

namespace Logic.DTOs
{
    public class AreaTypeDto
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public AreaTypeDto(AreaType areaType)
        {
            Value = (int)areaType;
            Name = areaType.ToString();
        }
    }
}