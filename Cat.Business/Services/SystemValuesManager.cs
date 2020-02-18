using System;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Cat.Domain.Entities.SystemValues;
using Cat.Domain.Repositories;

namespace Cat.Business.Services
{
    public interface ISystemValuesManager
    {
        object Get(string descriminator);

        Task<object> GetAsync(string descriminator);

        void Set(object value, string descriminator, SystemValueType type);

        Task SetAsync(object value, string descriminator, SystemValueType type);

        void Remove(string descriminator);

        Task RemoveAsync(string descriminator);
    }

    public class SystemValuesManager : ISystemValuesManager
    {
        private readonly ISystemValuesRespository _valuesRepo;

        public SystemValuesManager(ISystemValuesRespository valuesRepo)
        {
            _valuesRepo = valuesRepo;
        }


        public object Get(string descriminator)
        {
            var storedValue = GetStoredValue(descriminator);
            return GetValue(storedValue);
        }

        public async Task<object> GetAsync(string descriminator)
        {
            var storedValue = await GetStoredValueAsync(descriminator);
            return GetValue(storedValue);
        }

        public void Set(object value, string descriminator, SystemValueType type)
        {
            var storedValue = GetStoredValue(descriminator);
            SetValue(storedValue, value, descriminator, type);
            _valuesRepo.SaveChanges();
        }

        public async Task SetAsync(object value, string descriminator, SystemValueType type)
        {
            var storedValue = await GetStoredValueAsync(descriminator);
            SetValue(storedValue, value, descriminator, type);
            await _valuesRepo.SaveChangesAsync();
        }

        public void Remove(string descriminator)
        {
            var stroredValue = GetStoredValue(descriminator);
            if (stroredValue == null) return;
            _valuesRepo.Remove(stroredValue.Id);
            _valuesRepo.SaveChanges();
        }

        public async Task RemoveAsync(string descriminator)
        {
            var stroredValue = await GetStoredValueAsync(descriminator);
            if (stroredValue == null) return;
            await _valuesRepo.RemoveAsync(stroredValue.Id);
            await _valuesRepo.SaveChangesAsync();
        }



        private object GetValue(SystemValue systemValue)
        {
            return systemValue == null ? null : DeserializeValue(systemValue.Data, systemValue.DataType);
        }

        private void SetValue(SystemValue systemValue, object value, string descriminator, SystemValueType type)
        {
            if (systemValue == null)
            {
                var sysValue = new SystemValue
                {
                    Data = SerializeValue(value),
                    DataDescriptor = descriminator,
                    DataType = type
                };
                _valuesRepo.Add(sysValue);
            }
            else
            {
                systemValue.Data = SerializeValue(value);
                systemValue.DataDescriptor = descriminator;
                systemValue.DataType = type;
                _valuesRepo.Update(systemValue);
            }
        }

        private SystemValue GetStoredValue(string descriminator)
        {
            return _valuesRepo.GetAll().FirstOrDefault(x => x.DataDescriptor == descriminator);
        }

        private async Task<SystemValue> GetStoredValueAsync(string descriminator)
        {
            return await _valuesRepo.GetAll().FirstOrDefaultAsync(x => x.DataDescriptor == descriminator);
        }

        private string SerializeValue(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        private object DeserializeValue(string value, SystemValueType type)
        {
            switch (type)
            {
                case SystemValueType.Boolean:
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Boolean>(value);
                case SystemValueType.String:
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<String>(value);
                case SystemValueType.Int:
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<Int32>(value);
                case SystemValueType.DateTime:
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DateTime>(value);
                case SystemValueType.TimeSpan:
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSpan>(value);
                case SystemValueType.Unknown:
                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }
        }
    }
}
