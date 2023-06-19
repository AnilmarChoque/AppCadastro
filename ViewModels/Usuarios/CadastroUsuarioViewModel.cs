using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCadastro.Services.Usuarios;
using AppCadastro.Models;
using AppCadastro.Models.Enuns;
using System.Windows.Input;

namespace AppCadastro.ViewModels.Usuarios
{
    [QueryProperty("UsuarioSelecionadoId", "uId")]
    public class CadastroUsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;

        public ICommand SalvarCommand { get; }

        public ICommand CancelarCommand { get; set; }

        public CadastroUsuarioViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            uService = new UsuarioService(token);
            _ = ObterClasses();

            SalvarCommand = new Command(async () => { await SalvarUsuario(); });
            CancelarCommand = new Command(async => CancelarCadastro());
        }

        private async void CancelarCadastro()
        {
            await Shell.Current.GoToAsync("..");
        }

        private int id;
        public string nome;
        private long cpf;
        public string email;
        public string senha;
        private TipoClasse tipoClasseSelecionado;
        private string usuarioSelecionadoId;

        public int Id 
        { 
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged();
            }
        }

        public long Cpf
        {
            get => cpf;
            set
            {
                cpf = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }

        public string Senha
        {
            get => senha;
            set
            {
                senha = value;
                OnPropertyChanged();
            }
        }

        public TipoClasse TipoClasseSelecionado
        {
            get { return tipoClasseSelecionado; }
            set
            {
                if (value != null)
                {
                    tipoClasseSelecionado = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UsuarioSelecionadoId
        {
            set
            {
                if(value != null)
                {
                    usuarioSelecionadoId = Uri.UnescapeDataString(value);
                    CarregarUsuario();
                }
            }
        }

        private ObservableCollection<TipoClasse> listaTiposClasse;
        public ObservableCollection<TipoClasse> ListaTiposClasse
        {
            get { return listaTiposClasse; }
            set
            {
                if (value != null)
                {
                    listaTiposClasse = value;
                    OnPropertyChanged();
                }
            }
        }
        

        public async Task ObterClasses()
        {
            try
            {
                ListaTiposClasse = new ObservableCollection<TipoClasse>();
                ListaTiposClasse.Add(new TipoClasse() { Id = 1, Descricao = "Padrão" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 2, Descricao = "Idoso" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 3, Descricao = "Cadeirante" });
                ListaTiposClasse.Add(new TipoClasse() { Id = 4, Descricao = "Gestante" });
                OnPropertyChanged(nameof(ListaTiposClasse));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task SalvarUsuario()
        {
            try
            {
                Usuario model = new Usuario()
                {
                    Nome = this.nome,
                    Cpf = this.cpf,
                    Email = this.email,
                    Senha = this.senha,
                    Id = this.id,
                    Preferencia = (ClasseEnum)tipoClasseSelecionado.Id
                };
                if (model.Id == 0)
                    await uService.PostUsuarioAsync(model);
                else
                    await uService.PutUsuarioAsync(model);

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dados salvos com sucesso!", "Ok");
                await Shell.Current.GoToAsync(".."); //Remove a página atual da pilha de páginas
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async void CarregarUsuario()
        {
            try
            {
                Usuario u = await uService.GetUsuarioAsync(int.Parse(usuarioSelecionadoId));

                this.Nome = u.Nome;
                this.Cpf = u.Cpf;
                this.Email = u.Email;
                this.Senha = u.Senha;
                this.Id = u.Id;

                TipoClasseSelecionado = this.ListaTiposClasse
                    .FirstOrDefault(tClasse => tClasse.Id == (int)u.Preferencia);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }

    }
}
