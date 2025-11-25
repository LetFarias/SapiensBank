using static System.Console;

public class UX
{
    private readonly Banco _banco;
    private readonly string _titulo;

    public UX(string titulo, Banco banco)
    {
        _titulo = titulo;
        _banco = banco;
    }

    public static void CriarLinha()
    {
        WriteLine("-------------------------------------------------");
    }

    public static void CriarTitulo(string titulo)
    {
        Clear();
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        WriteLine(" " + titulo);
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
    }

    public static void CriarRodape(string? mensagem = null)
    {
        CriarLinha();
        ForegroundColor = ConsoleColor.Green;
        if (mensagem != null)
            WriteLine(" " + mensagem);
        Write(" ENTER para continuar");
        ForegroundColor = ConsoleColor.White;
        ReadLine();
    }

    public void Executar()
    {
        CriarTitulo(_titulo);
        WriteLine(" [1] Criar Conta");
        WriteLine(" [2] Listar Contas");
        WriteLine(" [3] Efetuar Saque");
        WriteLine(" [4] Efetuar Depósito");
        WriteLine(" [5] Mudar Limite");
        
        ForegroundColor = ConsoleColor.Red;
        WriteLine("\n [9] Sair");
        ForegroundColor = ConsoleColor.White;
        CriarLinha();
        ForegroundColor = ConsoleColor.Yellow;
        Write(" Digite a opção desejada: ");
        var opcao = ReadLine() ?? "";
        ForegroundColor = ConsoleColor.White;
        switch (opcao)
        {
            case "1": CriarConta(); break;
            case "2": MenuListarContas(); CriarRodape(); break;
            case "3": Sacar(); break;
            case "4": Depositar(); break;
            case "5": MudarLimite(); break;
        }
        if (opcao != "9")
        {
            Executar();
        }
        _banco.SaveContas();
    }

    private void CriarConta()
    {
        CriarTitulo(_titulo + " - Criar Conta");
        Write(" Numero:  ");
        var numero = Convert.ToInt32(ReadLine());
        Write(" Cliente: ");
        var cliente = ReadLine() ?? "";
        Write(" CPF:     ");
        var cpf = ReadLine() ?? "";
        Write(" Senha:   ");
        var senha = ReadLine() ?? "";
        Write(" Limite:  ");
        var limite = Convert.ToDecimal(ReadLine());

        var conta = new Conta(numero, cliente, cpf, senha, limite);
        _banco.Contas.Add(conta);

        CriarRodape("Conta criada com sucesso!");
    }

    private void MenuListarContas()
    {
        CriarTitulo(_titulo + " - Listar Contas");
        foreach (var conta in _banco.Contas)
        {
            WriteLine($" Conta: {conta.Numero} - {conta.Cliente}");
            WriteLine($" Saldo: {conta.Saldo:C} | Limite: {conta.Limite:C}");
            WriteLine($" Saldo Disponível: {conta.SaldoDisponível:C}\n");
        }
    }

    private Conta? EscolherConta(string msg)
    {
        MenuListarContas();
        CriarLinha();
        WriteLine($"Escolha a Conta para {msg}:");

        int id = Convert.ToInt32(ReadLine() ?? "0");
        
        foreach (var conta in _banco.Contas)
        {
           if (id == conta.Numero)
            return conta; 
        }
        
        WriteLine($"Conta {id} não existe");
        CriarRodape();
        return null;
    }
    
    private decimal PegarValor()
    {
        decimal valor = Convert.ToDecimal(ReadLine() ?? "0");
        return Math.Max(valor,0);
    }

    private void MudarLimite()
    {
        var conta = EscolherConta("Mudar Limite");
        if (conta == null){return;}

        CriarTitulo($"Editando conta {conta.Numero}");
        WriteLine($"Limite atual: {conta.Limite}");
        CriarLinha();
        
        WriteLine("Digite o novo Limite:");
        conta.Limite = Convert.ToInt32(PegarValor());

        WriteLine($"Novo Limite: {conta.Limite:C}");
        CriarRodape();
    }
   
    
    private void Sacar()
    {
        var conta = EscolherConta("Efetuar Saque");
        if (conta == null){return;}

        CriarTitulo($"Sacando dinheiro da conta {conta.Numero}");
        WriteLine($"Saldo total disponível: {conta.SaldoDisponível:C}");
        CriarLinha();
        
        WriteLine("Digite o total a sacar:");
        decimal total = PegarValor();

        if (total > conta.Saldo+conta.Limite)
            WriteLine("Dinheiro requerido excede o Saldo");
        else
        {
            conta.Saldo -= total;
            WriteLine($"Novo Saldo: {conta.Saldo:C} (Com Limite: {conta.SaldoDisponível:C})");
        }
        CriarRodape();
    }
    
    private void Depositar()
    {
        var conta = EscolherConta("Efetuar Depósito");
        if (conta == null){return;}

        CriarTitulo($"Depositando dinheiro na conta {conta.Numero}");
        WriteLine($"Saldo atual: {conta.Saldo:C}");
        CriarLinha();
        
        WriteLine("Digite o total a ser depositado:");
        conta.Saldo += PegarValor();

        WriteLine($"Novo Saldo: {conta.Saldo:C}");

        CriarRodape();
    }
}
