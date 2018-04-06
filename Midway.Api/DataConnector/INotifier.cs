namespace Midway.Api.DataConnector
{
    public interface INotifier
    {
         void OnChange(string name);
    }
}