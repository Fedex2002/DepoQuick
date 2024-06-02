using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPersonController
{
    public Person CreatePerson(PersonDto personDto);
    public void AddPerson(PersonDto personDto);
    public List<PersonDto> GetPersonsDto();    
}