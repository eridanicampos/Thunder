using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.DTO.Tarefa;
using ProjectTest.Application.Interfaces;
using ProjectTest.Domain.Helpers;

namespace ProjectTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TarefaController : BaseController
    {
        private readonly ITarefaService _tarefaService;
        private readonly ILogger<TarefaController> _logger;

        public TarefaController(ITarefaService tarefaService, ILogger<TarefaController> logger)
        {
            _tarefaService = tarefaService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TarefaDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTarefasAsync()
        {
            try
            {
                _logger.LogInformation("Buscando todas as tarefas.");
                var tarefas = await _tarefaService.GetAsync();
                return Ok(tarefas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar todas as tarefas.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar as tarefas.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TarefaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTarefaByIdAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Buscando tarefa com ID: {id}");
                var tarefa = await _tarefaService.GetById(id);
                if (tarefa == null)
                {
                    _logger.LogWarning($"Tarefa com ID: {id} não encontrada.");
                    return NotFound();
                }

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao buscar tarefa com ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar a tarefa.");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(TarefaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] TarefaParamDTO tarefaParamDTO)
        {
            if (tarefaParamDTO == null)
            {
                _logger.LogWarning("Dados da tarefa inválidos ao tentar criar uma nova tarefa.");
                return BadRequest("Dados da tarefa inválidos.");
            }

            try
            {
                _logger.LogInformation("Criando uma nova tarefa.");
                var tarefa = await _tarefaService.CreateAsync(tarefaParamDTO);
                return CreatedAtAction(nameof(GetTarefaByIdAsync), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar a tarefa.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao criar a tarefa.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTarefaAsync(string id, [FromBody] TarefaModifyDTO tarefaModifyDTO)
        {
            if (id != tarefaModifyDTO.Id.ToString())
            {
                _logger.LogWarning("ID do corpo da requisição não corresponde ao ID da URL.");
                return BadRequest("ID do corpo não corresponde ao ID da URL.");
            }

            try
            {
                _logger.LogInformation($"Atualizando a tarefa com ID: {id}");
                var updated = await _tarefaService.UpdateAsync(tarefaModifyDTO);
                if (!updated)
                {
                    _logger.LogWarning($"Tarefa com ID: {id} não encontrada para atualização.");
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao atualizar a tarefa com ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao atualizar a tarefa.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTarefaAsync(string id)
        {
            try
            {
                _logger.LogInformation($"Excluindo a tarefa com ID: {id}");
                var deleted = await _tarefaService.DeleteAsync(id);
                if (!deleted)
                {
                    _logger.LogWarning($"Tarefa com ID: {id} não encontrada para exclusão.");
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao excluir a tarefa com ID: {id}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao excluir a tarefa.");
            }
        }
    }
}
