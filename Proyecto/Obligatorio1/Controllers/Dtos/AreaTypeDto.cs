using Model.Enums;

namespace Controllers.Dtos
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