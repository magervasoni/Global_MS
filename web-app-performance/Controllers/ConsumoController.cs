using Microsoft.AspNetCore.Mvc;
using web_app_domain;
using web_app_repository;

[Route("api/[controller]")]
[ApiController]
public class ConsumptionController : ControllerBase
{
    private readonly IConsumoRepository _repository;

    public ConsumptionController(IConsumoRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetConsumption()
    {
        var consumptions = await _repository.ListConsumptions();
        if (consumptions == null || !consumptions.Any())
            return NotFound();

        return Ok(consumptions);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Consumo consumption)
    {
        await _repository.SaveConsumption(consumption);
        return Ok(new { message = "Consumption data created successfully!" });
    }
}
