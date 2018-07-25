using Digisystem.Calibracao.Data;
using Digisystem.Calibracao.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digisystem.Calibracao.View
{
    public partial class GerenciamentoClientes : Form
    {
        public GerenciamentoClientes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var file = new OpenFileDialog();
            file.Filter = "Arquivos .CSV|*.csv";
            file.CheckFileExists = true;
            file.Multiselect = false;
            file.ShowDialog();

            if (string.IsNullOrWhiteSpace(file.FileName))
            {
                MessageBox.Show("Nenhum arquivo selecionado!");
                return;
            }

            var atualizados = 0;
            var inseridos = 0;
            var totalCount = 0;
            using (var sr = new StreamReader(file.FileName))
            {
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    sr.ReadLine();
                    totalCount++;
                }
            }
            progresso.Value = 0;
            progresso.Maximum = totalCount;

            lblProgresso.Text = $"Importando os dados... (0/{totalCount})";
            pnlProgresso.Visible = true;

            new Thread(() =>
            {
                var novosClientes = new List<Cliente>();
                var encoding = Encoding.GetEncoding(1252);
                using (var sr = new StreamReader(file.FileName, encoding))
                {
                    sr.ReadLine();
                    var cont = 0;
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        var dados = line.Split(';');
                        cont++;
                        if (dados.Length < 7) continue;
                        if (dados[4] == "  .   .   /    -  " || dados[4] == "00.000.000/0000-00" || dados[4].Contains(" ")) continue;
                        if (string.IsNullOrWhiteSpace(dados[0])) continue;

                        // 0 = Razao/Nome
                        // 1 = Nome Fantasia
                        // 2 = Contato
                        // 3 = Fone
                        // 4 = CNPJ
                        // 5 = CPF
                        // 6 = Atividade
                        // 7 = Endereço
                        // 8 = Numero
                        // 9 = Bairro
                        // 10 = CEP
                        // 11 = Cidade
                        // 12 = Email

                        var itemToAdd = new Cliente
                        {
                            RazaoSocial = dados[0].Trim(),
                            Cnpj = dados[4].Trim(),
                            Endereco = $"{dados[7].Trim()}, {dados[8].Trim()} - {dados[11].Trim()}"
                        };
                        novosClientes.Add(itemToAdd);

                        Invoke((Action)delegate
                        {
                            progresso.Value = cont;
                            lblProgresso.Text = $"Importando os dados... ({cont}/{totalCount})";
                        });
                    }
                }

                var clientesExistentes = Clientes.GetAll();

                var listaNovos = new List<Cliente>();
                var listaAtualizar = new List<Cliente>();

                novosClientes = novosClientes.GroupBy(x => x.Cnpj).Select(x => x.FirstOrDefault()).ToList();

                foreach (var item in novosClientes)
                {
                    var existente = clientesExistentes.FirstOrDefault(x => x.Cnpj == item.Cnpj);
                    if (existente == null)
                    {
                        listaNovos.Add(item);
                        continue;
                    }

                    if ((existente.Endereco ?? "") != item.Endereco || (existente.RazaoSocial ?? "") != item.RazaoSocial)
                    {
                        existente.Endereco = item.Endereco;
                        existente.RazaoSocial = item.RazaoSocial;
                        listaAtualizar.Add(existente);
                    }
                }

                if (listaNovos.Count > 0)
                {
                    using (var db = Common.CertDb)
                    {
                        var col = db.GetCollection<Cliente>("clientes");
                        inseridos = col.InsertBulk(listaNovos);
                    }
                }

                if (listaAtualizar.Count > 0)
                {
                    using (var db = Common.CertDb)
                    {
                        var col = db.GetCollection<Cliente>("clientes");
                        col.EnsureIndex(x => x.Cnpj);
                        atualizados = col.Update(listaAtualizar);
                    }
                }

                Invoke((Action)delegate
                {
                    MessageBox.Show($"Importação concluída! {inseridos} inserido(s), {atualizados} atualizado(s).");
                    RefreshClientes();
                    pnlProgresso.Visible = false;
                });
            }).Start();
        }

        private void GerenciamentoClientes_Load(object sender, EventArgs e)
        {
            RefreshClientes();
        }

        private void RefreshClientes()
        {
            listView1.Items.Clear();
            var ordered = Data.Clientes.GetAll().OrderBy(x => x.RazaoSocial).ToList();
            foreach (var item in ordered)
            {
                listView1.Items.Add(new ListViewItem(new string[]{
                    item.RazaoSocial,
                    item.Cnpj,
                    item.Endereco
                }));
            }
        }
    }
}
