namespace Project.Domain
{
    public interface IUpdateable<in T> : IModel where T : class, IUpdateable<T>
    {
        int Id { get; set; }
        void Update(T source);
        bool IsDirty { get; set; }
    }
}
