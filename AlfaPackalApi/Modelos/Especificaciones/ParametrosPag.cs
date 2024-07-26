namespace Api_PACsServer.Modelos.Especificaciones
{
    public class ParametrosPag
    {
        //  -- Parametros de paginados
        public int PageNumber { get; set; } // Numero de pagina
        public int PageSize { get; set; } = 15; // Cuantos registros pagina
        public int InstitutionId { get; set; }
    }
}
