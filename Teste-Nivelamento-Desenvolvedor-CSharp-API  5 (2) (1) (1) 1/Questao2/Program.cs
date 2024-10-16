using Newtonsoft.Json;
using System.Net;

public class Program
{
    private static readonly string apiUrl = "https://jsonmock.hackerrank.com/api/football_matches";
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages;

        // Substituindo HttpClient por WebClient para utilizar DownloadString de forma síncrona
        using (WebClient client = new WebClient())
        {
            do
            {
                var response = client.DownloadString($"{apiUrl}?year={year}&team1={team}&page={currentPage}");
                var result = JsonConvert.DeserializeObject<ApiResponse>(response);

                // Somar os gols do time como "team1" corrigindo para team1goals
                foreach (var match in result.data)
                {
                    totalGoals += int.Parse(match.teamGoals); // Corrigido para team1goals
                }

                totalPages = result.total_pages;
                currentPage++;

            } while (currentPage <= totalPages);
        }


        return totalGoals;
    }

    public class ApiResponse
    {
        public int total_pages { get; set; }
        public List<Match> data { get; set; }
    }

    public class Match
    {
        public string teamGoals { get; set; }
    }
}

