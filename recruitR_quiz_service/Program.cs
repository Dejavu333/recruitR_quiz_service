using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using MongoDB.Driver;
using recruitR_quiz_service.Repository;
using recruitR_quiz_service.Service;
using recruitR_quiz_service.Usecases.OpenQuizAndRetrieveQuizAccessTokens;
using recruitR_quiz_service.Usecases.RetrieveQuizToAttend;

namespace recruitR_quiz_service;

static partial class Program
{
    static void Main(string[] args)
    {
        defaultSetup();
    }
}

//TODO global error handling