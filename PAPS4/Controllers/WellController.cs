using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace PAPS4.Controllers
{
    [ApiController]
    [Route("api/well")]
    public class WellController : ControllerBase
    {
        private readonly IWellService _wellService;

        public WellController(IWellService wellService)
        {
            _wellService = wellService;
            var wellGenerator = new WellGenerator();
            var numberOfWellsToGenerate = 50;

            var generatedWells = wellGenerator.GenerateWells(numberOfWellsToGenerate);
            foreach (var generatedWell in generatedWells)
            {
                _wellService.CreateWell(generatedWell);
            }
        }

        [HttpGet("statistics")]
        public ActionResult<WellStatistics> GetWellStatistics()
        {
            var wellStatistics = new WellStatistics
            {
                TotalWells = _wellService.GetWells().Count(),
                AverageDepth = _wellService.GetWells().Average(w => w.Depth)
            };

            return Ok(wellStatistics);
        }

        [HttpGet]
        public ActionResult<IEnumerable<WellDto>> GetWells([FromQuery] string sortField)
        {
            var wells = _wellService.GetWells();

            // Проверяем, указан ли параметр сортировки
            if (sortField is not null)
            {
                // Применяем сортировку в зависимости от переданного поля
                switch (sortField.ToLower())
                {
                    case "name":
                        wells = wells.OrderBy(w => w.Name).ToList();
                        break;
                    case "id":
                        wells = wells.OrderBy(w => w.Id).ToList();
                        break;
                    // Добавьте дополнительные кейсы для других полей
                    default:
                        // Если указано некорректное поле, игнорируем параметр сортировки
                        break;
                }
            }

            return Ok(wells);
        }

        [HttpGet("filter")]
        public ActionResult<IEnumerable<WellDto>> GetWellsByDepth([FromQuery] double minDepth, [FromQuery] double maxDepth)
        {
            var filteredWells = _wellService.GetWells().Where(w => w.Depth >= minDepth && w.Depth <= maxDepth).ToList();

            return Ok(filteredWells);
        }

        [HttpPost]
        public ActionResult<WellDto> CreateWell([FromBody] WellDto wellDto)
        {
            var createdWell = _wellService.CreateWell(wellDto);
            return CreatedAtAction(nameof(GetWellById), new { id = createdWell.Id }, createdWell);
        }

        [HttpGet("{id}")]
        public ActionResult<WellDto> GetWellById(int id)
        {
            var well = _wellService.GetWellById(id);
            if (well == null)
                return NotFound();

            return Ok(well);
        }

        [HttpPut("{id}")]
        public ActionResult<WellDto> UpdateWell(int id, [FromBody] WellDto wellDto)
        {
            var updatedWell = _wellService.UpdateWell(id, wellDto);
            if (updatedWell == null)
                return NotFound();

            return Ok(updatedWell);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWell(int id)
        {
            _wellService.DeleteWell(id);
            return Ok(new { message = "Well deleted successfully." });
        }
    }
}
