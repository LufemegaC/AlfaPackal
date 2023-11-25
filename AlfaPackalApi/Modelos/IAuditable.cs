namespace AlfaPackalApi.Modelos
{
    public interface IAuditable
    { 
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
    }
}
