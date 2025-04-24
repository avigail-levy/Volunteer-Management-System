using BlApi;
using Helpers;

namespace BlImplementation;

internal class AdminImplementation : IAdmin
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Advance the system clock by the appropriate time unit.
    /// </summary>
    /// <param name="timeUnit">The unit of time for promotion</param>
    public void AdvanceClock(BO.TimeUnit timeUnit)
    {
        switch (timeUnit)
        {
            case BO.TimeUnit.Minute: ClockManager.UpdateClock(ClockManager.Now.AddMinutes(1)); break;
            case BO.TimeUnit.Hour: ClockManager.UpdateClock(ClockManager.Now.AddHours(1)); break;
            case BO.TimeUnit.Day: ClockManager.UpdateClock(ClockManager.Now.AddDays(1)); break;
            case BO.TimeUnit.Month: ClockManager.UpdateClock(ClockManager.Now.AddMonths(1)); break;
            case BO.TimeUnit.Year: ClockManager.UpdateClock(ClockManager.Now.AddYears(1)); break;
        };
    }
    /// <summary>
    /// Returns the value of the system clock.
    /// </summary>
    /// <returns> the value of the system clock. </returns>
    public DateTime GetClock()
    {
        return ClockManager.Now;
    }
    /// <summary>
    /// Return the value of the configuration variable "Risk Range"
    /// </summary>
    /// <returns>The value of the configuration variable "Risk Range"</returns>
    public TimeSpan GetRiskRange()
    {
        return _dal.Config.RiskRange;
    }
    /// <summary>
    /// Initialize the database.
    /// </summary>
    public void InitializeDB()
    {
        DalTest.Initialization.Do();
        ClockManager.UpdateClock(ClockManager.Now);
    }
    /// <summary>
    /// Reset all configuration data (reset all configuration data to its initial value)
    /// Clear all entity data(clear all data lists)
    /// </summary>
    public void ResetDB()
    {
        _dal.ResetDB();
        ClockManager.UpdateClock(ClockManager.Now);
    }
    /// <summary>
    /// Updates the value of the configuration variable "Risk Time Range" to the value received as a parameter
    /// </summary>
    /// <param name="newRiskRange">Risk time frame</param>
    public void SetRiskRange(TimeSpan newRiskRange)
    {
        _dal.Config.RiskRange = newRiskRange;
    }
}