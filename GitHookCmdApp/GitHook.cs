using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using GitHookCmdApp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace GitHookCmdApp
{
    public class GitHook
    {
        public const string Path = ".git/msg_commit.txt";
        private const string History = "history.txt";
        private const string Url = "http://35.237.131.8:7999";
        private const string Token = "c2FsaWtpbg==.NDgtMA==.mYK7lgtomcKc2KVcNVPE63EZovZBU5";

        public static void RunPrepareCommitMsg(IList<string> args)
        {
            var merge = args.FirstOrDefault(f => f.ToLower().Contains("merge"));
            if (merge != null)
            {
                var branch = File.ReadAllText(args[0]).Replace("Merge branch", "").Replace("\'", "").Trim();
                var tags = ExecuteRequest<List<Tags>>($"/api/issues/{branch}/tags?fields=name");
                var issue = ExecuteRequest<Issues>($"/api/issues/{branch}?fields=summary");
                if (tags != null && issue != null && tags.Count > 0)
                {
                    var message = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss") + $" {tags[0].Name}. {issue.Summary}. {branch}";
                    File.WriteAllText(args[0], message, Encoding.UTF8);
                    File.WriteAllText(Path, message);
                }
            }
        }

        public static void RunPreMergeCommit(IList<string> args)
        {
            var message = File.ReadAllText(Path);
            File.AppendAllText(History, message + Environment.NewLine);
            Process.Start("git", "add -A");
        }

        private static T ExecuteRequest<T>(string url)
        {
            try
            {
                T deserialize;
                var request = WebRequest.Create(Url + url);
                request.Headers.Add("access", Token);
                var response = request.GetResponse();

                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                    {
                        var json = reader.ReadToEnd();
                        deserialize = JsonSerializer.Deserialize<T>(json);
                    }
                }

                response.Close();
                return deserialize;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return default;
            }
        }
    }
}
