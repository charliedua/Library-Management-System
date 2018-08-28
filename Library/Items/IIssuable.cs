namespace Library
{
    public interface IIssuable
    {
        bool Acquired { get; set; }

        bool Give();

        bool Take();
    }
}