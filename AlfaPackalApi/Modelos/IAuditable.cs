namespace AlfaPackalApi.Modelos
{
    public interface IAuditable
    { 
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    }
}
