namespace _ElementsMatch3.Scripts.GameSaves
{
    public interface IProgressData
    {
        System.Type DataType { get; }
        object GetProgressModel();
        void SetProgressModel(object model);
    }
}