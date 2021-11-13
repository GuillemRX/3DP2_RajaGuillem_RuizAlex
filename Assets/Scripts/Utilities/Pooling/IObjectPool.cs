namespace Utilities.Pooling
{
    public interface IObjectPool<T>
    {
        T Get();
        void Return(T t);
    }
}