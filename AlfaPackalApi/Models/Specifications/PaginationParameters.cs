﻿namespace Api_PACsServer.Modelos.Especificaciones
{
    public class PaginationParameters
    {
        public int PageNumber { get; set; } // page number
        public int InstitutionId { get; set; } // ID institution
        public int PageSize { get; set; } // Number if records per page
    }
}