using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelFileReader.Services
{
    internal class CacheService<T, TKey>
    {
        private readonly ISourceCache<T, TKey> _data;
        private ISourceCache<T, TKey> _tempData;

        public CacheService(Func<T, TKey> keySelector)
        {
            _data = new SourceCache<T, TKey>(keySelector);
            _tempData = new SourceCache<T, TKey>(keySelector);
        }

        public IObservable<IChangeSet<T, TKey>> DataConnection() => _data.Connect();

        public void LoadData(IEnumerable<T> items)
        {
            _data.AddOrUpdate(items);
        }

        public void RemoveData(IEnumerable<T> items)
        {
            _tempData.Remove(items);
            _data.Remove(items);
        }

        public void ClearData() => _data.Clear();

        public List<T> GetData() => _data.Items.ToList();

        public List<T> GetTempData() => _tempData.Items.ToList();

        public void SaveDataToTemp()
        {
            _tempData.AddOrUpdate(GetData());
        }

        public void RestoreDataFromTemp()
        {
            LoadData(_tempData.Items);
        }

        public void FullClear()
        {
            _tempData.Clear();
            _data.Clear();
        }
    }
}
