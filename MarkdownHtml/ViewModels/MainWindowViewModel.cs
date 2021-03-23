using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using Markdig;
using Prism.Mvvm;

namespace MarkdownHtml.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _html;
        public string Html
        {
            get => _html;
            set => SetProperty(ref _html, value);
        }

        public MainWindowViewModel()
        {
            var changelogText = ReadChangelogFile($"{GetDebugFolderPath()}/Resources/CHANGELOG.md");
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            CreateHtmlFile("changelog.html", Markdown.ToHtml(changelogText, pipeline));
        }

        private void CreateHtmlFile(string path, string html)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(html);
                }
            }
        }

        /// <summary>
        /// Reads the CHANGELOG markdown
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ReadChangelogFile(string path)
        {
            return !File.Exists(path) ? $"Failed to generate the desired page.\n\nMarkdown file wasn't found at '{path}'." : File.ReadAllText(path);
        }

        /// <summary>
        /// Retrieves the path to the debug folder that runs the application.
        /// </summary>
        /// <returns>Path to folder.</returns>
        private string GetDebugFolderPath()
        {
            return $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)?.Substring(6)}";
        }
    }
}