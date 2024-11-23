using web_app_domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_app_repository
{
    public class HealthRepository : IConsumoRepository
    {
        
        private readonly List<Consumo> _consumos;

        public HealthRepository()
        {
           
            _consumos = new List<Consumo>
            {
                new Consumo { Id = 1, RegistroData = "Consumo A", ValorConsumo = "100.0" },
                new Consumo { Id = 2, RegistroData = "Consumo B", ValorConsumo = "200.0" }
            };
        }

  
        public Task<IEnumerable<Consumo>> ListConsumptions()
        {
          
            return Task.FromResult<IEnumerable<Consumo>>(_consumos);
        }

        
        public Task SaveConsumption(Consumo consumption)
        {
            
            _consumos.Add(consumption);
            return Task.CompletedTask;
        }

       
        public Task UpdateConsumption(Consumo consumption)
        {
            
            var existingConsumption = _consumos.FirstOrDefault(c => c.Id == consumption.Id);
            if (existingConsumption != null)
            {
               
                existingConsumption.RegistroData = consumption.RegistroData;
                existingConsumption.ValorConsumo = consumption.ValorConsumo;
            }
            return Task.CompletedTask;
        }

        
        public Task RemoveConsumption(string id)
        {
            
            if (int.TryParse(id, out int consumoId))
            {
                var consumptionToRemove = _consumos.FirstOrDefault(c => c.Id == consumoId);
                if (consumptionToRemove != null)
                {
                    
                    _consumos.Remove(consumptionToRemove);
                }
            }
            return Task.CompletedTask;
        }
    }
}
