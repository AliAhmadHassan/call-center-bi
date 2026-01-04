using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlDesk.Coletor
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            webBrowser1.Document.GetElementById("name").InnerText = "90637";
            webBrowser1.Document.GetElementById("password").InnerText = "12345678";

            foreach (HtmlElement HtmlElement1 in webBrowser1.Document.Body.All)
            {
                if (HtmlElement1.GetAttribute("value") == "Entrar")
                {
                    HtmlElement1.InvokeMember("click");
                    break;
                }
            }
            waitPolice();

            timer1.Enabled = true;
            timer2.Enabled = true;

        }

        private void Pausas(int Id)
        {
            //http://172.16.20.3/report/login_logout_calls_summary_report
            int Tentativas = 10;
        TentaNovamente: ;
            webBrowser1.Navigate("http://172.16.20.3/report/login_logout_calls_summary_report");
            waitPolice();

            webBrowser1.Document.GetElementById("group_id").SetAttribute("value", Id.ToString());

            foreach (HtmlElement HtmlElement1 in webBrowser1.Document.Body.All)
            {
                if (HtmlElement1.InnerText == "Gerar Relatório")
                {
                    HtmlElement1.InvokeMember("click");
                    break;
                }
            }
            waitPolice();


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNodeCollection linhas;

            doc.LoadHtml(webBrowser1.Document.Body.InnerHtml);
            linhas = doc.DocumentNode.SelectNodes("table/tbody/tr");

            TimeSpan TempoFalando = new TimeSpan();
            TimeSpan TempoFalandoDemais = new TimeSpan();
            int TotalAtendidas = 0;

            foreach (var Linha in linhas)
            {
                if (Linha.InnerHtml.Contains("<td"))
                {
                    var nodes = Linha.SelectNodes("td");
                    if (nodes.Count >= 37)
                    {
                        string nome = "";
                        int Quantidade = 0;
                        int Atendidas = 0;
                        TimeSpan Total = new TimeSpan();
                        TimeSpan TMA = new TimeSpan();


                        DateTime Login = DateTime.MinValue;
                        DateTime Logoff = DateTime.MinValue;
                        


                        for (int i = 0; i < nodes.Count; i++)
                        {
                            var node = nodes[i];

                            if (i == 0)
                            {
                                nome = RemoveTags(Linha.SelectSingleNode("th").InnerText).Trim();

                                if (nome.Contains("Patricia"))
                                    Application.DoEvents();

                                if (nome.Length > 20)
                                    nome = nome.Substring(0, 17) + "...";
                            }

                            if (",24,26,28,30,32,34,36,".Contains("," + i.ToString() + ","))
                                Quantidade += Convert.ToInt32(node.InnerText);

                            if (",23,25,27,29,31,33,35,".Contains("," + i.ToString() + ","))
                                Total += TimeSpan.Parse(RemoveTags(node.InnerText));


                            if (i == 0)
                                if (RemoveTags(node.InnerText) != "")
                                    Login = DateTime.Parse(RemoveTags(node.InnerText));

                            if (i == 1)
                                if (RemoveTags(node.InnerText) != "")
                                    Logoff = DateTime.Parse(RemoveTags(node.InnerText));

                            if (i == 17)
                            {
                                Atendidas = int.Parse(RemoveTags(node.InnerText));
                                TotalAtendidas += int.Parse(RemoveTags(node.InnerText));
                            }

                            if (i == 18)
                                TempoFalando += TimeSpan.Parse(RemoveTags(node.InnerText));

                            if (i == 19)
                                TempoFalandoDemais += TimeSpan.Parse(RemoveTags(node.InnerText));

                            if (i == 20)
                            {
                                TMA = TimeSpan.Parse(RemoveTags(node.InnerText));
                            }

                        }
                        System.Diagnostics.Debug.WriteLine(string.Format("Nome: {0}     {1}     {2}", nome, Quantidade.ToString(), Total.ToString()));

                        Dominio.Pausa pausa = new Dominio.Pausa();

                        pausa.Operador = nome.Trim();
                        pausa.QtdPausas = Quantidade;
                        pausa.TotalPausas = Total;
                        pausa.Login = Login;
                        pausa.Logoff = Logoff;
                        pausa.Atendidas = Atendidas;
                        pausa.TMA = TMA;
                        pausa.Salvar();




                    }
                }
            }

            if (TempoFalandoDemais.TotalSeconds > 0)
            {
                // Idle
                TimeSpan Idle = new TimeSpan();

                Idle = TempoFalandoDemais.Subtract(TempoFalando);
                Idle = TimeSpan.FromSeconds((double)Idle.TotalSeconds / (double)TotalAtendidas);

                Dominio.GrupoIdle grupoIdle = new Dominio.GrupoIdle();
                grupoIdle.IdGrupo = Id;
                grupoIdle.Idle = Idle;
                grupoIdle.Salvar();
            }
        }

        private void OperadoresEmAtividade(int Id)
        {
            int Tentativas = 10;
        TentaNovamente: ;
            webBrowser1.Navigate("http://172.16.20.3/report/queue_realtime_report?call_queue%5Bid%5D=" + Id + "&radio=0");
            waitPolice();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNodeCollection linhas;
            try
            {
                doc.LoadHtml(webBrowser1.Document.GetElementById("queue_realtime_report_div").Document.GetElementsByTagName("table")[3].InnerHtml);
                //linhas = doc.DocumentNode.SelectNodes("tbody/tr/td")[0].SelectNodes("table/tbody/tr")[1].SelectNodes("td")[0].SelectNodes("table/tbody/tr");
                linhas = doc.DocumentNode.SelectNodes("tbody/tr/td")[0].SelectNodes("table/tbody/tr")[0].SelectNodes("td")[0].SelectNodes("table/tbody/tr");

                
            }
            catch (Exception erro)
            {
                if (Tentativas > 0)
                {
                    Tentativas--;
                    System.Diagnostics.Debug.WriteLine("Tentativa: " + Tentativas);
                    System.Threading.Thread.Sleep(1000);
                    goto TentaNovamente;
                }
                else
                    throw;
            }


            var x = Regex.Matches(webBrowser1.Document.Body.InnerHtml, @"<TR[^>]*>(.*?)</TR>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            
            

            foreach (System.Text.RegularExpressions.Match Linha in x)
            {
                if (Linha.Value.Contains("td"))
                {
                    System.Text.RegularExpressions.MatchCollection colunas = Regex.Matches(Linha.Value, @"<TD[^>]*>(.*?)</TD>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                    if (colunas.Count >= 9)
                    {
                        Dominio.Atividade atividade = new Dominio.Atividade();
                        for (int i = 0; i < colunas.Count; i++)
                        {
                            doc.LoadHtml(colunas[i].Value);

                            string node = doc.DocumentNode.InnerText;

                            switch (i)
                            {
                                case 0: System.Diagnostics.Debug.WriteLine(RemoveTags(node));
                                    atividade.Operador = RemoveTags(node).Substring(5).Trim();
                                    break;

                                case 4:
                                    if ((Linha.Value.Contains("colorDisabledUser")) ||
                                        (Linha.Value.Contains("colorDisabledUser")))
                                    {
                                        System.Diagnostics.Debug.WriteLine("Linha Presa");
                                        atividade.Status = "Linha Presa";
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine(RemoveTags(node));
                                        atividade.Status = RemoveTags(node);
                                    }
                                    break;

                                case 5:
                                    {
                                        string dado = RemoveTags(node); //Atendendo / Discando / Discado
                                        System.Diagnostics.Debug.WriteLine(dado);

                                        TimeSpan TempoStatus;

                                        if (TimeSpan.TryParse(dado, out TempoStatus))
                                        {
                                            atividade.TempoStatus = TempoStatus;
                                            if (atividade.Status.Trim() == "Logado")
                                                atividade.Status = "Ocioso";
                                        }


                                    } //Ocioso
                                    break;

                                case 6:
                                    {
                                        string dado = RemoveTags(node); //Atendendo / Discando / Discado
                                        System.Diagnostics.Debug.WriteLine(dado);

                                        TimeSpan TempoStatus;

                                        if (TimeSpan.TryParse(dado, out TempoStatus))
                                        {
                                            atividade.TempoStatus = TempoStatus;

                                            if (atividade.Status.Trim() == "Logado")
                                                atividade.Status = "Falando";
                                        }
                                    }
                                    break;

                                case 2: System.Diagnostics.Debug.WriteLine(RemoveTags(node));
                                    atividade.Campanha = RemoveTags(node);
                                    break;
                            }

                        }

                        atividade.Salvar();
                    }
                }
            }
        }

        private void MailingPart1()
        {
            int Tentativas = 30;
        TentaNovamente: ;
            webBrowser1.Navigate("http://172.16.20.3/campaign/list");
            waitPolice();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNodeCollection linhas;
            try
            {
                doc.LoadHtml(webBrowser1.Document.Body.InnerHtml);
                linhas = doc.GetElementbyId("table_content").SelectNodes("tbody/tr");
            }
            catch (Exception erro)
            {
                if (Tentativas > 0)
                {
                    Tentativas--;
                    System.Diagnostics.Debug.WriteLine("Tentativa: " + Tentativas);
                    System.Threading.Thread.Sleep(1000);
                    goto TentaNovamente;
                }
                else
                    throw;
            }

            

            foreach (var Linha in linhas)
            {
                if (Linha.InnerHtml.Contains("td"))
                {
                    var nodes = Linha.SelectNodes("td");
                    if (nodes.Count >= 10)
                    {
                        Dominio.Mailing mailing = new Dominio.Mailing();

                        for (int i = 0; i < nodes.Count; i++)
                        {
                            var node = nodes[i];

                            switch (i)
                            {
                                case 0: System.Diagnostics.Debug.WriteLine(RemoveTags(node.InnerText));
                                    mailing.Campanha = RemoveTags(node.InnerText);
                                    break;

                                case 3: System.Diagnostics.Debug.WriteLine(RemoveTags(node.InnerText));
                                    int CpfRestantes;
                                    if (int.TryParse(RemoveTags(node.InnerText), out CpfRestantes))
                                        mailing.CpfRestantes = CpfRestantes;
                                    break;

                                case 6: System.Diagnostics.Debug.WriteLine(RemoveTags(node.InnerText));
                                    int Atendidas;
                                    if (int.TryParse(RemoveTags(node.InnerText), out Atendidas))
                                        mailing.Atendidas = Atendidas;
                                    break;

                                case 10:
                                    string dado = RemoveTags(node.InnerText);
                                    System.Diagnostics.Debug.WriteLine(dado.Substring(0, dado.IndexOf('%')));
                                    int PercMailing;
                                    if (int.TryParse(dado.Substring(0, dado.IndexOf('%')), out PercMailing))
                                        mailing.PercMailing = PercMailing;

                                    break;
                            }

                        }
                        mailing.Salvar();
                    }
                }
            }
        }

        private void MailingPart2()
        {
            int Tentativas = 10;
        TentaNovamente: ;
            webBrowser1.Navigate("http://172.16.20.3/report/quality_report?date%5Bfinish_day%5D=" + DateTime.Today.Day.ToString("00") + "&date%5Bfinish_month%5D=" + DateTime.Today.Month.ToString("00") + "&date%5Bfinish_year%5D=" + DateTime.Today.Year.ToString("0000") + "&date%5Bstart_day%5D=" + DateTime.Today.Day.ToString("00") + "&date%5Bstart_month%5D=" + DateTime.Today.Month.ToString("00") + "&date%5Bstart_year%5D=" + DateTime.Today.Year.ToString("0000") + "");
            waitPolice();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNodeCollection linhas;
            try
            {
                doc.LoadHtml(webBrowser1.Document.Body.InnerHtml);
                linhas = doc.GetElementbyId("quality_report_all_details").SelectNodes("table/tbody/tr");
            }
            catch (Exception erro)
            {
                if (Tentativas > 0)
                {
                    Tentativas--;
                    System.Diagnostics.Debug.WriteLine("Tentativa: " + Tentativas);
                    System.Threading.Thread.Sleep(1000);
                    goto TentaNovamente;
                }
                else
                    throw;
            }

            List<Dominio.Mailing> mailings = new Dominio.Mailing().Mailings(Convert.ToDateTime("2000-01-01"));

            foreach (var Linha in linhas)
            {
                if (Linha.InnerHtml.Contains("td"))
                {
                    var nodes = Linha.SelectNodes("td");
                    if (nodes.Count >= 10)
                    {
                        Dominio.Mailing mailing = null;

                        for (int i = 0; i < nodes.Count; i++)
                        {
                            var node = nodes[i];

                            switch (i)
                            {
                                case 0: System.Diagnostics.Debug.WriteLine(RemoveTags(node.InnerText));
                                    mailing = mailings.Where(c => c.Campanha.Equals(RemoveTags(node.InnerText))).FirstOrDefault();

                                    break;

                                case 4:
                                    {
                                        string Dado = node.InnerText.Replace("\t", "");
                                        Dado = Dado.Substring(0, Dado.Length - 1);
                                        Dado = Dado.Substring(Dado.LastIndexOf('\n') + 1, Dado.Length - Dado.LastIndexOf('\n') - 1);
                                        System.Diagnostics.Debug.WriteLine(Dado);

                                        int Desligadas;
                                        if (int.TryParse(Dado, out Desligadas))
                                            mailing.Desligadas = Desligadas;
                                    }
                                    break;

                                case 6: System.Diagnostics.Debug.WriteLine(RemoveTags(node.InnerText));

                                    int TNA;
                                    if (int.TryParse(RemoveTags(node.InnerText), out TNA))
                                            mailing.TNA = TNA;

                                    break;

                                case 8:
                                    {
                                        string Dado = node.InnerText.Replace("\t", "");
                                        string[] aux = Dado.Split('\n');

                                        if (aux.Length == 4)
                                        {
                                            System.Diagnostics.Debug.WriteLine(aux[1]);

                                            TimeSpan TMA;
                                            if (TimeSpan.TryParse(aux[1], out TMA))
                                                mailing.TMA = TMA;
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine(aux[3]);

                                            TimeSpan TMA;
                                            if (TimeSpan.TryParse(aux[3], out TMA))
                                                mailing.TMA = TMA;   
                                        }

                                    }
                                    break;

                                case 9:
                                    {
                                        string Dado = node.InnerText.Replace("\t", "");
                                        string[] aux = Dado.Split('\n');
                                        if (aux.Length == 4)
                                        {
                                            System.Diagnostics.Debug.WriteLine(aux[1]);

                                            TimeSpan TME;
                                            if (TimeSpan.TryParse(aux[1], out TME))
                                                mailing.TME = TME;
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine(aux[3]);

                                            TimeSpan TME;
                                            if (TimeSpan.TryParse(aux[3], out TME))
                                                mailing.TME = TME;
                                        }
                                    }
                                    break;
                            }
                            if (mailing == null)
                                break;
                        }
                        if (mailing != null)
                            mailing.Salvar();

                    }
                }
            }
        }

        private string RemoveTags(string p)
        {
            return p.Replace("\n", "").Replace("\t", "").Replace("&lt;", "<");
        }

        private int iframe_counter = 1; // needs to be 1, to pass DCF test
        public bool isLazyMan = true;

        /// <summary>
        /// LOCK to stop inspecting DOM before DCF
        /// </summary>
        public void waitPolice()
        {
            isLazyMan = true;
            while (isLazyMan) Application.DoEvents();
        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!e.TargetFrameName.Equals(""))
                iframe_counter--;
            isLazyMan = true;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!((WebBrowser)sender).Document.Url.Equals(e.Url))
                iframe_counter++;
            if (((WebBrowser)sender).Document.Window.Frames.Count <= iframe_counter)
            {//DCF test
                DocumentCompletedFully((WebBrowser)sender, e);
                isLazyMan = false;
            }
        }

        private void DocumentCompletedFully(WebBrowser sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //code here
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            MailingPart1();
            MailingPart2();
            new Dominio.Mailing().Atualizar();

            foreach (Dominio.Grupo grupo in new Dominio.Grupo().Grupos())
                OperadoresEmAtividade(grupo.IdTotalIpMailing);
            new Dominio.Atividade().Atualiza();

            timer1.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            new Dominio.Pausa().Remover();
            new Dominio.GrupoIdle().Remover();
            foreach (Dominio.Grupo grupo in new Dominio.Grupo().Grupos())
                Pausas(grupo.IdTotalIpPausa);

            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            new Dominio.Pausa().Remover();
            new Dominio.GrupoIdle().Remover();
            foreach (Dominio.Grupo grupo in new Dominio.Grupo().Grupos())
                Pausas(grupo.IdTotalIpPausa);

            timer1.Enabled = true;
        }
    }
}

