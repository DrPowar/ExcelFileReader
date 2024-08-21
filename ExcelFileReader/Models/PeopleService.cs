using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelFileReader.Models
{
    internal class PeopleService
    {
        private readonly ISourceCache<Person, int> _people;
        private ISourceCache<Person, int> _tempPeople;

        public PeopleService()
        {
            _people = new SourceCache<Person, int>(e => (int)e.Number);
            _tempPeople = new SourceCache<Person, int>(e => (int)e.Number);
        }

        public IObservable<IChangeSet<Person, int>>
               PeopleConnection() => _people.Connect();

        public void LoadData(IEnumerable<Person> people) =>
            _people.AddOrUpdate(people);

        public void UpdateData(IEnumerable<Person> people) =>
            _people.Remove(people);


        public void ClearData() =>
            _people.Clear();

        public List<Person> GetPeople() =>
            _people.Items.ToList();

        public List<Person> GetTempPeople() =>
            _tempPeople.Items.ToList();

        public void SavePeopleToTemp()
        {
            _tempPeople.AddOrUpdate(GetPeople());
        }

        public void RestorePeopleFromTemp() =>
            LoadData(_tempPeople.Items);

    }
}
