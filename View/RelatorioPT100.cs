using Digisystem.Calibracao.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digisystem.Calibracao.View
{
    public partial class RelatorioPT100 : Form
    {
        public RelatorioPT100()
        {
            InitializeComponent();
        }


        //NomeCliente
        //NCertificado
        //Endereco
        //Fabricante
        //TipoEquipamento
        //NSerieEquipamento
        //NCertCalibrador
        //ValidadeCalibrador
        //NCertTermorresistencia
        //ValidadeTermorresistencia
        //T1Padrao
        //T1Lido
        //T1Incerteza
        //T2Padrao
        //T2Lido
        //T2Incerteza
        //DataCalibracao
        //DataEmissao

        private void RelatorioPT100_Load(object sender, EventArgs e)
        {
            var rpd = new Reporting.Reports.PT100();

            rpd.SetParameterValue("NomeCliente", "ZEEH MANOLO");
            rpd.SetParameterValue("NCertificado", "Nº 99999");
            rpd.SetParameterValue("Endereco", "Rua das Laranjovisks, 45 - Curitiba/PR");
            rpd.SetParameterValue("Fabricante", "Eu memo");
            rpd.SetParameterValue("TipoEquipamento", "Algum ae");
            rpd.SetParameterValue("NSerieEquipamento", "45998");
            rpd.SetParameterValue("NCertCalibrador", "89997");
            rpd.SetParameterValue("ValidadeCalibrador", "Agosto de 2018");
            rpd.SetParameterValue("NCertTermorresistencia", "56465456");
            rpd.SetParameterValue("ValidadeTermorresistencia", "Janeiro de 2019");
            rpd.SetParameterValue("T1Padrao", "45,8");
            rpd.SetParameterValue("T1Lido", "50,0");
            rpd.SetParameterValue("T1Incerteza", "0,08");
            rpd.SetParameterValue("T2Padrao", "45,8");
            rpd.SetParameterValue("T2Lido", "50,0");
            rpd.SetParameterValue("T2Incerteza", "0,08");

            rpd.SetParameterValue("DataCalibracao", DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"));
            rpd.SetParameterValue("DataEmissao", DateTime.Now.ToString("dd/MM/yyyy"));

            crystalReportViewer1.ReportSource = rpd;
            //crystalReportViewer1.RefreshReport();
        }
    }
}
