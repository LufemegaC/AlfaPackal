﻿namespace InterfazDeUsuario.Models.Especificaciones
{
    public class PaginationParameters
    {
        public int PageNumber { get; set; } // Numero de pagina
        public int PageSize { get; set; } = 15; // Cuantos registros pagina
        public int InstitutionId { get; set; }
    }
}