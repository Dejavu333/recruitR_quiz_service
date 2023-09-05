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
    private readonly IModel _channel;
    private readonly string _exchange;
    private readonly string _queue;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public RabbitMQLogger(IConfiguration config)
    {
        var rabbitMQConfig = config.GetSection("RabbitMQ");
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMQConfig["Hostname"],
            Port = int.Parse(rabbitMQConfig["port"]),
            UserName = rabbitMQConfig["UserName"],
            Password = rabbitMQConfig["Password"]
        };
        
        try {
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _exchange = "";
            _queue = "log_queue";
            _channel.QueueDeclare(queue: _queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
        catch(Exception e)
        {
            Console.WriteLine("[Error] Can't init RabbitMQLogger\n"+e);
        }
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    private void Log(LogLevel level, string message)
    {
        var logMessage = $"[{level}] {message}";
        var body = Encoding.UTF8.GetBytes(logMessage);
        _channel.BasicPublish(exchange: _exchange, routingKey: _queue, basicProperties: null, body: body);
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
