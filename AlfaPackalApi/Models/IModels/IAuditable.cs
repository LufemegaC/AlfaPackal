namespace Api_PACsServer.Models.IModels
{
    public interface IAuditable
    {
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
