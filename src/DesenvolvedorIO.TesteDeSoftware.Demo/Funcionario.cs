using System;
using System.Collections.Generic;

namespace DesenvolvedorIO.TesteDeSoftware.Demo
{
    public class Pessoa
    {
        public string Nome { get; protected set; }
        public string Apelido { get; set; }
    }

    public class Funcionario : Pessoa
    {
        public double Salario { get; private set; }
        public NivelProfissional NivelProfissional { get; private set; }
        public IList<string> Habilidades { get; private set; }

        public Funcionario(string nome, double salario)
        {
            Nome = string.IsNullOrEmpty(nome) ? "Fulano" : nome;
            DefinirSalario(salario);
            DefinirHabilidades();
        }

        public void DefinirSalario(double salario)
        {
            Salario = salario;

            NivelProfissional = salario switch
            {
                < 500 => throw new Exception("Salario inferior ao permitido"),
                < 2000 => NivelProfissional.Junior,
                >= 2000 and < 8000 => NivelProfissional.Pleno,
                >= 8000 => NivelProfissional.Senior,
            };
        }

        private void DefinirHabilidades()
        {
            var habilidadesBasicas = new List<string>()
            {
                
                "Lógica de Programação",
                "OOP"
            };

            Habilidades = habilidadesBasicas;

            Action a = NivelProfissional switch
            {
                NivelProfissional.Pleno => () => Habilidades.Add("Testes"),
                NivelProfissional.Senior => () => 
                { 
                    Habilidades.Add("Testes"); 
                    Habilidades.Add("Microservices"); 
                },
                _ => () => Habilidades.Add("Nível Profissional Desconhecido"),
            };
            a();
        }
    }

    public enum NivelProfissional
    {
        Junior,
        Pleno,
        Senior
    }

    public class FuncionarioFactory
    {
        public static Funcionario Criar(string nome, double salario)
        {
            return new Funcionario(nome, salario);
        }
    }
}