using System;
using System.IO;
using System.Linq;
using System.Text;

class GeradorSenhas
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== GERADOR DE SENHAS SEGURAS ===");
            Console.WriteLine("1. Gerar nova senha");
            Console.WriteLine("2. Recuperar senhas salvas");
            Console.WriteLine("3. Sair");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    GerarNovaSenha();
                    break;
                case "2":
                    RecuperarSenhas();
                    break;
                case "3":
                    Console.WriteLine("Saindo do programa...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                    break;
            }
        }
    }

    static void GerarNovaSenha()
    {
        try
        {
            Console.Write("Digite o comprimento da senha que deseja (em números): ");
            int comprimento = int.Parse(Console.ReadLine());

            if (comprimento <= 0)
            {
                Console.WriteLine("O comprimento deve ser maior que zero.");
                return;
            }

            Console.Write("Incluir letras? (S/N): ");
            bool usarLetras = Console.ReadLine().ToUpper() == "S";

            Console.Write("Incluir números? (S/N): ");
            bool usarNumeros = Console.ReadLine().ToUpper() == "S";

            Console.Write("Incluir caracteres especiais (@!#-)? (S/N): ");
            bool usarEspeciais = Console.ReadLine().ToUpper() == "S";

            string senha = GerarSenha(comprimento, usarLetras, usarNumeros, usarEspeciais);

            if (!string.IsNullOrEmpty(senha))
            {
                Console.WriteLine($"\nSenha gerada: {senha}");
                Console.Write("Deseja salvar esta senha? (S/N): ");
                bool salvar = Console.ReadLine().ToUpper() == "S";

                if (salvar)
                {
                    SalvarSenha(senha);
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Por favor, digite um número válido para o comprimento.");
        }
    }

    static string GerarSenha(int comprimento, bool usarLetras, bool usarNumeros, bool usarEspeciais)
    {
        const string caracteresEspeciais = "@!#-";
        var caracteres = new StringBuilder();

        if (usarNumeros)
            caracteres.Append("0123456789");
        if (usarLetras)
            caracteres.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (usarEspeciais)
            caracteres.Append(caracteresEspeciais);

        if (caracteres.Length == 0)
        {
            Console.WriteLine("Erro: Você deve selecionar pelo menos um tipo de caractere.");
            return null;
        }

        var random = new Random();
        return new string(Enumerable.Repeat(caracteres.ToString(), comprimento)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    static void SalvarSenha(string senha, string arquivo = "bkp.TXT")
    {
        try
        {
            File.AppendAllText(arquivo, senha + Environment.NewLine);
            Console.WriteLine($"Senha salva com sucesso no arquivo {arquivo}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao salvar senha: {ex.Message}");
        }
    }

    static void RecuperarSenhas(string arquivo = "bkp.TXT")
    {
        try
        {
            if (!File.Exists(arquivo))
            {
                Console.WriteLine("Arquivo de backup não encontrado.");
                return;
            }

            string[] senhas = File.ReadAllLines(arquivo);

            if (senhas.Length == 0)
            {
                Console.WriteLine("Nenhuma senha encontrada no arquivo de backup.");
            }
            else
            {
                Console.WriteLine("\nSenhas recuperadas:");
                for (int i = 0; i < senhas.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {senhas[i]}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao recuperar senhas: {ex.Message}");
        }
    }
}