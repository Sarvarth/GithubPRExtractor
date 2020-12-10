using Octokit;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GithubAppSettingDownloader
{
    class Program
    {
        public static readonly string GitHubIdentity = Assembly
            .GetEntryAssembly()
            .GetCustomAttribute<AssemblyProductAttribute>()
            .Product;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var prodHeader = new ProductHeaderValue(GitHubIdentity);
            var credentials = new Credentials("7f4d674999cf17062ca99a2b9c2fdbce56222839");
            var enterpriseUrl = "https://github-rd.carefusion.com/vanguard";
            var client = new GitHubClient(prodHeader, new Uri(enterpriseUrl))
            {
                Credentials = credentials
            };
            //var repos = await client.Repository.GetAllPublic();
            //var repoList = repos.ToList();
            //var dump = repoList.Where(x => x.HtmlUrl.StartsWith("https://github-rd.carefusion.com/vanguard/logistics-"));
            //var prs = await client.PullRequest.Get(1904, 502);
            var prr = new PullRequestRequest();
            prr.State = ItemStateFilter.Closed;

            var orchRepoPRs = await client.PullRequest.GetAllForRepository(1877, prr);
            var filteredPrs = orchRepoPRs.Where(x => x.ClosedAt.Value <= DateTimeOffset.Now && x.ClosedAt.Value >= DateTimeOffset.Now.AddDays(-10));
            var rev = await client.PullRequest.Get(1877, 188);
            var reviewers = await client.PullRequest.Review.GetAll(1877, 188);
            var prsList = await client.PullRequest.ReviewComment.GetAll(1877, 188);
            // Orch - 1855
            Console.ReadLine();
        }
    }
}
