using System;
using System.Windows;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SupportClass;

namespace Api_Marvel_Heros
{
    class Program
    {

        public static string content_d = "";

        public static void Main(string[] args)
        {

            while (true)
            {
                int escolha = 0;

                Console.WriteLine("---------------- Menu ----------------");
                Console.WriteLine("[INFO] Operante: " + Environment.UserName);
                Console.WriteLine("[INFO] Host: https://gateway.marvel.com\n");
                Console.WriteLine("[INFO] Pesquisa: /v1/public/characters?ts=1&apikey={ apikey }&hash={ hash }");
                Console.WriteLine("1 - Bustar todos herois\n");
                Console.WriteLine("[INFO] Pesquisa: /v1/public/characters/{ id }?ts=1&apikey={ apikey }&hash={ hash }");
                Console.WriteLine("2 - Bustar pelo id\n");
                Console.WriteLine("9 - Sair");
                Console.Write("\nEscolha: ");

                try
                {
                    escolha = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[ERRO] " +ex.ToString());
                }

                

                switch (escolha)
                {
                    case 9:
                        Environment.Exit(0);
                        break;

                    case 1:

                        string auxiliar = content_d;
                        Buscar_infos();

                        Console.WriteLine("\n[API INFO] Aguardando a respota...");

                        while (auxiliar == content_d)
                        {
                            Thread.Sleep(1000);
                        }

                        if (content_d != "Falha")
                            Savar();

                        break;

                    case 2:
                        string auxiliar1 = content_d;

                        string id = "0";
                        Console.Write("Digite o  id: ");
                        id = Console.ReadLine();

                        Buscar_infos(id);

                        Console.WriteLine("\n[API INFO] Aguardando a respota...");

                        while (auxiliar1 == content_d)
                        {
                            Thread.Sleep(1000);
                        }

                        if (content_d != "Falha")
                            Savar();

                        break;
                }

                Console.Clear();

            }

        }

        public static void Savar()
        {
            Console.WriteLine("\n[INFO] Salvando na raiz...");
            using (StreamWriter wrt = new StreamWriter("personagensmarvel.txt"))
            {
                wrt.WriteLine(content_d);
            }
            Console.WriteLine("\n[INFO] Arquivo salvo!");
            Console.WriteLine("\n[INFO] Aperte qualquer tecla para voltar ao menu!");
            Console.ReadKey();

        }

        static async Task Buscar_infos(string id = "")
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("https://gateway.marvel.com:443/v1/public/") };

            string busca;

            if (id == "")
                busca = "characters?ts=1&apikey=473da253b3977826288936c4a61c0991&hash=8be15a064f1557728066139e0619aaf6";
            else
                busca = $"characters/{id}?ts=1&apikey=473da253b3977826288936c4a61c0991&hash=8be15a064f1557728066139e0619aaf6";


            var response = await client.GetAsync(busca);
            var content = await response.Content.ReadAsStringAsync();


            Response reponse = JsonConvert.DeserializeObject<Response>(content);

            try
            {
                List<Result> data = reponse.Data.Results;

                string content_txt = "";

                foreach (Result result in data)
                {

                    string comics_names = "";

                    List<ComicsItem> all_comics = result.Comics.Items;

                    foreach (ComicsItem comic_x in all_comics)
                    {
                        if (comics_names == "")
                            comics_names += comic_x.Name;
                        else
                            comics_names += ", " + comic_x.Name;
                    }

                    string series_names = "";

                    List<ComicsItem> all_series = result.Series.Items;

                    foreach (ComicsItem comic_x in all_series)
                    {
                        if (series_names == "")
                            series_names += comic_x.Name;
                        else
                            series_names += ", " + comic_x.Name;
                    }

                    string stories_names = "";

                    List<StoriesItem> all_stories = result.Stories.Items;

                    foreach (StoriesItem storie_x in all_stories)
                    {
                        if (stories_names == "")
                            stories_names += storie_x.Name;
                        else
                            stories_names += ", " + storie_x.Name;
                    }

                    string events_names = "";

                    List<ComicsItem> all_events = result.Events.Items;

                    foreach (ComicsItem event_x in all_series)
                    {
                        if (events_names == "")
                            events_names += event_x.Name;
                        else
                            events_names += ", " + event_x.Name;
                    }

                    content_txt += "Id: " + result.Id +
                                    "\nNome: " + result.Name +
                                    "\nDescrição: " + result.Description +
                                    "\nComics: " + comics_names +
                                    "\nSeries: " + series_names +
                                    "\nStories: " + stories_names +
                                    "\nEvents: " + events_names + "\n";
                }

                Console.WriteLine(content_txt);

                content_d = content_txt;
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
                content_d = "Falha";
            }
        }
    }
}



