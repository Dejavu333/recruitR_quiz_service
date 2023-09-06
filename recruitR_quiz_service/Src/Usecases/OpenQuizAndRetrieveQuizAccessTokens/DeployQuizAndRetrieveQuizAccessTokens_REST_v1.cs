using recruitR_quiz_service.Service;

namespace recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;

public class DeployQuizAndRetrieveQuizAccessTokens_REST_v1
{
    //---------------------------------------------
    // fields, properties
    //---------------------------------------------
    // private readonly IReadQuizInstanceService _readQuizInstanceService;
    private readonly IUpsertQuizInstanceService _upsertQuizInstanceService;
    private readonly IGenerateQuizAccessTokensService _generateQuizAccessTokensService;
    private readonly ILoggerService _logger;

    //---------------------------------------------
    // constructors
    //---------------------------------------------
    public DeployQuizAndRetrieveQuizAccessTokens_REST_v1()
    {
    }

    //---------------------------------------------
    // methods
    //---------------------------------------------
    

}

public interface IGenerateQuizAccessTokensService
{
}

public interface IUpsertQuizInstanceService
{
}