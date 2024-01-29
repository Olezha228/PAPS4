namespace PAPS4.Controllers
{
    public interface IWellService
    {
        IEnumerable<WellDto> GetWells();
        WellDto GetWellById(int id);
        WellDto CreateWell(WellDto wellDto);
        WellDto UpdateWell(int id, WellDto wellDto);
        void DeleteWell(int id);
    }

    public class WellDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Depth { get; set; }
    }

    public class WellStatistics
    {
        public int TotalWells { get; set; }
        public double AverageDepth { get; set; }
        // Добавьте дополнительные поля статистики по необходимости

        public WellStatistics(int totalWells, double averageDepth)
        {
            TotalWells = totalWells;
            AverageDepth = averageDepth;
        }

        public WellStatistics()
        {
            // Пустой конструктор для сериализации
        }
    }

    public class WellService : IWellService
    {
        private readonly List<WellDto> _wells = new();
        private int _idCounter = 1;

        public IEnumerable<WellDto> GetWells()
        {
            return _wells;
        }

        public WellDto GetWellById(int id)
        {
            return _wells.FirstOrDefault(w => w.Id == id);
        }

        public WellDto CreateWell(WellDto wellDto)
        {
            wellDto.Id = _idCounter++;
            _wells.Add(wellDto);
            return wellDto;
        }

        public WellDto UpdateWell(int id, WellDto wellDto)
        {
            var existingWell = _wells.FirstOrDefault(w => w.Id == id);
            if (existingWell == null)
                return null;

            existingWell.Name = wellDto.Name;
            existingWell.Depth = wellDto.Depth;

            return existingWell;
        }

        public void DeleteWell(int id)
        {
            var wellToRemove = _wells.FirstOrDefault(w => w.Id == id);
            if (wellToRemove != null)
                _wells.Remove(wellToRemove);
        }
    }
}
