using AstrologiaAPI.Models;
using AstrologiaAPI.Utils;

namespace AstrologiaAPI.Data
{
    public static class UsuarioDb
    {
        public static List<LoginEntity> Logins = PersistenciaUtils.CarregarLogins();
        public static List<UsuarioEntity> Usuarios = PersistenciaUtils.CarregarUsuarios();

        public static void Salvar()
        {
            PersistenciaUtils.SalvarLogins(Logins);
            PersistenciaUtils.SalvarUsuarios(Usuarios);
        }
    }
}
