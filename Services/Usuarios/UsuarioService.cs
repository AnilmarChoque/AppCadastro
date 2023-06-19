using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCadastro.Models;

namespace AppCadastro.Services.Usuarios
{
    public class UsuarioService : Request
    {
        private readonly Request _request;
        private const string apiUrlBase = "http://programinds.somee.com/Cadastro/Usuarios";

        private string _token;

        public UsuarioService(string token)
        {
            _request = new Request();
            _token = token;
        }

        public async Task<ObservableCollection<Usuario>> GetUsuariosAsync()
        {
            string urlComplementar = string.Format("{0}", "/GetAll");
            ObservableCollection<Models.Usuario> listaUsuarios = await
            _request.GetAsync<ObservableCollection<Models.Usuario>>(apiUrlBase + urlComplementar, _token);
            return listaUsuarios;
        }
        public async Task<Usuario> GetUsuarioAsync(int usuarioId)
        {
            string urlComplementar = string.Format("/{0}", usuarioId);
            var usuario = await _request.GetAsync<Models.Usuario>(apiUrlBase + urlComplementar, _token);
            return usuario;
        }
        public async Task<int> PostUsuarioAsync(Usuario u)
        {
            return await _request.PostReturnIntTokenAsync(apiUrlBase, u, _token);
        }
        public async Task<int> PutUsuarioAsync(Usuario u)
        {
            var result = await _request.PutAsync(apiUrlBase, u, _token);
            return result;
        }
        public async Task<int> DeleteUsuarioAsync(int usuarioId)
        {
            string urlComplementar = string.Format("/{0}", usuarioId);
            var result = await _request.DeleteAsync(apiUrlBase + urlComplementar, _token);
            return result;
        }
    }
}
