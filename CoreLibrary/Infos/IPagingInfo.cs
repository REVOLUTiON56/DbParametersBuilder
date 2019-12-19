namespace CoreLibrary.Infos
{
    public interface IPagingInfo
    {
        int page { get; set; }
        int start { get; set; }
        int limit { get; set; }
    }
}