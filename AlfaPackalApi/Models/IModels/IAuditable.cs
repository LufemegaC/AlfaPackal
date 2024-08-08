namespace Api_PACsServer.Modelos.IModels
{
    public interface IAuditable
    {
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
