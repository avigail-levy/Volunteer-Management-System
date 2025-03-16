using BlApi;
using BO;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    public void AdvanceClock(TimeUnit timeUnit)
    {
        throw new NotImplementedException();
    }

    public DateTime GetClock()
    {
        throw new NotImplementedException();
    }

    public TimeSpan GetRiskRange()
    {
        throw new NotImplementedException();
    }

    public void InitializeDB()
    {
        throw new NotImplementedException();
    }

    public void ResetDB()
    {
        throw new NotImplementedException();
    }

    public void SetRiskRange(TimeSpan newRiskRange)
    {
        throw new NotImplementedException();
    }
}
