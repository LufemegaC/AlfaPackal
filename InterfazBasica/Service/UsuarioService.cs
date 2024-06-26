﻿using InterfazBasica.Models;
using InterfazBasica.Service;
using InterfazBasica_DCStore.Models.Indentity;
using InterfazBasica_DCStore.Service.IService;
using Utileria;

namespace InterfazBasica_DCStore.Service
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        public readonly IHttpClientFactory _httpClient;
        private string _villaUrl;

        public UsuarioService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient)
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServiceUrls:API_URL");
        }
        public Task<T> Login<T>(LoginRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/usuario/login"

            });
        }

        public Task<T> Registar<T>(RegistroRequestDto dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.POST,
                Datos = dto,
                Url = _villaUrl + "/api/usuario/registrar"

            });
        }
    }
}
