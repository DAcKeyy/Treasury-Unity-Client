/* MIT License
Copyright (c) 2016 RedBlueGames
Code written by Doug Cox
*/

using System;
using System.Diagnostics;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Treasury.Editor
{
    /// <summary>
    /// GitException includes the error output from a Git.Run() command as well as the
    /// ExitCode it returned.
    /// </summary>
    public class GitException : InvalidOperationException
    {
        public GitException(int exitCode, string errors) : base(errors) =>
            this.ExitCode = exitCode;

        /// <summary>
        /// The exit code returned when running the Git command.
        /// </summary>
        public readonly int ExitCode;
    }
    
    public static class GitBranchToProjectVersionExtention 
    {
    
        public static string BuildVersion
        {
            get
            {
                var version = Run(@"describe --tags --long --match ""v[0-9]*""");
                // Remove initial 'v' and ending git commit hash.
                version = version.Replace('-', '.');
                version = version.Substring(1, version.LastIndexOf('.') - 1);
                return version;
            }
        }

        /// <summary>
        /// The currently active branch.
        /// </summary>
        public static string Branch => Run(@"rev-parse --short HEAD");

        /// <summary>
        /// Returns a listing of all uncommitted or untracked (added) files.
        /// </summary>
        public static string Status => Run(@"status --porcelain");


        /* Methods ================================================================================================================ */

        /// <summary>
        /// Runs git.exe with the specified arguments and returns the output.
        /// </summary>
        public static string Run(string arguments)
        {
            using (var process = new System.Diagnostics.Process())
            {
                var exitCode = process.Run(@"git", arguments, Application.dataPath,
                    out var output, out var errors);
                if (exitCode == 0)
                {
                    return output;
                }
                else
                {
                    throw new GitException(exitCode, errors);
                }
            }
        }
    }
    
    public static class ProcessExtensions
    {
        /* Properties ============================================================================================================= */

        /* Methods ================================================================================================================ */

        /// <summary>
        /// Runs the specified process and waits for it to exit. Its output and errors are
        /// returned as well as the exit code from the process.
        /// See: https://stackoverflow.com/questions/4291912/process-start-how-to-get-the-output
        /// Note that if any deadlocks occur, read the above thread (cubrman's response).
        /// </summary>
        /// <remarks>
        /// This should be run from a using block and disposed after use. It won't 
        /// work properly to keep it around.
        /// </remarks>
        public static int Run(this Process process, string application,
            string arguments, string workingDirectory, out string output,
            out string errors )
        {
            process.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = application,
                Arguments = arguments,
                WorkingDirectory = workingDirectory
            };

            // Use the following event to read both output and errors output.
            var outputBuilder = new StringBuilder();
            var errorsBuilder = new StringBuilder();
            process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (_, args) => errorsBuilder.AppendLine(args.Data);

            // Start the process and wait for it to exit.
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            output = outputBuilder.ToString().TrimEnd();
            errors = errorsBuilder.ToString().TrimEnd();
            return process.ExitCode;
        }
    }

    
    [InitializeOnLoad]
    public static class InitializeOnLoad
    {
        static InitializeOnLoad()
        {
            PlayerSettings.bundleVersion = $"{System.DateTime.Now.Year}.{System.DateTime.Now.Month}.{System.DateTime.Now.Day}.{GitBranchToProjectVersionExtention.Branch}";
            
            EditorApplication.quitting += () =>
            {
                PlayerSettings.bundleVersion = $"{System.DateTime.Now.Year}.{System.DateTime.Now.Month}.{System.DateTime.Now.Day}.{GitBranchToProjectVersionExtention.Branch}";
            };
        }
    }
}
