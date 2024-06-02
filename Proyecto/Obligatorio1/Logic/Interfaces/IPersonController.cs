using Logic.DTOs;
using Model;

namespace Logic.Interfaces;

public interface IPersonController
{
    public Person CreatePerson(PersonDto personDto);
    public void AddPerson(Person person);
    public List<PersonDto> GetPersonsDto();    
}