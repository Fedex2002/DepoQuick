using Model.Enums;

namespace Controllers.Dtos
{
    public class SizeTypeDto
    {
        public int Value { get; set; }
        public string Name { get; set; }

        public SizeTypeDto(SizeType sizeType)
        {
            Value = (int)sizeType;
            Name = sizeType.ToString();
        }
    }
}