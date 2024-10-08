using Moq;
using Xunit;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ProjectTest.Application.Interfaces;
using ProjectTest.Application.Services;
using ProjectTest.Application.DTO.Tarefa;
using ProjectTest.Domain.Entities;
using ProjectTest.Domain.Interfaces;
using ProjectTest.Domain.Interfaces.Repository;
using ProjectTest.Domain.Interfaces.Common;

public class TarefaServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ICurrentUserInfo> _mockCurrentUserInfo;
    private readonly Mock<ILogger<TarefaService>> _mockLogger;
    private readonly ITarefaService _tarefaService;

    public TarefaServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockCurrentUserInfo = new Mock<ICurrentUserInfo>();
        _mockLogger = new Mock<ILogger<TarefaService>>();

        _tarefaService = new TarefaService(_mockUnitOfWork.Object, _mockMapper.Object, _mockCurrentUserInfo.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidTarefa_ReturnsTarefaDTO()
    {
        // Arrange
        var tarefaParamDto = new TarefaParamDTO { Titulo = "Nova Tarefa", Descricao = "Descrição da tarefa" };
        var tarefa = new Tarefa { Id = Guid.NewGuid(), Titulo = "Nova Tarefa", Descricao = "Descrição da tarefa" };
        var tarefaDto = new TarefaDTO { Id = tarefa.Id, Titulo = "Nova Tarefa", Descricao = "Descrição da tarefa" };

        _mockMapper.Setup(m => m.Map<Tarefa>(It.IsAny<TarefaParamDTO>())).Returns(tarefa);
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.AddAndSaveAsync(It.IsAny<Tarefa>())).ReturnsAsync(tarefa);
        _mockMapper.Setup(m => m.Map<TarefaDTO>(It.IsAny<Tarefa>())).Returns(tarefaDto);
        _mockCurrentUserInfo.Setup(c => c.UserId).Returns(Guid.NewGuid().ToString());

        // Act
        var result = await _tarefaService.CreateAsync(tarefaParamDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tarefaDto.Id, result.Id);
        Assert.Equal(tarefaDto.Titulo, result.Titulo);
        Assert.Equal(tarefaDto.Descricao, result.Descricao);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Tarefa criada com sucesso")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public async Task GetById_InvalidId_ThrowsException()
    {
        // Arrange
        var invalidId = "invalid-guid";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.GetById(invalidId));

        Assert.Equal("ID é inválido", exception.Message);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("ID inválido")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var tarefa = new Tarefa { Id = tarefaId };

        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.GetByGuidAsync(It.IsAny<Guid>())).ReturnsAsync(tarefa);
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.SoftDeleteAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

        // Act
        var result = await _tarefaService.DeleteAsync(tarefaId.ToString());

        // Assert
        Assert.True(result);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Tarefa com ID: {tarefaId} excluída com sucesso")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ValidId_UpdatesSuccessfully()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var tarefaModifyDto = new TarefaModifyDTO { Id = tarefaId, Titulo = "Tarefa Atualizada", Descricao = "Descrição atualizada" };
        var tarefa = new Tarefa { Id = tarefaId, Titulo = "Tarefa Antiga", Descricao = "Descrição antiga" };

        // Mock para o UserId
        _mockCurrentUserInfo.Setup(x => x.UserId).Returns(Guid.NewGuid().ToString());

        // Mock para o repositório
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.GetByGuidAsync(It.IsAny<Guid>())).ReturnsAsync(tarefa);
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.UpdateAsync(It.IsAny<Tarefa>())).Returns(Task.CompletedTask);

        // Act
        var result = await _tarefaService.UpdateAsync(tarefaModifyDto);

        // Assert
        Assert.True(result);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Information),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Tarefa com ID: {tarefaModifyDto.Id} atualizada com sucesso")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }


    [Fact]
    public async Task UpdateAsync_InvalidId_ThrowsException()
    {
        // Arrange
        var tarefaModifyDto = new TarefaModifyDTO { Id = Guid.Empty, Titulo = "Tarefa Atualizada", Descricao = "Descrição atualizada" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.UpdateAsync(tarefaModifyDto));

        Assert.Equal("ID é inválido", exception.Message);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("ID da tarefa é inválido")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_TarefaNotFound_ThrowsException()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        var tarefaModifyDto = new TarefaModifyDTO { Id = tarefaId, Titulo = "Tarefa Atualizada", Descricao = "Descrição atualizada" };

        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.GetByGuidAsync(It.IsAny<Guid>())).ReturnsAsync((Tarefa)null); // Tarefa não encontrada

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.UpdateAsync(tarefaModifyDto));

        Assert.Equal("Tarefa não encontrada!", exception.Message);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Tarefa não encontrada para o ID: {tarefaId}")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }


    [Fact]
    public async Task GetAsync_RepositoryThrowsException_LogsErrorAndThrows()
    {
        // Arrange
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.GetAllAsync()).ThrowsAsync(new Exception("Erro de conexão"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.GetAsync());

        Assert.Equal("Erro de conexão", exception.Message);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Erro ao buscar todas as tarefas")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_RepositoryThrowsException_LogsErrorAndThrows()
    {
        // Arrange
        var tarefaId = Guid.NewGuid();
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.GetByGuidAsync(It.IsAny<Guid>())).ReturnsAsync(new Tarefa { Id = tarefaId });
        _mockUnitOfWork.Setup(uow => uow.TarefaRepository.SoftDeleteAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Erro ao excluir"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _tarefaService.DeleteAsync(tarefaId.ToString()));

        Assert.Equal("Erro ao excluir", exception.Message);

        _mockLogger.Verify(logger => logger.Log(
            It.Is<LogLevel>(logLevel => logLevel == LogLevel.Error),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Erro ao excluir a tarefa com ID: {tarefaId}")),
            It.IsAny<Exception>(),
            It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
    }

}
