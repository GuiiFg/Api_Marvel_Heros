namespace SupportClass
{
    public partial class Result
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Thumbnail? Thumbnail { get; set; }
        public Uri? ResourceUri { get; set; }
        public Comics? Comics { get; set; }
        public Comics? Series { get; set; }
        public Stories? Stories { get; set; }
        public Comics? Events { get; set; }
        public List<Url>? Urls { get; set; }
    }
}



