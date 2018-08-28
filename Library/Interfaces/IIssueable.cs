namespace Library
{
    public interface IIssueable
    {
        bool Give();

        bool IsAvailable();

        bool Take();
    }
}