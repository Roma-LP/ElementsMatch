namespace _ElementsMatch3.Scripts.GameSaves
{
    public abstract class ProgressData<T> : IProgressData where T : class, new()
    {
        public abstract T GetProgressModel();
        public abstract void SetProgressModel(T state);

        object IProgressData.GetProgressModel() => GetProgressModel();
        void IProgressData.SetProgressModel(object model) => SetProgressModel(model as T);
    
        public System.Type DataType => typeof(T);
    }
}