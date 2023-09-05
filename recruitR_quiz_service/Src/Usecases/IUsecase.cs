using System.Reflection.Metadata;

namespace recruitR_quiz_service;

public interface IUsecase <TReturn>
{
    //---------------------------------------------
    // methods to implement
    //---------------------------------------------
    public TReturn handle();
}
