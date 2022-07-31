using System.IO;

namespace GitHookCmdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                GitHook.RunPreMergeCommit(args);
            else
            {
                CreateTxtFileMsgCommit();
                GitHook.RunPrepareCommitMsg(args);
            }
        }

        private static void CreateTxtFileMsgCommit()
        {
            if (!File.Exists(GitHook.Path))
                File.WriteAllText(GitHook.Path, string.Empty);
        }
    }
}
