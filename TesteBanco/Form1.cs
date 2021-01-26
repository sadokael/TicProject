using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TesteBanco
{
    public partial class Form1 : Form
    {

        //Objeto Funcionário
        public class Funcionario
        {
            public int numero;
            public string nome;
            public int idade;

        }
        
        //array de objetos, limitada a 10
        public Funcionario[] lista = new Funcionario[10];

        public Form1()
        {
            InitializeComponent();

            //atribui o valor no load do formulário
            labelGetValue.Text = getValue();
            listInit();
        }

        //inicializa a lista com valores padrão, para me facilitar as validações na inserção
        public void listInit() 
        {
            for(int i=0; i<lista.Length; i++)
            {
                Funcionario f = new Funcionario();
                f.numero = 0;
                f.nome = "";
                f.idade = 0;
                lista[i] = f;
            }
        }
        public string getValue()
        {
            //https://www.codeproject.com/tips/858775/csharp-website-html-content-parsing-or-how-to-get <-- Onde descobri o HtmlAgilityPack

            string HTML;

            //String de retorno
            string eurValue;

            using (var wc = new WebClient())
            {
                HTML = wc.DownloadString("https://www.bportugal.pt/conversor-moeda?mlid=604");
            }
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(HTML);


            //https://www.w3schools.com/xml/xpath_intro.asp  <-- W3 Schools, procurei como selecionar a div específica, porque não tinha id, descobri xpath's
            
            return eurValue = doc.DocumentNode.SelectNodes("//*[@id=\"edit-result\"]/div[1]/div/div")[0].InnerText;
            
        }
        
        public void addFuncionario(int n)
        {   
            // Atribui os valores nas caixas ás propriedades do funcionário, numéricos parsed a int
            Funcionario f = new Funcionario();
            f.numero = int.Parse(caixaNumero.Text);
            f.nome = caixaNome.Text;
            f.idade = int.Parse(caixaIdade.Text);
            lista[n] = f;
           
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //faltam validações mais refinadas, para exemplo confirma que as caixas não estão vazias
            //pode ocorrer exceção se o número ou idade não forem numéricos
            if (caixaNumero.Text != "" && caixaNome.Text != "" && caixaIdade.Text != "")
            {   
                //atribui o numero a uma variável
                int check = int.Parse(caixaNumero.Text);

                //Boleano para decidir a messageBox a mostrar
                bool inseriu = false;

                //primeiro ciclo, para confirmar se o numero a introduzir é novo
                for (int j = 0; j < lista.Length; j++)
                {
                    if(lista[j].numero == check)
                    {
                        //Se o número já existe
                        MessageBox.Show(
                                "Um funcionário com esse número já existe!",
                                "Funcionarios",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                    }
                    else
                    {
                        //segundo ciclo que confirma se existem funcionários com numero 0
                        for (int i = 0; i < lista.Length; i++)
                        {
                            //se o numero introduzido foi diferente do padrão (0)
                            if (lista[i].numero != 0)
                            {
                                //adiciona o funcionário
                                addFuncionario(check);
                                //dá trigger ao booleano para mostrar a mensagem de sucesso
                                inseriu = true;
                                //e sai do ciclo for
                                break;
                            }
                            else
                            {
                                //senão dá trigger á mensagem de erro
                                inseriu = false;

                            }
                        }
                        if (inseriu == false)
                        {
                            //Esta mensagem assume que já não existem funcionários com numero "0"
                            MessageBox.Show(
                                    "Já têm 10 funcionários!",
                                    "Funcionarios",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                        }
                        else
                        {
                            //mensagem de sucesso
                            caixaNumero.Text = "";
                            caixaNome.Text = "";
                            caixaIdade.Text = "";
                            MessageBox.Show(
                                        "Funcionario inserido com sucesso",
                                        "Funcionarios",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        }
                    }
                }
                    
            }
            else
            {
                //caso algum campo esteja por preencher
                MessageBox.Show(
                            "Os campos não foram totalmente inseridos",
                            "Funcionarios",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
            }
        }

        private void srcFuncionario()
        {
            //atribui o valor da caixa para uma variavel Int...
            int n = int.Parse(caixaPesId.Text);

            for (int i = 0; i < lista.Length; i++)
            {
                //...e precorre a array até encontrar o ID correcto
                if (lista[i].numero == n)
                {
                    //concatenação para poupar a quantidade de labels
                    labelPesNumero.Text = "Número: " +lista[i].numero.ToString();
                    labelPesNome.Text = "Nome: " + lista[i].nome.ToString();
                    labelPesIdade.Text = "Idade: " + lista[i].idade.ToString();               
                }
            }
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            srcFuncionario();
        }

   
    }
}

  