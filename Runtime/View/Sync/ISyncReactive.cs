namespace MVVM
{
    public interface ISyncReactive
    {
        bool IsNull();
        bool IsPropertyEquals(string propertyName);
    }
}