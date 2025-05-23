﻿using System.Text.Json;
using AstrologiaAPI.Models;
using System.Text;

namespace AstrologiaAPI.Utils
{
    public static class PersistenciaUtils
    {
        private const string BasePath = "FakeDb/";
        private const string LoginPath = $"{BasePath}logins.json";
        private const string UsuarioPath = $"{BasePath}usuarios.json";

        public static void SalvarLogins(List<LoginEntity> logins)
        {
            try
            {
                var json = JsonSerializer.Serialize(logins, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LoginPath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar logins: {ex.Message}");
            }
        }

        public static void SalvarUsuarios(List<UsuarioEntity> usuarios)
        {
            try
            {
                var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(UsuarioPath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar usuários: {ex.Message}");
            }
        }

        public static List<LoginEntity> CarregarLogins()
        {
            try
            {
                if (!File.Exists(LoginPath)) return new List<LoginEntity>();
                var json = File.ReadAllText(LoginPath);
                return JsonSerializer.Deserialize<List<LoginEntity>>(json) ?? new List<LoginEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar logins: {ex.Message}");
                return new List<LoginEntity>();
            }
        }

        public static List<UsuarioEntity> CarregarUsuarios()
        {
            try
            {
                if (!File.Exists(UsuarioPath)) return new List<UsuarioEntity>();
                var json = File.ReadAllText(UsuarioPath);
                return JsonSerializer.Deserialize<List<UsuarioEntity>>(json) ?? new List<UsuarioEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar usuários: {ex.Message}");
                return new List<UsuarioEntity>();
            }
        }
    }
}
