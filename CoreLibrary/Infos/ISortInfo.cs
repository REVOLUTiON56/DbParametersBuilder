namespace CoreLibrary.Infos
{
    public interface ISortInfo
    {
        string property { get; set; }
        Directions direction { get; set; }
    }
}