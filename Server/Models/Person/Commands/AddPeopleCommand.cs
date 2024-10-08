﻿using MediatR;
using Server.Models.Person;

namespace Server.Models.Person.Commands
{
    public class AddPeopleCommand : IRequest<PeopleCommandResult>
    {
        public List<Person> People { get; private set; }

        public AddPeopleCommand(List<Person> people)
        {
            People = people;
        }
    }
}
