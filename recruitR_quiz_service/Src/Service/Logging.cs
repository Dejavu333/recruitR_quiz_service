using System.Text;
using RabbitMQ.Client;

namespace recruitR_quiz_service.Service;

public interface ILoggerService
{
    void Debug(string message);
    void Info(string message);
    void Warning(string message);
    void Error(string message);
    void Critical(string message);
}

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    Critical
}

public class RabbitMQLogger : ILoggerService
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    private IModel _channel;
    private readonly string _exchange;
    private readonly string _queue;
    private readonly ConnectionFactory _factory;
    private ILogger? _fallbackLogger; //todo should be a separate class
    private int _retryDelay;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public RabbitMQLogger(IConfiguration config)
    {
        var rabbitMQConfig = config.GetSection("RabbitMQ");
        _factory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig["Hostname"] ?? "localhost",
            Port = int.Parse(rabbitMQConfig["port"] ?? "5672"),
            UserName = rabbitMQConfig["UserName"] ?? "root",
            Password = rabbitMQConfig["Password"] ?? "test"
        };
        _retryDelay = int.Parse(rabbitMQConfig["retryDelay"] ?? "1");
        _exchange = rabbitMQConfig["exchange"] ?? "";
        _queue = rabbitMQConfig["queue"] ?? "log_queue";
        tryConnect();
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    private async Task tryConnect()
    {
        try
        {
            var connection = _factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _fallbackLogger = null;
        }
        catch (Exception e)
        {
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            _fallbackLogger = loggerFactory.CreateLogger<RabbitMQLogger>();
            _fallbackLogger.LogError($"Error initializing RabbitMQLogger: " + e.Message + $"\nRetrying in {_retryDelay} mins...");
            await Task.Delay(TimeSpan.FromMinutes(_retryDelay));
            await tryConnect();
        }
    }

    private void Log(LogLevel level, string message)
    {
        if (_fallbackLogger is null)
        {
            var logMessage = $"[{level}] {message}";
            var body = Encoding.UTF8.GetBytes(logMessage);
            _channel.BasicPublish(exchange: _exchange, routingKey: _queue, basicProperties: null, body: body);
        }
        else
        {
            fallbackLog(level,message);
        }
    }

    private void fallbackLog(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Debug:
                _fallbackLogger?.LogDebug(message);
                break;
            case LogLevel.Info:
                _fallbackLogger?.LogInformation(message);
                break;
            case LogLevel.Warning:
                _fallbackLogger?.LogWarning(message);
                break;
            case LogLevel.Error:
                _fallbackLogger?.LogError(message);
                break;
            case LogLevel.Critical:
                _fallbackLogger?.LogCritical(message);
                break;
        }
    }

    public void Debug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    public void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    public void Warning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    /// <summary>
    /// Error happened but program can still continue execution.
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message)
    {
        Log(LogLevel.Error, message);
    }
    
    //todo better summaries
    /// <summary>
    /// Program is unable to continue execution.
    /// </summary>
    /// <param name="message"></param>
    public void Critical(string message)
    {
        Log(LogLevel.Critical, message);
    }
}
